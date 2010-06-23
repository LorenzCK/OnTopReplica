using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using VistaControls.Dwm.Helpers;
using System.Drawing;

namespace OnTopReplica {

    /// <summary>
    /// Form that automatically keeps a certain aspect ratio and resizes without flickering.
    /// </summary>
    public class AspectRatioForm : GlassForm {

        public AspectRatioForm() {
            AspectRatio = 1.0;
        }

        bool _keepAspectRatio = true;
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

        protected override void OnResizeEnd(EventArgs e) {
            base.OnResizeEnd(e);

            //Ensure that the ClientSize of the form is always respected (not ensured by the WM_SIZING message alone)
            if (KeepAspectRatio) {
                //Since WM_SIZING already fixes up the size almost correctly,
                //simply set the height to the correct value
                var newHeight = (int)((ClientSize.Width - ExtraPadding.Horizontal) / AspectRatio + ExtraPadding.Vertical);
                ClientSize = new Size(ClientSize.Width, newHeight);
            }
        }

        /// <summary>
        /// Override WM_SIZING message to restrict resizing.
        /// Taken from: http://www.vcskicks.com/maintain-aspect-ratio.php
        /// </summary>
        protected override void WndProc(ref Message m) {
            if (KeepAspectRatio && m.Msg == NativeMethods.WM_SIZING) {
                var rc = (NativeMethods.Rectangle)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.Rectangle));
                int res = m.WParam.ToInt32();

                if (res == NativeMethods.WMSZ_LEFT || res == NativeMethods.WMSZ_RIGHT) {
                    //Left or right resize -> adjust height (bottom)
                    int targetHeight = (int)Math.Ceiling((Width - ExtraPadding.Horizontal) / AspectRatio) + ExtraPadding.Vertical;
                    int originalHeight = rc.Bottom - rc.Top;
                    int diffHeight = originalHeight - targetHeight;

                    rc.Top += (diffHeight / 2);
                    rc.Bottom = rc.Top + targetHeight;
                }
                else if (res == NativeMethods.WMSZ_TOP || res == NativeMethods.WMSZ_BOTTOM) {
                    //Up or down resize -> adjust width (right)
                    int targetWidth = (int)Math.Ceiling((Height - ExtraPadding.Vertical) * AspectRatio) + ExtraPadding.Horizontal;
                    int originalWidth = rc.Right - rc.Left;
                    int diffWidth = originalWidth - targetWidth;

                    rc.Left += (diffWidth / 2);
                    rc.Right = rc.Left + targetWidth;
                }
                else if (res == NativeMethods.WMSZ_RIGHT + NativeMethods.WMSZ_BOTTOM || res == NativeMethods.WMSZ_LEFT + NativeMethods.WMSZ_BOTTOM) {
                    //Lower-right/left corner resize -> adjust height (could have been width)
                    rc.Bottom = rc.Top + (int)Math.Ceiling((Width - ExtraPadding.Horizontal) / AspectRatio) + ExtraPadding.Vertical;
                }
                else if (res == NativeMethods.WMSZ_LEFT + NativeMethods.WMSZ_TOP || res == NativeMethods.WMSZ_RIGHT + NativeMethods.WMSZ_TOP) {
                    //Upper-left/right corner -> adjust width (could have been height)
                    rc.Top = rc.Bottom - (int)Math.Ceiling((Width - ExtraPadding.Horizontal) / AspectRatio) + ExtraPadding.Vertical;
                }

                Marshal.StructureToPtr(rc, m.LParam, true);
            }

            base.WndProc(ref m);
        }

        #region ClientSize/Size conversion

        //bool clientSizeConversionSet = false;
        int clientSizeConversionWidth, clientSizeConversionHeight;

        public Size FromClientSizeToSize(Size clientSize) {
            return new Size(clientSize.Width + clientSizeConversionWidth, clientSize.Height + clientSizeConversionHeight);
        }

        public Size FromSizeToClientSize(Size size) {
            return new Size(size.Width - clientSizeConversionWidth, size.Height - clientSizeConversionHeight);
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);

            clientSizeConversionWidth = this.Size.Width - this.ClientSize.Width;
            clientSizeConversionHeight = this.Size.Height - this.ClientSize.Height;
        }

        #endregion

    }

}
