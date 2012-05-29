using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;
using System.Drawing;
using System.Windows.Forms;

namespace OnTopReplica {
	public static class Win32Helper {

        #region Injection

        /// <summary>Inject a fake left mouse click on a target window, on a location expressed in client coordinates.</summary>
		/// <param name="window">Target window to click on.</param>
		/// <param name="clickLocation">Location of the mouse click expressed in client coordiantes of the target window.</param>
		/// <param name="doubleClick">True if a double click should be injected.</param>
		public static void InjectFakeMouseClick(IntPtr window, CloneClickEventArgs clickArgs) {
            NPoint clientClickLocation = NPoint.FromPoint(clickArgs.ClientClickLocation);
			NPoint scrClickLocation = WindowManagerMethods.ClientToScreen(window, clientClickLocation);

			//HACK (?)
			//If target window has a Menu (which appears on the thumbnail) move the clicked location down
			//in order to adjust (the menu isn't part of the window's client rect).
			IntPtr hMenu = WindowMethods.GetMenu(window);
			if (hMenu != IntPtr.Zero)
				scrClickLocation.Y -= SystemInformation.MenuHeight;

			IntPtr hChild = GetRealChildControlFromPoint(window, scrClickLocation);
            NPoint clntClickLocation = WindowManagerMethods.ScreenToClient(hChild, scrClickLocation);

            if (clickArgs.Buttons == MouseButtons.Left) {
                if(clickArgs.IsDoubleClick)
                    InjectDoubleLeftMouseClick(hChild, clntClickLocation);
                else
                    InjectLeftMouseClick(hChild, clntClickLocation);
            }
            else if (clickArgs.Buttons == MouseButtons.Right) {
                if(clickArgs.IsDoubleClick)
                    InjectDoubleRightMouseClick(hChild, clntClickLocation);
                else
                    InjectRightMouseClick(hChild, clntClickLocation);
            }
		}

		private static void InjectLeftMouseClick(IntPtr child, NPoint clientLocation) {
			IntPtr lParamClickLocation = MessagingMethods.MakeLParam(clientLocation.X, clientLocation.Y);

            MessagingMethods.PostMessage(child, WM.LBUTTONDOWN, new IntPtr(MK.LBUTTON), lParamClickLocation);
            MessagingMethods.PostMessage(child, WM.LBUTTONUP, new IntPtr(MK.LBUTTON), lParamClickLocation);

#if DEBUG
			Console.WriteLine("Left click on window #" + child.ToString() + " at " + clientLocation.ToString());
#endif
		}

        private static void InjectRightMouseClick(IntPtr child, NPoint clientLocation) {
            IntPtr lParamClickLocation = MessagingMethods.MakeLParam(clientLocation.X, clientLocation.Y);

            MessagingMethods.PostMessage(child, WM.RBUTTONDOWN, new IntPtr(MK.RBUTTON), lParamClickLocation);
            MessagingMethods.PostMessage(child, WM.RBUTTONUP, new IntPtr(MK.RBUTTON), lParamClickLocation);

#if DEBUG
            Console.WriteLine("Right click on window #" + child.ToString() + " at " + clientLocation.ToString());
#endif
        }

		private static void InjectDoubleLeftMouseClick(IntPtr child, NPoint clientLocation) {
            IntPtr lParamClickLocation = MessagingMethods.MakeLParam(clientLocation.X, clientLocation.Y);

            MessagingMethods.PostMessage(child, WM.LBUTTONDBLCLK, new IntPtr(MK.LBUTTON), lParamClickLocation);

#if DEBUG
			Console.WriteLine("Double left click on window #" + child.ToString() + " at " + clientLocation.ToString());
#endif
		}

        private static void InjectDoubleRightMouseClick(IntPtr child, NPoint clientLocation) {
            IntPtr lParamClickLocation = MessagingMethods.MakeLParam(clientLocation.X, clientLocation.Y);

            MessagingMethods.PostMessage(child, WM.RBUTTONDBLCLK, new IntPtr(MK.RBUTTON), lParamClickLocation);

#if DEBUG
            Console.WriteLine("Double right click on window #" + child.ToString() + " at " + clientLocation.ToString());
#endif
        }

        #endregion

        /// <summary>Returns the child control of a window corresponding to a screen location.</summary>
		/// <param name="parent">Parent window to explore.</param>
		/// <param name="scrClickLocation">Child control location in screen coordinates.</param>
		private static IntPtr GetRealChildControlFromPoint(IntPtr parent, NPoint scrClickLocation) {
			IntPtr curr = parent, child = IntPtr.Zero;
			do {
                child = WindowManagerMethods.RealChildWindowFromPoint(curr,
                    WindowManagerMethods.ScreenToClient(curr, scrClickLocation));

				if (child == IntPtr.Zero || child == curr)
					break;

				//Update for next loop
				curr = child;
			}
			while (true);

			//Safety check, shouldn't happen
			if (curr == IntPtr.Zero)
				curr = parent;

			return curr;
		}

        /// <summary>
        /// Gets a handle to the window that currently is in the foreground.
        /// </summary>
        /// <returns>May return null if call fails or no valid window selected.</returns>
        public static WindowHandle GetCurrentForegroundWindow() {
            IntPtr handle = WindowManagerMethods.GetForegroundWindow();
            if (handle == IntPtr.Zero)
                return null;

            return new WindowHandle(handle);
        }

	}
}
