using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OnTopReplica.Native;
using OnTopReplica.Properties;

namespace OnTopReplica.MessagePumpProcessors {

    /// <summary>
    /// Predefined color categories for detection.
    /// </summary>
    public enum ColorCategory {
        None,
        Red,    // 红色
        Orange, // 橙色
        Gray    // 灰色
    }

    /// <summary>
    /// Monitors the cloned window for predefined color categories and triggers an alarm when detected.
    /// Uses LockBits for fast pixel scanning to avoid blocking the UI thread.
    /// </summary>
    class ColorDetectionProcessor : BaseMessagePumpProcessor {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSrc, int xSrc, int ySrc, uint rop);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        private const uint SRCCOPY = 0x00CC0020;

        // PW_RENDERFULLCONTENT = 2: full rendering including DirectComposition content
        private const uint PW_RENDERFULLCONTENT = 2;
        // PW_CLIENTONLY = 1: only client area
        private const uint PW_CLIENTONLY = 1;

        private bool _enabled = false;
        private HashSet<ColorCategory> _enabledCategories = new HashSet<ColorCategory>() { ColorCategory.Red };
        private int _sampleInterval = 500; // Sampling interval in milliseconds
        private float _alarmVolume = 1.0f; // 0.0 - 1.0
        private string _alarmSoundFile = string.Empty;
        private volatile bool _alarmActive = false;
        private System.Threading.Timer _alarmStopTimer = null; // fires exactly AlarmDuration ms after alarm starts
        private System.Threading.Thread _detectionThread = null; // background detection thread (independent of message pump)
        private volatile bool _detectionRunning = false;
        private System.Windows.Threading.Dispatcher _uiDispatcher = null; // captured on UI thread in Initialize()
        private const int AlarmDuration = 3000; // 3 seconds in milliseconds
        // Average-color detection: thresholds for comparing average HSV against target colors
        // These are intentionally loose because averaging many pixels produces a muted blended color
        // --- Red ---
        // (kept for reference but no longer used in per-pixel mode)
        // --- Gray density threshold ---
        // Gray triggers only when grayPixels / totalPixels >= this %, preventing scrollbar/border false alarms.
        // Red/Orange trigger on even 1 matching pixel (colored backgrounds almost never exist).
        private const int GrayMinDensityPct = 8; // gray pixels must be >= 8% of total region pixels
        // Minimum absolute non-background pixel count to proceed with detection.
        // Set to 1: any region with at least 1 valid pixel should be evaluated.
        private const int MinNonBgPixels = 1; // absolute pixel count
        private System.Windows.Media.MediaPlayer _mediaPlayer;
        private bool _selfTestRun = false; // Run classification self-test once on first enabled cycle

        public bool Enabled {
            get { return _enabled; }
            set {
                bool wasDisabled = !_enabled;
                _enabled = value;
                // Run classification self-test once on first enable (regardless of window/region state)
                if (value && wasDisabled && !_selfTestRun) {
                    _selfTestRun = true;
                    RunClassificationSelfTest();
                }
                // Start or stop the background detection thread
                if (value) {
                    StartDetectionThread();
                } else {
                    StopDetectionThread();
                }
            }
        }

        /// <summary>
        /// Set of color categories that should trigger the alarm when detected.
        /// </summary>
        public HashSet<ColorCategory> EnabledCategories {
            get { return _enabledCategories; }
            set { _enabledCategories = value ?? new HashSet<ColorCategory>(); }
        }

        public int SampleInterval {
            get { return _sampleInterval; }
            set { _sampleInterval = Math.Max(100, value); }
        }

        public bool IsAlarmActive {
            get { return _alarmActive; }
        }

        public float AlarmVolume {
            get { return _alarmVolume; }
            set { _alarmVolume = Math.Max(0, Math.Min(1, value)); }
        }

        public string AlarmSoundFile {
            get { return _alarmSoundFile; }
            set { _alarmSoundFile = value; }
        }

        public override void Initialize(MainForm form) {
            base.Initialize(form);
            // Capture WPF Dispatcher on the UI thread so background threads can marshal
            // MediaPlayer calls to the correct thread (Application.Current is null in WinForms).
            _uiDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
        }

        public override bool Process(ref Message msg) {
            // Detection is handled by the background thread (_detectionThread).
            // The message pump is no longer needed for color detection timing.
            return false;
        }

        /// <summary>
        /// Starts the background detection thread if not already running.
        /// </summary>
        private void StartDetectionThread() {
            if (_detectionThread != null && _detectionThread.IsAlive)
                return;
            _detectionRunning = true;
            _detectionThread = new System.Threading.Thread(DetectionThreadLoop) {
                IsBackground = true,
                Name = "ColorDetection"
            };
            _detectionThread.Start();
            Log.Write("ColorDetection: background thread started (interval={0}ms)", _sampleInterval);
        }

