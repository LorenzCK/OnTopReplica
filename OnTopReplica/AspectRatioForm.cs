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

        /// <summary>
        /// Forces the form to update its height based on the current aspect ratio setting.
        /// </summary>
        public void RefreshAspectRatio() {
            ClientSize = new Size(ClientSize.Width, (int)(ClientSize.Width / AspectRatio));
        }

        /// <summary>
        /// Updates the aspect ratio of the form and refreshes it.
        /// </summary>
        public void SetAspectRatio(Size aspectRatioSource) {
            _keepAspectRatio = true; //set without updating
            AspectRatio = ((double)aspectRatioSource.Width / (double)aspectRatioSource.Height);
            RefreshAspectRatio();
        }

        /// <summary>
        /// Override WM_SIZING message.
        /// Taken from: http://www.vcskicks.com/maintain-aspect-ratio.php
        /// </summary>
        protected override void WndProc(ref Message m) {
            if (KeepAspectRatio && m.Msg == NativeMethods.WM_SIZING) {
                var rc = (NativeMethods.Rectangle)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.Rectangle));
                int res = m.WParam.ToInt32();

                if (res == NativeMethods.WMSZ_LEFT || res == NativeMethods.WMSZ_RIGHT) {
                    //Left or right resize -> adjust height (bottom)
                    int targetHeight = (int)Math.Ceiling(this.Width / AspectRatio);
                    int originalHeight = rc.Bottom - rc.Top;
                    int diffHeight = originalHeight - targetHeight;

                    rc.Top += (diffHeight / 2);
                    rc.Bottom = rc.Top + targetHeight;
                }
                else if (res == NativeMethods.WMSZ_TOP || res == NativeMethods.WMSZ_BOTTOM) {
                    //Up or down resize -> adjust width (right)
                    int targetWidth = (int)Math.Ceiling(this.Height * AspectRatio);
                    int originalWidth = rc.Right - rc.Left;
                    int diffWidth = originalWidth - targetWidth;

                    rc.Left += (diffWidth / 2);
                    rc.Right = rc.Left + targetWidth;
                }
                else if (res == NativeMethods.WMSZ_RIGHT + NativeMethods.WMSZ_BOTTOM) {
                    //Lower-right corner resize -> adjust height (could have been width)
                    rc.Bottom = rc.Top + (int)Math.Ceiling(this.Width / AspectRatio);
                }
                else if (res == NativeMethods.WMSZ_LEFT + NativeMethods.WMSZ_BOTTOM) {
                    //Lower-left corner resize -> adjust height (could have been width)
                    rc.Bottom = rc.Top + (int)Math.Ceiling(this.Width / AspectRatio);
                }
                else if (res == NativeMethods.WMSZ_LEFT + NativeMethods.WMSZ_TOP) {
                    //Upper-left corner -> adjust width (could have been height)
                    rc.Left = rc.Right - (int)Math.Ceiling(this.Height * AspectRatio);
                }
                else if (res == NativeMethods.WMSZ_RIGHT + NativeMethods.WMSZ_TOP) {
                    //Upper-right corner -> adjust width (could have been height)
                    rc.Right = rc.Left + (int)Math.Ceiling(this.Height * AspectRatio);
                }

                Marshal.StructureToPtr(rc, m.LParam, true);
            }

            base.WndProc(ref m);
        }


    }

}
