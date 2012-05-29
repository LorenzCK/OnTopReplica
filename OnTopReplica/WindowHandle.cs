using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OnTopReplica.Native;

namespace OnTopReplica {

	/// <summary>
    /// Helper class that keeps a window handle (HWND),
    /// the title of the window and can load its icon.
    /// </summary>
	public class WindowHandle : System.Windows.Forms.IWin32Window {
		
        IntPtr _handle;
		string _title;

        /// <summary>
        /// Creates a new WindowHandle instance. The handle pointer must be valid, the title
        /// may be null or empty and will be updated as requested.
        /// </summary>
		public WindowHandle(IntPtr p, string title) {
			_handle = p;
			_title = title;
		}

        /// <summary>
        /// Creates a new WindowHandle instance. Additional features of the handle will be queried as needed.
        /// </summary>
        /// <param name="p"></param>
        public WindowHandle(IntPtr p) {
            _handle = p;
            _title = null;
        }

		public string Title {
			get {
                if (_title == null) {
                    _title = WindowMethods.GetWindowText(_handle) ?? string.Empty;
                }

				return _title;
			}
		}

		Icon _icon = null;
		bool _iconFetched = false;
		public Icon Icon {
			get {
				if (!_iconFetched) {
					//Fetch icon from window
					IntPtr hIcon;

                    if (MessagingMethods.SendMessageTimeout(_handle, WM.GETICON, new IntPtr(2), new IntPtr(0),
                        MessagingMethods.SendMessageTimeoutFlags.AbortIfHung | MessagingMethods.SendMessageTimeoutFlags.Block, 500, out hIcon) == IntPtr.Zero) {
                        hIcon = IntPtr.Zero;
                    }

					if (hIcon != IntPtr.Zero) {
						_icon = Icon.FromHandle(hIcon);
					}
					else {
						//Fetch icon from window class
                        hIcon = (IntPtr)WindowMethods.GetClassLong(_handle, WindowMethods.ClassLong.IconSmall);

						if (hIcon.ToInt64() != 0) {
							_icon = Icon.FromHandle(hIcon);
						}
					}
				}

				_iconFetched = true;

				return _icon;
			}
		}

        string _class = null;

        /// <summary>
        /// Gets the window's class name.
        /// </summary>
        /// <remarks>
        /// This value is cached and is never null.
        /// </remarks>
        public string Class {
            get {
                if (_class == null) {
                    _class = Native.WindowMethods.GetWindowClass(Handle) ?? string.Empty;
                }
                return _class;
            }
        }

        #region Object override

        public override string ToString() {
			return _title;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(obj, this))
				return true;

			System.Windows.Forms.IWin32Window win = obj as System.Windows.Forms.IWin32Window;

			if (win == null)
				return false;

			return (win.Handle == _handle);
		}

		public override int GetHashCode() {
			return _handle.GetHashCode();
		}

        #endregion

        #region IWin32Window Members

        public IntPtr Handle {
			get { return _handle; }
		}

		#endregion

        /// <summary>
        /// Creates a new windowHandle instance from a given IntPtr handle.
        /// </summary>
        /// <param name="handle">Handle value.</param>
        public static WindowHandle FromHandle(IntPtr handle) {
            return new WindowHandle(handle, null);
        }

	}
}