        /// <summary>
        /// Signals the background detection thread to stop.
        /// </summary>
        private void StopDetectionThread() {
            _detectionRunning = false;
            // Thread exits on next sleep cycle — no Join needed (IsBackground=true)
            Log.Write("ColorDetection: background thread stop requested");
        }

        /// <summary>
        /// Background detection loop. Runs every _sampleInterval ms independent of the WinForms
        /// message pump, so detection latency stays at ~500 ms even when a fullscreen game
        /// (such as EVE Online) reduces message pump frequency to once per ~6 s.
        /// </summary>
        private void DetectionThreadLoop() {
            while (_detectionRunning) {
                System.Threading.Thread.Sleep(_sampleInterval);
                if (!_detectionRunning) break;
                if (!_enabled) break;
                if (Form == null || Form.CurrentThumbnailWindowHandle == null) continue;
                if (_enabledCategories.Count == 0) continue;
                if (_alarmActive) continue;

                var catList = string.Join(",", _enabledCategories);
                Log.Write("Performing color detection (categories={0})", catList);
                try {
                    if (DetectColorInWindow(Form.CurrentThumbnailWindowHandle.Handle)) {
                        StartAlarm();
                    }
                } catch (Exception ex) {
                    Log.Write("ColorDetection thread error: {0}", ex.Message);
                }
            }
            Log.Write("ColorDetection: background thread exited");
        }

