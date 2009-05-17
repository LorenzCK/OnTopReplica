using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OnTopReplica {
	public static class Win32Helper {

		/// <summary>Inject a fake left mouse click on a target window, on a location expressed in client coordinates.</summary>
		/// <param name="window">Target window to click on.</param>
		/// <param name="clickLocation">Location of the mouse click expressed in client coordiantes of the target window.</param>
		/// <param name="doubleClick">True if a double click should be injected.</param>
		public static void InjectFakeMouseClick(IntPtr window, Point clickLocation, bool doubleClick) {
			NativeMethods.Point scrClickLocation = NativeMethods.ClientToScreen(window,
				NativeMethods.Point.FromPoint(clickLocation));

			//HACK (?)
			//If target window has a Menu (which appears on the thumbnail) move the clicked location down
			//in order to adjust (the menu isn't part of the window's client rect).
			IntPtr hMenu = NativeMethods.GetMenu(window);
			if (hMenu != IntPtr.Zero)
				scrClickLocation.Y -= SystemInformation.MenuHeight;

			IntPtr hChild = GetRealChildControlFromPoint(window, scrClickLocation);
			NativeMethods.Point clntClickLocation = NativeMethods.ScreenToClient(hChild, scrClickLocation);

			if (doubleClick)
				InjectDoubleLeftMouseClick(hChild, clntClickLocation);
			else
				InjectLeftMouseClick(hChild, clntClickLocation);
		}

		private static void InjectLeftMouseClick(IntPtr child, NativeMethods.Point clientLocation) {
			IntPtr lParamClickLocation = NativeMethods.MakeLParam(clientLocation.X, clientLocation.Y);

			NativeMethods.PostMessage(child, NativeMethods.WM_LBUTTONDOWN,
					new IntPtr(NativeMethods.MK_LBUTTON), lParamClickLocation);

			NativeMethods.PostMessage(child, NativeMethods.WM_LBUTTONUP,
				new IntPtr(NativeMethods.MK_LBUTTON), lParamClickLocation);

#if DEBUG
			Console.WriteLine("Left click on window #" + child.ToString() + " at " + clientLocation.ToString());
#endif
		}

		private static void InjectDoubleLeftMouseClick(IntPtr child, NativeMethods.Point clientLocation) {
			IntPtr lParamClickLocation = NativeMethods.MakeLParam(clientLocation.X, clientLocation.Y);

			NativeMethods.PostMessage(child, NativeMethods.WM_LBUTTONDBLCLK,
					new IntPtr(NativeMethods.MK_LBUTTON), lParamClickLocation);

#if DEBUG
			Console.WriteLine("Double left click on window #" + child.ToString() + " at " + clientLocation.ToString());
#endif
		}

		/// <summary>Returns the child control of a window corresponding to a screen location.</summary>
		/// <param name="parent">Parent window to explore.</param>
		/// <param name="scrClickLocation">Child control location in screen coordinates.</param>
		private static IntPtr GetRealChildControlFromPoint(IntPtr parent, NativeMethods.Point scrClickLocation) {
			IntPtr curr = parent, child = IntPtr.Zero;
			do {
				child = NativeMethods.RealChildWindowFromPoint(curr, NativeMethods.ScreenToClient(curr, scrClickLocation));

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

	}
}
