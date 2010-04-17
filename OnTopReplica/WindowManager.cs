using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {
	/// <summary>A helper class that allows you to easily build and keep a list of Windows (in the form of WindowHandle objects).</summary>
	public class WindowManager {
        List<WindowHandle> _windows = null;

		public enum EnumerationMode {
			/// <summary>All windows with 'Visible' flag.</summary>
			AllVisible,

			/// <summary>All top level windows.</summary>
			AllTopLevel,

			/// <summary>Windows of a task (like Alt+Tab).</summary>
			TaskWindows
		}

		public WindowManager() {
			Refresh(EnumerationMode.AllTopLevel);
		}

		public WindowManager(EnumerationMode mode) {
			Refresh(mode);
		}

		/// <summary>Refreshes the window list.</summary>
		public void Refresh(EnumerationMode mode) {
            _windows = new List<WindowHandle>();

			NativeMethods.EnumWindowsProc proc = null;
			switch (mode) {
				case EnumerationMode.AllVisible:
					proc = new NativeMethods.EnumWindowsProc(EnumWindowProcAll);
					break;

				case EnumerationMode.AllTopLevel:
					proc = new NativeMethods.EnumWindowsProc(EnumWindowProcTopLevel);
					break;

				case EnumerationMode.TaskWindows:
					proc = new NativeMethods.EnumWindowsProc(EnumWindowProcTask);
					break;
			}

			NativeMethods.EnumWindows(proc, IntPtr.Zero);
		}


		public IEnumerable<WindowHandle> Windows {
			get {
                return _windows;
			}
		}



		private bool EnumWindowProcAll(IntPtr hwnd, IntPtr lParam) {
			if (NativeMethods.IsWindowVisible(hwnd)) {
                string title = GetWindowTitle(hwnd);
				_windows.Add( new WindowHandle(hwnd, title));
			}
			return true;
		}

		private bool EnumWindowProcTopLevel(IntPtr hwnd, IntPtr lParam) {
			if (NativeMethods.IsWindowVisible(hwnd)) {
				//Check if window has no parent
				if ((long)NativeMethods.GetParent(hwnd) == 0 && NativeMethods.GetDesktopWindow() != hwnd) {
                    string title = GetWindowTitle(hwnd);
					_windows.Add( new WindowHandle(hwnd, title));
				}
			}
			return true;
		}

		private bool EnumWindowProcTask(IntPtr hwnd, IntPtr lParam) {
			//Code taken from: http://www.thescarms.com/VBasic/alttab.aspx

			//Accept windows that
			// - are visible
			// - do not have a parent
			// - have no owner and are not Tool windows OR
			// - have an owner and are App windows

			//Reject empty titles

			string title = GetWindowTitle(hwnd);
			if (string.IsNullOrEmpty(title))
				return true;

			if (NativeMethods.IsWindowVisible(hwnd)) {
				if ((long)NativeMethods.GetParent(hwnd) == 0) {
					bool hasOwner = (long)NativeMethods.GetWindow(hwnd, NativeMethods.GetWindowMode.GW_OWNER) != 0;
					NativeMethods.WindowExStyles exStyle = (NativeMethods.WindowExStyles)NativeMethods.GetWindowLong(hwnd, NativeMethods.WindowLong.ExStyle);

					if (((exStyle & NativeMethods.WindowExStyles.ToolWindow) == 0 && !hasOwner) || //unowned non-tool window
						((exStyle & NativeMethods.WindowExStyles.AppWindow) == NativeMethods.WindowExStyles.AppWindow && hasOwner)) { //owned application window
						_windows.Add(new WindowHandle(hwnd, title));
					}
				}
			}

			return true;
		}

		#region Auxiliary methods

		private string GetWindowTitle(IntPtr hwnd) {
			int length = NativeMethods.GetWindowTextLength(hwnd);

			if (length > 0) {
				StringBuilder sb = new StringBuilder(length + 1);
				if (NativeMethods.GetWindowText(hwnd, sb, sb.Capacity) > 0)
					return sb.ToString();
				else
					return String.Empty;
			}
			else
				return String.Empty;
		}

		#endregion
	}
}