        /// <summary>
        /// Detects if the target color exists in the monitored window.
        /// Uses PrintWindow API to capture the source window directly (avoids occlusion issues).
        /// </summary>
        private bool DetectColorInWindow(IntPtr windowHandle)
        {
            try
            {
                // 获取窗口客户区大小
                Rectangle clientRect;
                if (!WindowManagerMethods.GetClientRect(windowHandle, out clientRect))
                {
                    Log.Write("ColorDetection: Failed to get client rect");
                    return false;
                }

                Size clientSize = new Size(clientRect.Width, clientRect.Height);
                if (clientSize.Width <= 0 || clientSize.Height <= 0)
                {
                    Log.Write("ColorDetection: Invalid client size {0}x{1}", clientSize.Width, clientSize.Height);
                    return false;
                }

                // 获取主窗体实例
                MainForm mainForm = null;
                foreach (Form form in Application.OpenForms)
                {
                    if (form is MainForm mf)
                    {
                        mainForm = mf;
                        break;
                    }
                }

                Rectangle regionRect;

                // 优先使用选定区域
                if (mainForm != null && mainForm.SelectedThumbnailRegion != null)
                {
                    var selectedRegion = mainForm.SelectedThumbnailRegion;
                    regionRect = selectedRegion.ComputeRegionRectangle(clientSize);
                    Log.Write("ColorDetection: Region={0},{1} {2}x{3} (relative={4})",
                        regionRect.X, regionRect.Y, regionRect.Width, regionRect.Height, selectedRegion.Relative);
                }
                else
                {
                    regionRect = clientRect;
                    Log.Write("ColorDetection: No region, full window {0}x{1}", clientSize.Width, clientSize.Height);
                }

                if (regionRect.Width <= 0 || regionRect.Height <= 0)
                {
                    Log.Write("ColorDetection: Invalid region {0}x{1}", regionRect.Width, regionRect.Height);
                    return false;
                }

                // 使用 PrintWindow 捕获窗口内容（不受遮挡影响）
                Bitmap windowBmp = null;
                Bitmap regionBmp = null;
                try
                {
                    // 获取完整窗口大小（包含非客户区）
                    // 注意: GetWindowRect 返回 RECT (left,top,right,bottom) 映射到 Rectangle (X,Y,Width=right,Height=bottom)
                    Rectangle windowRect;
                    WindowManagerMethods.GetWindowRect(windowHandle, out windowRect);
                    int windowWidth = windowRect.Width - windowRect.X;
                    int windowHeight = windowRect.Height - windowRect.Y;

                    if (windowWidth <= 0 || windowHeight <= 0)
                    {
                        Log.Write("ColorDetection: Invalid window size {0}x{1} (rect={2},{3},{4},{5})",
                            windowWidth, windowHeight, windowRect.X, windowRect.Y, windowRect.Width, windowRect.Height);
                        return false;
                    }

                    // 创建窗口大小的位图，用 PrintWindow 捕获
                    windowBmp = new Bitmap(windowWidth, windowHeight, PixelFormat.Format32bppArgb);
                    bool printSuccess = false;

                    // 尝试不同的 PrintWindow flags
                    uint[] flags = new uint[] { PW_RENDERFULLCONTENT, PW_CLIENTONLY, 0 };
                    foreach (uint flag in flags)
                    {
                        using (Graphics g = Graphics.FromImage(windowBmp))
                        {
                            IntPtr hdc = g.GetHdc();
                            try
                            {
                                printSuccess = PrintWindow(windowHandle, hdc, flag);
                            }
                            finally
                            {
                                g.ReleaseHdc(hdc);
                            }
                        }
                        if (printSuccess)
                        {
                            Log.Write("ColorDetection: PrintWindow OK with flag={0}, winSize={1}x{2}", flag, windowWidth, windowHeight);
                            break;
                        }
                    }

                    if (!printSuccess)
                    {
                        Log.Write("ColorDetection: PrintWindow failed (all flags), falling back to CopyFromScreen");
                        windowBmp.Dispose();
                        windowBmp = null;
                        return DetectColorFallback(windowHandle, regionRect);
                    }

                    // 计算客户区在窗口位图中的偏移量
                    var clientOriginScreen = WindowManagerMethods.ClientToScreen(windowHandle, new NPoint(0, 0));
                    int clientOffsetX = clientOriginScreen.X - windowRect.X;
                    int clientOffsetY = clientOriginScreen.Y - windowRect.Y;

                    // 将区域坐标偏移到窗口位图坐标
                    int cropX = clientOffsetX + regionRect.X;
                    int cropY = clientOffsetY + regionRect.Y;
                    int cropW = Math.Min(regionRect.Width, windowWidth - cropX);
                    int cropH = Math.Min(regionRect.Height, windowHeight - cropY);

                    Log.Write("ColorDetection: PrintWindow OK, winSize={0}x{1}, clientOffset=({2},{3}), crop=({4},{5} {6}x{7})",
                        windowWidth, windowHeight, clientOffsetX, clientOffsetY, cropX, cropY, cropW, cropH);

                    if (cropW <= 0 || cropH <= 0)
                    {
                        Log.Write("ColorDetection: crop region empty");
                        return false;
                    }

                    // 裁剪出选定区域
                    regionBmp = windowBmp.Clone(new Rectangle(cropX, cropY, cropW, cropH), PixelFormat.Format32bppArgb);

                    // 保存调试截图（每20次保存一次，避免IO过多）
                    _debugCounter++;
                    if (_debugCounter % 20 == 1)
                    {
                        SaveDebugBitmap(regionBmp, "region_capture");
                    }

                    return SampleBitmapForColor(regionBmp);
                }
                finally
                {
                    if (regionBmp != null)
                        regionBmp.Dispose();
                    if (windowBmp != null)
                        windowBmp.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Write("ColorDetection Error: {0}\n{1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        private int _debugCounter = 0;

        /// <summary>
        /// Saves a bitmap to the AppData folder for debugging.
        /// </summary>
        private void SaveDebugBitmap(Bitmap bmp, string prefix)
        {
            try
            {
                string dir = AppPaths.PrivateRoamingFolderPath;
                if (string.IsNullOrEmpty(dir)) return;
                string path = Path.Combine(dir, prefix + "_debug.png");
                bmp.Save(path, ImageFormat.Png);
                Log.Write("ColorDetection: Debug bitmap saved to {0}", path);
            }
            catch (Exception ex)
            {
                Log.Write("ColorDetection: Failed to save debug bitmap: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Fallback: use BitBlt from source window DC to capture client area directly.
        /// This avoids CopyFromScreen which captures the OnTopReplica overlay.
        /// If BitBlt returns all-black image, falls back to CopyFromScreen with opacity trick.
        /// </summary>
        private bool DetectColorFallback(IntPtr windowHandle, Rectangle regionRect)
        {
            Log.Write("ColorDetection Fallback(BitBlt): region={0},{1} {2}x{3}",
                regionRect.X, regionRect.Y, regionRect.Width, regionRect.Height);

            if (regionRect.Width <= 0 || regionRect.Height <= 0)
                return false;

            int cropW = Math.Min(regionRect.Width, 800);
            int cropH = Math.Min(regionRect.Height, 600);

            // 方案1: 使用 BitBlt + GetDC 直接从源窗口客户区捕获（无闪烁）
            IntPtr hdcSrc = IntPtr.Zero;
            IntPtr hdcMem = IntPtr.Zero;
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBmp = IntPtr.Zero;
            Bitmap bmp = null;
            try
            {
                hdcSrc = GetDC(windowHandle);
                if (hdcSrc != IntPtr.Zero)
                {
                    hdcMem = CreateCompatibleDC(hdcSrc);
                    hBitmap = CreateCompatibleBitmap(hdcSrc, cropW, cropH);
                    hOldBmp = SelectObject(hdcMem, hBitmap);

                    bool ok = BitBlt(hdcMem, 0, 0, cropW, cropH, hdcSrc, regionRect.X, regionRect.Y, SRCCOPY);
                    SelectObject(hdcMem, hOldBmp);

                    if (ok)
                    {
                        bmp = Bitmap.FromHbitmap(hBitmap);

                        // 检查是否全黑（硬件渲染窗口可能返回黑色图像）
                        if (!IsBitmapAllBlack(bmp))
                        {
                            Log.Write("ColorDetection Fallback: BitBlt OK, size={0}x{1}", cropW, cropH);
                            _debugCounter++;
                            if (_debugCounter % 20 == 1)
                                SaveDebugBitmap(bmp, "bitblt_capture");
                            return SampleBitmapForColor(bmp);
                        }
                        else
                        {
                            Log.Write("ColorDetection Fallback: BitBlt returned all-black, trying CopyFromScreen");
                            bmp.Dispose();
                            bmp = null;
                        }
                    }
                    else
                    {
                        Log.Write("ColorDetection Fallback: BitBlt failed");
                    }
                }
                else
                {
                    Log.Write("ColorDetection Fallback: GetDC failed");
                }
            }
            catch (Exception ex)
            {
                Log.Write("ColorDetection Fallback BitBlt error: {0}", ex.Message);
                if (bmp != null) { bmp.Dispose(); bmp = null; }
            }
            finally
            {
                if (hBitmap != IntPtr.Zero) DeleteObject(hBitmap);
                if (hdcMem != IntPtr.Zero) DeleteDC(hdcMem);
                if (hdcSrc != IntPtr.Zero) ReleaseDC(windowHandle, hdcSrc);
            }

            // 方案2: CopyFromScreen（最后手段，可能包含覆盖层但不会闪烁）
            return DetectColorScreenCapture(windowHandle, regionRect);
        }

        /// <summary>
        /// Last resort capture. Tries to capture what OnTopReplica's ThumbnailPanel is *visually
        /// showing* on screen — first via PrintWindow(PW_RENDERFULLCONTENT), then via
        /// CopyFromScreen at the panel's own screen coordinates (DWM always renders it correctly
        /// there regardless of source window occlusion). Never falls back to the source window's
        /// screen coords, which would capture whatever is covering it.
        /// </summary>
        private bool DetectColorScreenCapture(IntPtr windowHandle, Rectangle regionRect)
        {
            // 获取 ThumbnailPanel 的句柄和屏幕坐标（必须在 UI 线程上访问）
            MainForm mainForm = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm mf) { mainForm = mf; break; }
            }

            if (mainForm != null)
            {
                IntPtr panelHandle = IntPtr.Zero;
                int panelW = 0, panelH = 0;
                Rectangle panelScreenRect = Rectangle.Empty;

                try
                {
                    mainForm.Invoke((Action)(() =>
                    {
                        var panel = mainForm.ThumbnailPanel;
                        if (panel != null && panel.IsHandleCreated && panel.Width > 0 && panel.Height > 0)
                        {
                            panelHandle = panel.Handle;
                            panelW = panel.Width;
                            panelH = panel.Height;
                            // 获取 panel 在屏幕上的实际坐标（DWM 在此位置渲染缩略图）
                            Point screenPt = panel.PointToScreen(Point.Empty);
                            panelScreenRect = new Rectangle(screenPt.X, screenPt.Y, panelW, panelH);
                        }
                    }));
                }
                catch { /* 窗口可能在关闭，忽略 */ }

                if (panelHandle != IntPtr.Zero)
                {
                    // --- 方案1: PrintWindow(ThumbnailPanel) ---
                    IntPtr hdcMem = IntPtr.Zero;
                    IntPtr hBitmap = IntPtr.Zero;
                    IntPtr hOldBmp = IntPtr.Zero;
                    IntPtr hdcScreen = IntPtr.Zero;
                    Bitmap bmp = null;
                    try
                    {
                        hdcScreen = GetDC(IntPtr.Zero);
                        hdcMem = CreateCompatibleDC(hdcScreen);
                        hBitmap = CreateCompatibleBitmap(hdcScreen, panelW, panelH);
                        hOldBmp = SelectObject(hdcMem, hBitmap);
                        bool ok = PrintWindow(panelHandle, hdcMem, PW_RENDERFULLCONTENT);
                        SelectObject(hdcMem, hOldBmp);

                        if (ok)
                        {
                            bmp = Bitmap.FromHbitmap(hBitmap);
                            if (!IsBitmapAllBlack(bmp))
                            {
                                Log.Write("ColorDetection ThumbnailPanel: PrintWindow OK, size={0}x{1}", panelW, panelH);
                                _debugCounter++;
                                if (_debugCounter % 5 == 1)
                                    SaveDebugBitmap(bmp, "thumbnail_panel_capture");
                                return SampleBitmapForColor(bmp);
                            }
                            Log.Write("ColorDetection ThumbnailPanel: PrintWindow all-black, trying panel CopyFromScreen");
                        }
                        else
                        {
                            Log.Write("ColorDetection ThumbnailPanel: PrintWindow failed, trying panel CopyFromScreen");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("ColorDetection ThumbnailPanel PrintWindow error: {0}", ex.Message);
                    }
                    finally
                    {
                        if (bmp != null) bmp.Dispose();
                        if (hBitmap != IntPtr.Zero) DeleteObject(hBitmap);
                        if (hdcMem != IntPtr.Zero) DeleteDC(hdcMem);
                        if (hdcScreen != IntPtr.Zero) ReleaseDC(IntPtr.Zero, hdcScreen);
                    }

                    // --- 方案2: CopyFromScreen 截取 ThumbnailPanel 的屏幕坐标 ---
                    // DWM 始终在 panel 的屏幕位置渲染源窗口内容，此方法比截源窗口坐标更可靠。
                    // 向内缩进 PANEL_INSET 像素，跳过可能含窗口边框/装饰的边缘像素。
                    if (!panelScreenRect.IsEmpty)
                    {
                        const int PANEL_INSET = 3;
                        int insetX = panelScreenRect.X + PANEL_INSET;
                        int insetY = panelScreenRect.Y + PANEL_INSET;
                        int insetW = Math.Max(1, panelScreenRect.Width - PANEL_INSET * 2);
                        int insetH = Math.Max(1, panelScreenRect.Height - PANEL_INSET * 2);
                        int maxW = Math.Min(insetW, 800);
                        int maxH = Math.Min(insetH, 600);
                        Bitmap bmpPanel = null;
                        try
                        {
                            bmpPanel = new Bitmap(maxW, maxH, PixelFormat.Format32bppArgb);
                            using (Graphics g = Graphics.FromImage(bmpPanel))
                            {
                                g.CopyFromScreen(insetX, insetY, 0, 0, new Size(maxW, maxH));
                            }
                            Log.Write("ColorDetection ThumbnailPanel: CopyFromScreen screen={0},{1} {2}x{3} (inset={4})",
                                insetX, insetY, maxW, maxH, PANEL_INSET);
                            _debugCounter++;
                            if (_debugCounter % 5 == 1)
                                SaveDebugBitmap(bmpPanel, "thumbnail_panel_screen");
                            return SampleBitmapForColor(bmpPanel);
                        }
                        catch (Exception ex)
                        {
                            Log.Write("ColorDetection ThumbnailPanel CopyFromScreen error: {0}", ex.Message);
                        }
                        finally
                        {
                            if (bmpPanel != null) bmpPanel.Dispose();
                        }
                    }
                }
            }

            // 无法通过 ThumbnailPanel 获取内容，跳过本次检测（避免对源窗口坐标截图产生误报）
            Log.Write("ColorDetection: ThumbnailPanel unavailable, skipping detection to avoid false alarm");
            return false;
        }

        /// <summary>
        /// Checks if a bitmap is nearly black (indicating failed capture from hardware-rendered window).
        /// A pixel is considered "black" if all channels are &lt;= threshold.
        /// Returns true if more than 50% of sampled pixels are near-black.
        /// Hardware-rendered windows often return mostly-black images with scattered noise.
        /// </summary>
        private bool IsBitmapAllBlack(Bitmap bmp)
        {
            if (bmp == null) return true;
            const int threshold = 15; // 硬件渲染窗口 BitBlt 可能返回 (3,3,3) 等近黑像素
            int stepX = Math.Max(1, bmp.Width / 10);
            int stepY = Math.Max(1, bmp.Height / 10);
            int totalSamples = 0;
            int blackSamples = 0;
            for (int y = 0; y < bmp.Height; y += stepY)
            {
                for (int x = 0; x < bmp.Width; x += stepX)
                {
                    Color c = bmp.GetPixel(x, y);
                    totalSamples++;
                    if (c.R <= threshold && c.G <= threshold && c.B <= threshold)
                        blackSamples++;
                }
            }
            bool result = totalSamples > 0 && (blackSamples * 100 / totalSamples) >= 50;
            if (result)
                Log.Write("ColorDetection: IsBitmapAllBlack=true ({0}/{1} samples near-black={2}%, threshold={3})", blackSamples, totalSamples, blackSamples * 100 / totalSamples, threshold);
            return result;
        }

        /// <summary>
        /// Per-pixel color detection algorithm:
        /// 1. Scan all pixels, skip white (S&lt;15 and V&gt;85), black/near-black (V&lt;15),
        ///    and high-saturation non-target colors (blue/green icons, S&gt;=40 and not Red/Orange/Gray).
        /// 2. Classify each remaining pixel as Red / Orange / Gray / None.
        /// 3a. Red or Orange: if even 1 pixel matches an enabled category → alarm immediately.
        /// 3b. Gray only (no red/orange found): grayPixels/totalPixels &gt;= GrayMinDensityPct% → alarm.
        ///     The density guard prevents scrollbars/borders (pure gray background, ~6%) from triggering.
        /// </summary>
        private bool SampleBitmapForColor(Bitmap bmp) {
            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0)
                return false;

            Log.Write("ColorDetection Scan: enabledCategories=[{0}], bmpSize={1}x{2}",
                string.Join(",", _enabledCategories), bmp.Width, bmp.Height);

            BitmapData data = null;
            try {
                data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                int stride = data.Stride;
                int byteCount = stride * bmp.Height;
                byte[] pixels = new byte[byteCount];
                Marshal.Copy(data.Scan0, pixels, 0, byteCount);

                bmp.UnlockBits(data);
                data = null;

                int totalPixels = bmp.Width * bmp.Height;
                int whiteSkipped    = 0;
                int blackSkipped    = 0;
                int coloredBgSkipped = 0; // high-S non-target colors (blue/green icons)
                int redCount        = 0;
                int orangeCount     = 0;
                int grayCount       = 0;
                int noneCount       = 0;

                for (int y = 0; y < bmp.Height; y++) {
                    int rowOffset = y * stride;
                    for (int x = 0; x < bmp.Width; x++) {
                        int idx = rowOffset + x * 4;
                        byte pb = pixels[idx];      // B
                        byte pg = pixels[idx + 1];  // G
                        byte pr = pixels[idx + 2];  // R

                        float h, s, v;
                        RgbToHsv(pr, pg, pb, out h, out s, out v);

                        // Skip white: low saturation + high brightness (icon text, borders)
                        if (s < 15 && v > 85) { whiteSkipped++; continue; }

                        // Skip black/near-black: dark UI background
                        if (v < 15) { blackSkipped++; continue; }

                        // Classify
                        ColorCategory cat = ClassifyPixelColor(pr, pg, pb);

                        // Exclude high-S non-target colors (blue/green icons) from analysis
                        if (cat == ColorCategory.None && s >= 40) { coloredBgSkipped++; continue; }

                        switch (cat) {
                            case ColorCategory.Red:    redCount++;    break;
                            case ColorCategory.Orange: orangeCount++; break;
                            case ColorCategory.Gray:   grayCount++;   break;
                            default:                   noneCount++;   break;
                        }
                    }
                }

                int validPixels = redCount + orangeCount + grayCount + noneCount;
                int grayDensityPct = totalPixels > 0 ? grayCount * 100 / totalPixels : 0;

                Log.Write("ColorDetection counts: red={0} orange={1} gray={2} none={3}  total={4} white={5} black={6} coloredBg={7}",
                    redCount, orangeCount, grayCount, noneCount, totalPixels, whiteSkipped, blackSkipped, coloredBgSkipped);
                Log.Write("  grayDensity={0}%(need \u2265{1}% for gray alarm)", grayDensityPct, GrayMinDensityPct);

                // --- Rule 1: Red or Orange — even 1 pixel triggers alarm ---
                if (_enabledCategories.Contains(ColorCategory.Red) && redCount >= 1) {
                    Log.Write("ColorDetection MATCH: Red, {0} pixel(s)", redCount);
                    SaveDebugBitmap(bmp, "alarm_trigger");
                    return true;
                }
                if (_enabledCategories.Contains(ColorCategory.Orange) && orangeCount >= 1) {
                    Log.Write("ColorDetection MATCH: Orange, {0} pixel(s)", orangeCount);
                    SaveDebugBitmap(bmp, "alarm_trigger");
                    return true;
                }

                // --- Rule 2: Gray — only if density >= GrayMinDensityPct% ---
                if (_enabledCategories.Contains(ColorCategory.Gray)) {
                    if (grayDensityPct >= GrayMinDensityPct) {
                        Log.Write("ColorDetection MATCH: Gray, {0}px density={1}%", grayCount, grayDensityPct);
                        SaveDebugBitmap(bmp, "alarm_trigger");
                        return true;
                    } else {
                        Log.Write("ColorDetection no-Gray: {0}px density={1}% < {2}%", grayCount, grayDensityPct, GrayMinDensityPct);
                    }
                }

                Log.Write("ColorDetection NO MATCH");
                return false;
            }
            finally {
                if (data != null) bmp.UnlockBits(data);
            }
        }

        /// <summary>
        /// Classifies a pixel's RGB color into a predefined color category using HSV ranges.
        /// Red:    H in [0,15] or [345,360], S >= 40%, V >= 25%  (red icons with dark edges)
        /// Orange: H in (15,55],              S >= 40%, V >= 25%  (orange icons with dark edges)
        /// Gray:   S < 20%, V in [15,75%]   (gray icons, broad range to cover dark→medium gray)
        /// White/black pixels should be pre-filtered before calling this.
        /// </summary>
        private static ColorCategory ClassifyPixelColor(byte r, byte g, byte b) {
            float h, s, v;
            RgbToHsv(r, g, b, out h, out s, out v);

            // Red: hue near 0° with decent saturation
            if (s >= 40 && v >= 25) {
                if (h <= 15 || h >= 345) {
                    return ColorCategory.Red;
                }
                // Orange: hue from reddish-orange to yellow-orange
                if (h > 15 && h <= 55) {
                    return ColorCategory.Orange;
                }
            }

            // Gray: low saturation, covers dark gray (V≈15%) to light gray (V≈83%)
            // White (V>85, S<15) and black (V<15) are pre-filtered in the scan loop.
            // S<25 allows slight rendering tint on gray icon backgrounds while excluding beige/warm tones (S≥25)
            if (s < 25 && v >= 15 && v <= 83) {
                return ColorCategory.Gray;
            }

            return ColorCategory.None;
        }

        /// <summary>
        /// Converts RGB (0-255) to HSV. H in degrees (0-360), S and V in percent (0-100).
        /// </summary>
        private static void RgbToHsv(int r, int g, int b, out float h, out float s, out float v) {
            float rf = r / 255f, gf = g / 255f, bf = b / 255f;
            float max = Math.Max(rf, Math.Max(gf, bf));
            float min = Math.Min(rf, Math.Min(gf, bf));
            float delta = max - min;

            // Value
            v = max * 100f;

            // Saturation
            if (max < 0.0001f) { h = 0; s = 0; return; }
            s = (delta / max) * 100f;

            // Hue
            if (delta < 0.0001f) { h = 0; return; }
            if (Math.Abs(max - rf) < 0.0001f)
                h = 60f * (((gf - bf) / delta) % 6f);
            else if (Math.Abs(max - gf) < 0.0001f)
                h = 60f * (((bf - rf) / delta) + 2f);
            else
                h = 60f * (((rf - gf) / delta) + 4f);

            if (h < 0) h += 360f;
        }

        /// <summary>
        /// Starts the alarm. Called from the background detection thread — all UI/WPF operations
        /// are dispatched to the WPF dispatcher to ensure thread safety.
        /// </summary>
        private void StartAlarm() {
            if (_alarmActive)
                return;

            _alarmActive = true;

            // Schedule automatic stop after AlarmDuration ms.
            // Independent of both the message pump and the detection thread.
            _alarmStopTimer?.Dispose();
            _alarmStopTimer = new System.Threading.Timer(_ => StopAlarm(), null, AlarmDuration, System.Threading.Timeout.Infinite);

            Log.Write("Color alarm triggered! volume={0}, file={1}", _alarmVolume, _alarmSoundFile);

            // MediaPlayer must be created/used on a thread with a WPF Dispatcher.
            // _uiDispatcher was captured in Initialize() on the UI thread.
            if (!string.IsNullOrEmpty(_alarmSoundFile) && File.Exists(_alarmSoundFile)) {
                var soundFile = _alarmSoundFile;
                var volume = _alarmVolume;
                if (_uiDispatcher != null) {
                    _uiDispatcher.BeginInvoke((Action)(() => {
                        try {
                            if (_mediaPlayer == null)
                                _mediaPlayer = new System.Windows.Media.MediaPlayer();
                            _mediaPlayer.Open(new Uri(soundFile));
                            _mediaPlayer.Volume = volume;
                            _mediaPlayer.Play();
                            Log.Write("MediaPlayer.Play() called for {0}", soundFile);
                        } catch (Exception ex) {
                            Log.Write("Error playing alarm: {0}", ex.Message);
                        }
                    }));
                } else {
                    Log.Write("Warning: _uiDispatcher is null, cannot play sound");
                    System.Media.SystemSounds.Beep.Play();
                }
            } else {
                System.Media.SystemSounds.Beep.Play();
            }
        }

        /// <summary>
        /// Plays an alarm tone for the duration.
        /// </summary>
        private void PlayAlarmTone() {
            try {
                // Generate and play a 1000Hz tone for 3 seconds
                int sampleRate = 44100;
                int duration = 3000; // 3 seconds
                int frequency = 1000; // 1000 Hz

                int numSamples = (sampleRate * duration) / 1000;
                byte[] data = new byte[numSamples * 2]; // 16-bit audio

                for (int i = 0; i < numSamples; i++) {
                    double t = (double)i / sampleRate;
                    short sample = (short)(short.MaxValue * 0.5 * Math.Sin(2 * Math.PI * frequency * t));
                    data[i * 2] = (byte)(sample & 0xFF);
                    data[i * 2 + 1] = (byte)((sample >> 8) & 0xFF);
                }

                // Play using WaveOut
                PlayAudioData(data, sampleRate);
            }
            catch (Exception ex) {
                Log.Write("Error playing alarm tone: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Plays audio data using Wave API.
        /// </summary>
        private void PlayAudioData(byte[] audioData, int sampleRate) {
            try {
                // Use the simple beep method from System.Media
                // For more complex tones, we would need WaveOut API
                for (int i = 0; i < 3; i++) {
                    System.Media.SystemSounds.Beep.Play();
                    System.Threading.Thread.Sleep(200);
                    if (!_alarmActive)
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        /// Stops the alarm.
        /// </summary>
        private void StopAlarm() {
            _alarmActive = false;
            _alarmStopTimer?.Dispose();
            _alarmStopTimer = null;
            Log.Write("Color alarm stopped");
            try {
                if (_mediaPlayer != null && _uiDispatcher != null) {
                    _uiDispatcher.BeginInvoke((Action)(() => {
                        try { _mediaPlayer?.Stop(); } catch { }
                    }));
                }
            }
            catch { }
        }

        /// <summary>
        /// Runs classification self-tests on startup to verify color detection logic.
        /// Results are written to the log for inspection.
        /// </summary>
        private void RunClassificationSelfTest() {
            Log.Write("=== ColorDetection Self-Test BEGIN ===");
            Log.Write("  Algorithm: per-pixel  Red/Orange>=1px→alarm  Gray>=density{0}%→alarm", GrayMinDensityPct);
            Log.Write("  White skip: S<15 && V>85, Black skip: V<15, MinNonBgPixels={0}", MinNonBgPixels);
            // Format: (label, R, G, B, expected category)
            var tests = new[] {
                // --- Red (icon body, various shades including dark edges) ---
                new { Label="Red icon body",  R=(byte)180, G=(byte)30,  B=(byte)30,  Expect=ColorCategory.Red    },
                new { Label="Deep red edge",  R=(byte)140, G=(byte)20,  B=(byte)20,  Expect=ColorCategory.Red    },
                new { Label="Bright red",     R=(byte)255, G=(byte)20,  B=(byte)10,  Expect=ColorCategory.Red    },
                new { Label="Dark red V=25",  R=(byte)64,  G=(byte)10,  B=(byte)10,  Expect=ColorCategory.Red    },
                // --- Orange (icon body, with dark border shades) ---
                new { Label="Orange body",    R=(byte)200, G=(byte)120, B=(byte)40,  Expect=ColorCategory.Orange },
                new { Label="Bright orange",  R=(byte)255, G=(byte)140, B=(byte)0,   Expect=ColorCategory.Orange },
                new { Label="Dark orange",    R=(byte)160, G=(byte)80,  B=(byte)20,  Expect=ColorCategory.Orange },
                new { Label="Yellow-orange",  R=(byte)255, G=(byte)180, B=(byte)0,   Expect=ColorCategory.Orange },
                // --- Gray (icon body, dark to medium gray) ---
                new { Label="Dark gray V=24", R=(byte)60,  G=(byte)60,  B=(byte)60,  Expect=ColorCategory.Gray   },
                new { Label="Med gray V=50",  R=(byte)128, G=(byte)128, B=(byte)128, Expect=ColorCategory.Gray   },
                new { Label="Gray V=70",      R=(byte)178, G=(byte)178, B=(byte)178, Expect=ColorCategory.Gray   },
                new { Label="Gray V=15",      R=(byte)39,  G=(byte)39,  B=(byte)39,  Expect=ColorCategory.Gray   },
                new { Label="Gray V=80 light",R=(byte)204, G=(byte)204, B=(byte)204, Expect=ColorCategory.Gray   },
                // --- Should NOT match (None) ---
                new { Label="Green H=120",    R=(byte)0,   G=(byte)200, B=(byte)0,   Expect=ColorCategory.None   },
                new { Label="Blue H=240",     R=(byte)0,   G=(byte)0,   B=(byte)200, Expect=ColorCategory.None   },
                new { Label="Yellow H=60",    R=(byte)255, G=(byte)255, B=(byte)0,   Expect=ColorCategory.None   },
                // --- White/Black: these would be pre-filtered in scan, but classify as None ---
                new { Label="White (skip)",   R=(byte)255, G=(byte)255, B=(byte)255, Expect=ColorCategory.None   },
                new { Label="Black (skip)",   R=(byte)0,   G=(byte)0,   B=(byte)0,   Expect=ColorCategory.None   },
                // --- Boundary / edge cases ---
                new { Label="Gray V=76 now ok",R=(byte)194, G=(byte)194, B=(byte)194, Expect=ColorCategory.Gray   },
                new { Label="Gray V=84 above",R=(byte)215, G=(byte)215, B=(byte)215, Expect=ColorCategory.None   },
                new { Label="Gray V=14 below",R=(byte)35,  G=(byte)35,  B=(byte)35,  Expect=ColorCategory.None   },
                new { Label="Low-sat orange", R=(byte)200, G=(byte)180, B=(byte)150, Expect=ColorCategory.None   },
            };

            int pass = 0, fail = 0;
            foreach (var t in tests) {
                var got = ClassifyPixelColor(t.R, t.G, t.B);
                float h, s, v;
                RgbToHsv(t.R, t.G, t.B, out h, out s, out v);
                bool ok = got == t.Expect;
                if (ok) pass++;
                else fail++;
                Log.Write("  [{0}] {1}: rgb=({2},{3},{4}) H={5:F0} S={6:F0}% V={7:F1}% → got={8} expect={9}",
                    ok ? "PASS" : "FAIL", t.Label, t.R, t.G, t.B, h, s, v, got, t.Expect);
            }
            Log.Write("=== ColorDetection Self-Test END: {0} passed, {1} FAILED ===", pass, fail);
        }

        protected override void Shutdown() {
            StopDetectionThread();
            Enabled = false;
            if (_alarmActive)
                StopAlarm();
            if (_mediaPlayer != null && _uiDispatcher != null) {
                try {
                    _uiDispatcher.Invoke((Action)(() => {
                        try { _mediaPlayer?.Close(); } catch { }
                    }));
                } catch { }
                _mediaPlayer = null;
            }
        }
    }
}
