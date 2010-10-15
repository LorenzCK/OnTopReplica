using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica {
	/// <summary>A helper class that allows you to easily build and keep a list of Windows (in the form of WindowHandle objects).</summary>
	public class WindowManager {
        
        List<WindowHandle> _windows = new List<WindowHandle>();

		public enum EnumerationMode {
			/// <summary>All windows with 'Visible' flag.</summary>
			AllVisible,

			/// <summary>All top level windows.</summary>
			AllTopLevel,

			/// <summary>Windows of a task (like Alt+Tab).</summary>
			TaskWindows
		}

		/// <summary>Refreshes the window list.</summary>
		public void Refresh(EnumerationMode mode) {
            _windows = new List<WindowHandle>();

            WindowManagerMethods.EnumWindowsProc proc = null;
			switch (mode) {
				case EnumerationMode.AllVisible:
                    proc = new WindowManagerMethods.EnumWindowsProc(EnumWindowProcAll);
					break;

				case EnumerationMode.AllTopLevel:
                    proc = new WindowManagerMethods.EnumWindowsProc(EnumWindowProcTopLevel);
					break;

				case EnumerationMode.TaskWindows:
                    proc = new WindowManagerMethods.EnumWindowsProc(EnumWindowProcTask);
					break;
			}

			WindowManagerMethods.EnumWindows(proc, IntPtr.Zero);
		}


		public IEnumerable<WindowHandle> Windows {
			get {
                return _windows;
			}
		}



		private bool EnumWindowProcAll(IntPtr hwnd, IntPtr lParam) {
            if (WindowManagerMethods.IsWindowVisible(hwnd)) {
                string title = WindowMethods.GetWindowText(hwnd);
				_windows.Add( new WindowHandle(hwnd, title));
			}
			return true;
		}

		private bool EnumWindowProcTopLevel(IntPtr hwnd, IntPtr lParam) {
            if (WindowManagerMethods.IsWindowVisible(hwnd)) {
				//Check if window has no parent
                if ((long)WindowManagerMethods.GetParent(hwnd) == 0 && WindowManagerMethods.GetDesktopWindow() != hwnd) {
                    string title = WindowMethods.GetWindowText(hwnd);
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

			string title = WindowMethods.GetWindowText(hwnd);
			if (string.IsNullOrEmpty(title))
				return true;

            if (WindowManagerMethods.IsWindowVisible(hwnd)) {
                if ((long)WindowManagerMethods.GetParent(hwnd) == 0) {
                    bool hasOwner = (long)WindowManagerMethods.GetWindow(hwnd, WindowManagerMethods.GetWindowMode.GW_OWNER) != 0;
                    WindowMethods.WindowExStyles exStyle = (WindowMethods.WindowExStyles)WindowMethods.GetWindowLong(hwnd, WindowMethods.WindowLong.ExStyle);

                    if (((exStyle & WindowMethods.WindowExStyles.ToolWindow) == 0 && !hasOwner) || //unowned non-tool window
                        ((exStyle & WindowMethods.WindowExStyles.AppWindow) == WindowMethods.WindowExStyles.AppWindow && hasOwner)) { //owned application window
						_windows.Add(new WindowHandle(hwnd, title));
					}
				}
			}

			return true;
		}

	}
}
