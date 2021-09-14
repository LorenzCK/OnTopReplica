﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using OnTopReplica.WindowSeekers;
using System.Windows.Forms;

namespace OnTopReplica.StartupOptions {

    /// <summary>
    /// Represents startup options that can be set via CLI scripting (or other stuff).
    /// </summary>
    class Options {

        public Options() {
            Status = CliStatus.Ok;
            Opacity = 255;
            DisableChrome = false;
            MustBeVisible = false;
            Fullscreen = false;
        }

        public int ScreenIndex = 0;

        #region Position and size

        public Point? StartLocation { get; set; }

        public ScreenPosition? StartPositionLock { get; set; }

        public Size? StartSize { get; set; }

        public int? StartWidth { get; set; }

        public int? StartHeight { get; set; }

        #endregion

        #region Window cloning

        public IntPtr? WindowId { get; set; }

        public string WindowTitle { get; set; }

        public string WindowClass { get; set; }

        public ThumbnailRegion Region { get; set; }

        public bool MustBeVisible { get; set; }

        #endregion

        #region Options

        public bool EnableClickForwarding { get; set; }

        public bool EnableClickThrough { get; set; }

        public byte Opacity { get; set; }

        public bool DisableChrome { get; set; }

        public bool Fullscreen { get; set; }

        #endregion

        #region Debug info

        StringBuilder _sb = new StringBuilder();
        TextWriter _sbWriter;

        public CliStatus Status { get; set; }

        /// <summary>
        /// Gets a debug message writer.
        /// </summary>
        public TextWriter DebugMessageWriter {
            get {
                if (_sbWriter == null) {
                    _sbWriter = new StringWriter(_sb);
                }
                return _sbWriter;
            }
        }

        /// <summary>
        /// Gets the debug message.
        /// </summary>
        public string DebugMessage {
            get {
                if(_sbWriter != null)
                    _sbWriter.Flush();
                return _sb.ToString();
            }
        }

        #endregion

        #region Application

        private void setFormLocation(Form form, Screen screen) {
            Point start = StartLocation ?? default(Point);
            Point loc = new Point(start.X + screen.WorkingArea.Location.X, start.Y + screen.WorkingArea.Location.Y);
            form.Location = loc;
        }

        public void Apply(MainForm form) {
            Log.Write("Applying command line launch parameters");

            form.Opacity = (double)Opacity / 255.0;

            //Seek handle for thumbnail cloning
            WindowHandle handle = null;
            if (WindowId.HasValue) {
                handle = WindowHandle.FromHandle(WindowId.Value);
            }
            else if (WindowTitle != null) {
                var seeker = new ByTitleWindowSeeker(WindowTitle) {
                    OwnerHandle = form.Handle,
                    SkipNotVisibleWindows = MustBeVisible
                };
                seeker.Refresh();

                handle = seeker.Windows.FirstOrDefault();
            }
            else if (WindowClass != null) {
                var seeker = new ByClassWindowSeeker(WindowClass) {
                    OwnerHandle = form.Handle,
                    SkipNotVisibleWindows = MustBeVisible
                };
                seeker.Refresh();

                handle = seeker.Windows.FirstOrDefault();
            }

            if (StartPositionLock.HasValue) {
                form.PositionLock = StartPositionLock.Value;
            }

            //Clone any found handle (this applies thumbnail and aspect ratio)
            if (handle != null) {
                form.SetThumbnail(handle, Region);
            }

            //Adaptive size handling
            if (!StartSize.HasValue && (StartWidth.HasValue || StartHeight.HasValue)) {
                if (StartWidth.HasValue) {
                    StartSize = new Size(StartWidth.Value, form.ComputeHeightFromWidth(StartWidth.Value));
                }
                else {
                    StartSize = new Size(form.ComputeWidthFromHeight(StartHeight.Value), StartHeight.Value);
                }
            }

            //Size and location start values
            if (StartLocation.HasValue && StartSize.HasValue) {
                form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                form.Location = StartLocation.Value;
                form.ClientSize = StartSize.Value;
            }
            else if (StartLocation.HasValue) {
                form.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
                form.Location = StartLocation.Value;
            }
            else if (StartSize.HasValue) {
                form.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
                form.ClientSize = StartSize.Value;
            }

            if (ScreenIndex != 0) {
                setFormLocation(form, Screen.AllScreens[ScreenIndex]);
            }
            //Other features
            if (EnableClickForwarding) {
                form.ClickForwardingEnabled = true;
            }
            if (EnableClickThrough) {
                form.ClickThroughEnabled = true;
            }

            form.IsChromeVisible = !DisableChrome;

            //Fullscreen
            if (Fullscreen) {
                form.FullscreenManager.SwitchFullscreen();
            }
        }

        #endregion

    }

}
