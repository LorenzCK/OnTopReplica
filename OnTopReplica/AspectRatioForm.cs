using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using VistaControls.Dwm.Helpers;
using System.Drawing;
using System.ComponentModel;

namespace OnTopReplica {

    /// <summary>
    /// Form that automatically keeps a certain aspect ratio and resizes without flickering.
    /// </summary>
    public class AspectRatioForm : GlassForm {

        bool _keepAspectRatio = true;

        /// <summary>
        /// Gets or sets whether the form should keep its aspect ratio.
        /// </summary>
        [Description("Enables fixed aspect ratio for this form."), Category("Appearance"), DefaultValue(true)]
        public bool KeepAspectRatio {
            get {
                return _keepAspectRatio;
            }
            set {
                _keepAspectRatio = value;
                
                if (value)
                    RefreshAspectRatio();
            }
        }

        double _aspectRatio = 1.0;

        /// <summary>
        /// Gets or sets the form's aspect ratio that will be kept automatically when resizing.
        /// </summary>
        [Description("Determins this form's fixed aspect ratio."), Category("Appearance"), DefaultValue(1.0)]
        public double AspectRatio {
            get {
                return _aspectRatio;
            }
            set {
                if (value <= 0.0 || Double.IsInfinity(value))
                    return;

                _aspectRatio = value;
            }
        }

        Padding _extraPadding;

        /// <summary>
        /// Gets or sets some additional internal padding of the form that is ignored when keeping the aspect ratio.
        /// </summary>
        [Description("Sets some padding inside the form's client area that is ignored when keeping the aspect ratio."),
            Category("Appearance")]
        public Padding ExtraPadding {
            get {
                return _extraPadding;
            }
            set {
                _extraPadding = value;
                
                if(KeepAspectRatio)
                    RefreshAspectRatio();
            }
        }

        /// <summary>
        /// Forces the form to update its height based on the current aspect ratio setting.
        /// </summary>
        public void RefreshAspectRatio() {
            int newWidth = ClientSize.Width;
            int newHeight = (int)((ClientSize.Width - ExtraPadding.Horizontal) / AspectRatio) + ExtraPadding.Vertical;
            if (newHeight < FromSizeToClientSize(MinimumSize).Height) {
                newHeight = FromSizeToClientSize(MinimumSize).Height;
                newWidth = (int)((newHeight - ExtraPadding.Vertical) * AspectRatio) + ExtraPadding.Horizontal;
            }

            ClientSize = new Size(newWidth, newHeight);
        }

        public void AdjustSize(double delta) {
            int newWidth = Math.Max((int)(ClientSize.Width + delta), MinimumSize.Width);
            int newHeight = (int)((newWidth - ExtraPadding.Horizontal) / AspectRatio + ExtraPadding.Vertical);

            //Readjust if we go lower than minimal height
            if (newHeight < MinimumSize.Height) {
                newHeight = MinimumSize.Height;
                newWidth = (int)((newHeight - ExtraPadding.Vertical) * AspectRatio + ExtraPadding.Horizontal);
            }

            //Compute movement to re-center
            int deltaX = newWidth - ClientSize.Width;
            int deltaY = newHeight - ClientSize.Height;

            ClientSize = new Size(newWidth, newHeight);
            Location = new Point(Location.X - (deltaX / 2), Location.Y - (deltaY / 2));
        }

        /// <summary>
        /// Updates the aspect ratio of the form and forces a refresh.
        /// </summary>
        public void SetAspectRatio(Size aspectRatioSource) {
            _keepAspectRatio = true; //set without updating
            AspectRatio = ((double)aspectRatioSource.Width / (double)aspectRatioSource.Height);
            RefreshAspectRatio();
        }

        #region Event overriding

