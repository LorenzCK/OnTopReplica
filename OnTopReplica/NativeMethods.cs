using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica
{
	static class NativeMethods
	{
		/// <summary>A native Rectangle Structure.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Rectangle
		{
			public Rectangle(int left, int top, int right, int bottom) {
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public Rectangle(System.Drawing.Rectangle rect) {
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
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Point {
			public int X, Y;

			public Point(int x, int y) {
				X = x;
				Y = y;
			}

			public Point(Point copy) {
				X = copy.X;
				Y = copy.Y;
			}

			public static Point FromPoint(System.Drawing.Point point) {
				return new Point(point.X, point.Y);
			}

			public System.Drawing.Point ToPoint() {
				return new System.Drawing.Point(X, Y);
			}

			public override string ToString() {
				return "{" + X + "," + Y + "}";
			}
		}


		[DllImport("user32.dll")]
		public static extern IntPtr RealChildWindowFromPoint(IntPtr parent, Point point);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		public static System.Drawing.Rectangle GetClient(IntPtr hWnd) {
			Rectangle ret;
			if (GetClientRect(hWnd, out ret))
				return ret.ToRectangle();
			else
				throw new Exception("Failed to get Client Rectangle.");
		}

		[DllImport("user32.dll")]
		static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

		public static System.Drawing.Rectangle GetWindowBounds(IntPtr hWnd) {
			Rectangle ret = new Rectangle();
			if (GetWindowRect(hWnd, out ret))
				return ret.ToRectangle();
			else
				throw new Exception("Failed to get Window Bounds.");
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		static extern bool ClientToScreen(IntPtr hwnd, ref Point point);

		public static Point ClientToScreen(IntPtr hwnd, Point clientPoint) {
			Point localCopy = new Point(clientPoint);

			if (ClientToScreen(hwnd, ref localCopy))
				return localCopy;
			else
				return new Point();
		}

		[DllImport("user32.dll")]
		static extern bool ScreenToClient(IntPtr hwnd, ref Point point);

		public static Point ScreenToClient(IntPtr hwnd, Point screenPoint) {
			Point localCopy = new Point(screenPoint);

			if (ScreenToClient(hwnd, ref localCopy))
				return localCopy;
			else
				return new Point();
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		public enum GetAncestorMode : uint {
			Parent = 1,
			Root = 2,
			RootOwner = 3
		}

		public enum GetWindowMode : uint {
			GW_HWNDFIRST = 0,
			GW_HWNDLAST = 1,
			GW_HWNDNEXT = 2,
			GW_HWNDPREV = 3,
			GW_OWNER = 4,
			GW_CHILD = 5,
			GW_ENABLEDPOPUP = 6
		}

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hwnd, GetWindowMode mode);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(IntPtr hWnd);





		public enum WindowLong {
			WndProc = (-4),
			HInstance = (-6),
			HwndParent = (-8),
			Style = (-16),
			ExStyle = (-20),
			UserData = (-21),
			Id = (-12)
		}

		public enum ClassLong {
			Icon = -14,
			IconSmall = -34
		}

		[Flags]
		public enum WindowStyles : long {
			None = 0,
			Disabled = 0x8000000L,
			Minimize = 0x20000000L,
			MinimizeBox = 0x20000L,
			Visible = 0x10000000L
		}

		[Flags]
		public enum WindowExStyles : long {
			AppWindow = 0x40000,
			Layered = 0x80000,
			NoActivate = 0x8000000L,
			ToolWindow = 0x80,
			TopMost = 8,
			Transparent = 0x20
		}

		public static IntPtr GetWindowLong(IntPtr hWnd, WindowLong i) {
			if (IntPtr.Size == 8) {
				return GetWindowLongPtr64(hWnd, (int)i);
			}
			else {
				return new IntPtr(GetWindowLong32(hWnd, (int)i));
			}
		}

		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);


		public static IntPtr GetClassLongPtr(IntPtr hWnd, ClassLong i) {
			if (IntPtr.Size == 8) {
				return GetClassLong64(hWnd, (int)i);
			}
			return new IntPtr(GetClassLong32(hWnd, (int)i));
		}

		[DllImport("user32.dll", EntryPoint = "GetClassLongPtrW")]
		private static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetClassLongW")]
		private static extern int GetClassLong32(IntPtr hWnd, int nIndex);





		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint message, IntPtr wparam, IntPtr lparam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

		public const uint WM_GETICON = 0x7f;
        public const int WM_SIZING = 0x214;
        
        public const int WMSZ_LEFT = 1;
        public const int WMSZ_RIGHT = 2;
        public const int WMSZ_TOP = 3;
        public const int WMSZ_BOTTOM = 6;

		[Flags]
		public enum SendMessageTimeoutFlags : uint {
			AbortIfHung = 2,
			Block = 1,
			Normal = 0
		}


		public const int HTTRANSPARENT = -1;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;

		public const int WM_NCHITTEST = 0x84;
		public const int WM_NCPAINT = 0x0085;
		public const int WM_LBUTTONDOWN = 0x0201;
		public const int WM_LBUTTONUP = 0x0202;
		public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONUP = 0x00A5;

		public const int MK_LBUTTON = 0x0001;

		public static IntPtr MakeLParam(int LoWord, int HiWord) {
			return new IntPtr((HiWord << 16) | (LoWord & 0xffff));
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = false)]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


		[DllImport("user32.dll", SetLastError = false)]
		public static extern bool SetForegroundWindow(IntPtr hwnd);


		[DllImport("user32.dll")]
		public static extern IntPtr GetMenu(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(VirtualKeyState nVirtKey);

	}
}
