using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>A native Rectangle Structure.</summary>
    [StructLayout(LayoutKind.Sequential)]
    struct NRectangle {
        public NRectangle(int left, int top, int right, int bottom) {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public NRectangle(System.Drawing.Rectangle rect) {
            Left = rect.X;
            Top = rect.Y;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width {
            get {
                return Right - Left;
            }
        }
        public int Height {
            get {
                return Bottom - Top;
            }
        }

        public System.Drawing.Rectangle ToRectangle() {
            return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
        }

        public System.Drawing.Size Size {
            get {
                return new System.Drawing.Size(Width, Height);
            }
        }

        public override string ToString() {
            return string.Format("{{{0},{1},{2},{3}}}", Left, Top, Right, Bottom);
        }

    }
}
