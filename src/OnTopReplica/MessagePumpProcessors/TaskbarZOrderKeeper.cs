using System;
using System.Windows.Forms;
using OnTopReplica.Native;

namespace OnTopReplica.MessagePumpProcessors {

    /// <summary>
    /// Keeps the main form above the Windows taskbar.
    /// </summary>
    /// <remarks>
    /// The taskbar is itself a top-most window and brings itself to the top of the
    /// top-most band whenever it is clicked, covering any other top-most window that
    /// overlaps it. This processor checks the form's z-order whenever the foreground
    /// window changes (plus a periodic fallback check) and, if a taskbar covers the form,
    /// moves the form back directly above it.
    /// The form is only moved when a taskbar actually covers it, so multiple OnTopReplica
    /// windows never fight each other over the z-order.
    /// </remarks>
    class TaskbarZOrderKeeper : BaseMessagePumpProcessor {

        const int FallbackCheckInterval = 500;

        //Guards against looping forever if the z-order changes while it is being walked
        const int MaxWindowsToScan = 4096;

        Timer _fallbackTimer;
        IntPtr _foregroundHook = IntPtr.Zero;

        //Must be kept referenced while the hook is active, otherwise it would be garbage collected
        HookMethods.WinEventProc _foregroundHookProc;

        public override void Initialize(MainForm form) {
            base.Initialize(form);

            _foregroundHookProc = new HookMethods.WinEventProc(ForegroundChangedCallback);
            _foregroundHook = HookMethods.SetWinEventHook(
                HookMethods.EVENT_SYSTEM_FOREGROUND, HookMethods.EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero, _foregroundHookProc, 0, 0,
                HookMethods.WINEVENT_OUTOFCONTEXT | HookMethods.WINEVENT_SKIPOWNPROCESS);
            if (_foregroundHook == IntPtr.Zero) {
                Log.Write("Failed to register foreground change event hook");
            }

            _fallbackTimer = new Timer {
                Interval = FallbackCheckInterval
            };
            _fallbackTimer.Tick += FallbackTimer_Tick;
            _fallbackTimer.Start();
        }

        public override bool Process(ref Message msg) {
            return false;
        }

        private void ForegroundChangedCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint idEventThread, uint dwmsEventTime) {
            EnsureAboveTaskbar();
        }

        private void FallbackTimer_Tick(object sender, EventArgs e) {
            EnsureAboveTaskbar();
        }

        /// <summary>
        /// Moves the form directly above the taskbar if a taskbar is currently covering it.
        /// </summary>
        private void EnsureAboveTaskbar() {
            if (Form == null || Form.IsDisposed || !Form.IsHandleCreated)
                return;

            //Only applies while the form is visibly "always on top" (fullscreen mode disables TopMost)
            if (!Form.TopMost || !Form.Visible || Form.Opacity <= 0.0 || Form.WindowState == FormWindowState.Minimized)
                return;

            var taskbar = FindHighestCoveringTaskbar(Form.Handle);
            if (taskbar == IntPtr.Zero)
                return;

            //Slot the form in right above the taskbar (i.e. below whatever sits on top of the taskbar),
            //so that taskbar popups such as jump lists and thumbnail previews stay above the form
            var insertAfter = WindowManagerMethods.GetWindow(taskbar, WindowManagerMethods.GetWindowMode.GW_HWNDPREV);
            if (insertAfter == IntPtr.Zero) {
                insertAfter = WindowManagerMethods.HWND_TOPMOST;
            }

            WindowManagerMethods.SetWindowPos(Form.Handle, insertAfter, 0, 0, 0, 0,
                WindowManagerMethods.SetWindowPosFlags.SWP_NOMOVE |
                WindowManagerMethods.SetWindowPosFlags.SWP_NOSIZE |
                WindowManagerMethods.SetWindowPosFlags.SWP_NOACTIVATE);
        }

        /// <summary>
        /// Walks the z-order upwards from a window and returns the highest visible taskbar
        /// that overlaps it, or <see cref="IntPtr.Zero"/> if the window is not covered by any taskbar.
        /// </summary>
        private static IntPtr FindHighestCoveringTaskbar(IntPtr hwnd) {
            NRectangle nativeBounds;
            if (!WindowMethods.GetWindowRect(hwnd, out nativeBounds))
                return IntPtr.Zero;
            var bounds = nativeBounds.ToRectangle();

            var found = IntPtr.Zero;
            var current = hwnd;
            for (int i = 0; i < MaxWindowsToScan; ++i) {
                current = WindowManagerMethods.GetWindow(current, WindowManagerMethods.GetWindowMode.GW_HWNDPREV);
                if (current == IntPtr.Zero)
                    break;

                if (!WindowManagerMethods.IsWindowVisible(current) || !IsTaskbar(current))
                    continue;

                NRectangle taskbarBounds;
                if (WindowMethods.GetWindowRect(current, out taskbarBounds) &&
                    taskbarBounds.ToRectangle().IntersectsWith(bounds)) {
                    found = current;
                }
            }

            return found;
        }

        private static bool IsTaskbar(IntPtr hwnd) {
            var windowClass = WindowMethods.GetWindowClass(hwnd);
            return windowClass == "Shell_TrayWnd" ||         //primary monitor taskbar
                   windowClass == "Shell_SecondaryTrayWnd";  //taskbars on other monitors
        }

        protected override void Shutdown() {
            if (_fallbackTimer != null) {
                _fallbackTimer.Stop();
                _fallbackTimer.Dispose();
                _fallbackTimer = null;
            }

            if (_foregroundHook != IntPtr.Zero) {
                if (!HookMethods.UnhookWinEvent(_foregroundHook)) {
                    Log.Write("Failed to unhook foreground change event hook");
                }
                _foregroundHook = IntPtr.Zero;
            }
            _foregroundHookProc = null;
        }

    }

}