        protected override void OnResizeEnd(EventArgs e) {
            base.OnResizeEnd(e);

            //Ensure that the ClientSize of the form is always respected
            //(not ensured by the WM_SIZING message alone because of rounding errors and the chrome space)
            if (KeepAspectRatio) {
                var newHeight = (int)((ClientSize.Width - ExtraPadding.Horizontal) / AspectRatio + ExtraPadding.Vertical);
                ClientSize = new Size(ClientSize.Width, newHeight);
            }
        }

        /// <summary>
        /// Override WM_SIZING message to restrict resizing.
        /// Taken from: http://www.vcskicks.com/maintain-aspect-ratio.php
        /// Improved with code from: http://stoyanoff.info/blog/2010/06/27/resizing-forms-while-keeping-aspect-ratio/
        /// </summary>
        protected override void WndProc(ref Message m) {
            if (KeepAspectRatio && m.Msg == NativeMethods.WM_SIZING) {
                var rc = (NativeMethods.Rectangle)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.Rectangle));
                int res = m.WParam.ToInt32();

                int width = (rc.Right - rc.Left) - clientSizeConversionWidth - ExtraPadding.Horizontal;
                int height = (rc.Bottom - rc.Top) - clientSizeConversionHeight - ExtraPadding.Vertical;

                if (res == NativeMethods.WMSZ_LEFT || res == NativeMethods.WMSZ_RIGHT) {
                    //Left or right resize, adjust top and bottom
                    int targetHeight = (int)(width / AspectRatio);
                    int diffHeight = height - targetHeight;

                    rc.Top += (int)(diffHeight / 2.0);
                    rc.Bottom = rc.Top + targetHeight + ExtraPadding.Vertical + clientSizeConversionHeight;
                }
                else if (res == NativeMethods.WMSZ_TOP || res == NativeMethods.WMSZ_BOTTOM) {
                    //Up or down resize, adjust left and right
                    int targetWidth = (int)(height * AspectRatio);
                    int diffWidth = width - targetWidth;

                    rc.Left += (int)(diffWidth / 2.0);
                    rc.Right = rc.Left + targetWidth + ExtraPadding.Horizontal + clientSizeConversionWidth;
                }
                else if (res == NativeMethods.WMSZ_RIGHT + NativeMethods.WMSZ_BOTTOM || res == NativeMethods.WMSZ_LEFT + NativeMethods.WMSZ_BOTTOM) {
                    //Lower corner resize, adjust bottom
                    rc.Bottom = rc.Top + (int)(width / AspectRatio) + ExtraPadding.Vertical + clientSizeConversionHeight;
                }
                else if (res == NativeMethods.WMSZ_LEFT + NativeMethods.WMSZ_TOP || res == NativeMethods.WMSZ_RIGHT + NativeMethods.WMSZ_TOP) {
                    //Upper corner resize, adjust top
                    rc.Top = rc.Bottom - (int)(width / AspectRatio) - ExtraPadding.Vertical - clientSizeConversionHeight;
                }

                Marshal.StructureToPtr(rc, m.LParam, true);
            }

            base.WndProc(ref m);
        }

        #endregion

        #region ClientSize/Size conversion helpers

        int clientSizeConversionWidth, clientSizeConversionHeight;

        /// <summary>
        /// Converts a client size measurement to a window size measurement.
        /// </summary>
        /// <param name="clientSize">Size of the window's client area.</param>
        /// <returns>Size of the whole window.</returns>
        public Size FromClientSizeToSize(Size clientSize) {
            return new Size(clientSize.Width + clientSizeConversionWidth, clientSize.Height + clientSizeConversionHeight);
        }

        /// <summary>
        /// Converts a window size measurement to a client size measurement.
        /// </summary>
        /// <param name="size">Size of the whole window.</param>
        /// <returns>Size of the window's client area.</returns>
        public Size FromSizeToClientSize(Size size) {
            return new Size(size.Width - clientSizeConversionWidth, size.Height - clientSizeConversionHeight);
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);

            clientSizeConversionWidth = Size.Width - ClientSize.Width;
            clientSizeConversionHeight = Size.Height - ClientSize.Height;
        }

        #endregion

    }

}
