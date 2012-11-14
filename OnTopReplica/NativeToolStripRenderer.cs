using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;

namespace Asztal.Szótár {
	public enum ToolbarTheme {
		Toolbar,
		MediaToolbar,
		CommunicationsToolbar,
		BrowserTabBar
	}
	
	/// <summary>
	/// Renders a toolstrip using the UxTheme API via VisualStyleRenderer. Visual styles must be supported for this to work; if you need to support other operating systems use
	/// </summary>
	class UXThemeToolStripRenderer : ToolStripSystemRenderer {
		/// <summary>
		/// It shouldn't be necessary to P/Invoke like this, however a bug in VisualStyleRenderer.GetMargins forces my hand.
		/// </summary>
		static internal class NativeMethods {
			[StructLayout(LayoutKind.Sequential, Pack = 1)]
			public struct MARGINS {
				public int cxLeftWidth;
				public int cxRightWidth;
				public int cyTopHeight;
				public int cyBottomHeight;
			}

			[DllImport("uxtheme", ExactSpelling = true)]
			public extern static Int32 GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr rect, out MARGINS pMargins);
		}

		//See http://msdn2.microsoft.com/en-us/library/bb773210.aspx - "Parts and States"
		#region Parts and States 
		
        enum MenuParts : int {
			MENU_MENUITEM_TMSCHEMA = 1,
			MENU_MENUDROPDOWN_TMSCHEMA = 2,
			MENU_MENUBARITEM_TMSCHEMA = 3,
			MENU_MENUBARDROPDOWN_TMSCHEMA = 4,
			MENU_CHEVRON_TMSCHEMA = 5,
			MENU_SEPARATOR_TMSCHEMA = 6,
			MENU_BARBACKGROUND = 7,
			MENU_BARITEM = 8,
			MENU_POPUPBACKGROUND = 9,
			MENU_POPUPBORDERS = 10,
			MENU_POPUPCHECK = 11,
			MENU_POPUPCHECKBACKGROUND = 12,
			MENU_POPUPGUTTER = 13,
			MENU_POPUPITEM = 14,
			MENU_POPUPSEPARATOR = 15,
			MENU_POPUPSUBMENU = 16,
			MENU_SYSTEMCLOSE = 17,
			MENU_SYSTEMMAXIMIZE = 18,
			MENU_SYSTEMMINIMIZE = 19,
			MENU_SYSTEMRESTORE = 20
		}

		enum MenuBarStates : int {
			MB_ACTIVE = 1,
			MB_INACTIVE = 2
		}

		enum MenuBarItemStates : int {
			MBI_NORMAL = 1,
			MBI_HOT = 2,
			MBI_PUSHED = 3,
			MBI_DISABLED = 4,
			MBI_DISABLEDHOT = 5,
			MBI_DISABLEDPUSHED = 6
		}

		enum MenuPopupItemStates : int {
			MPI_NORMAL = 1,
			MPI_HOT = 2,
			MPI_DISABLED = 3,
			MPI_DISABLEDHOT = 4
		}

		enum MenuPopupCheckStates : int {
			MC_CHECKMARKNORMAL = 1,
			MC_CHECKMARKDISABLED = 2,
			MC_BULLETNORMAL = 3,
			MC_BULLETDISABLED = 4
		}

		enum MenuPopupCheckBackgroundStates : int {
			MCB_DISABLED = 1,
			MCB_NORMAL = 2,
			MCB_BITMAP = 3
		}

		enum MenuPopupSubMenuStates : int {
			MSM_NORMAL = 1,
			MSM_DISABLED = 2
		}

		enum MarginTypes : int {
			TMT_SIZINGMARGINS = 3601,
			TMT_CONTENTMARGINS = 3602,
			TMT_CAPTIONMARGINS = 3603
		}

		const int RP_BACKGROUND = 6;

		#endregion

		#region Theme helpers

		Padding GetThemeMargins(IDeviceContext dc, MarginTypes marginType) {
			NativeMethods.MARGINS margins;
			try {
				IntPtr hDC = dc.GetHdc();
				if (0 == NativeMethods.GetThemeMargins(renderer.Handle, hDC, renderer.Part, renderer.State, (int)marginType, IntPtr.Zero, out margins))
					return new Padding(margins.cxLeftWidth, margins.cyTopHeight, margins.cxRightWidth, margins.cyBottomHeight);
				return new Padding(-1);
			} finally {
				dc.ReleaseHdc();
			}
		}

		private static int GetItemState(ToolStripItem item) {
			bool pressed = item.Pressed;
			bool hot = item.Selected;

			if (item.Owner.IsDropDown) {
				if (item.Enabled)
					return hot ? (int)MenuPopupItemStates.MPI_HOT : (int)MenuPopupItemStates.MPI_NORMAL;
				return hot ? (int)MenuPopupItemStates.MPI_DISABLEDHOT : (int)MenuPopupItemStates.MPI_DISABLED;
			} else {
				if (pressed)
					return item.Enabled ? (int)MenuBarItemStates.MBI_PUSHED : (int)MenuBarItemStates.MBI_DISABLEDPUSHED;
				if (item.Enabled)
					return hot ? (int)MenuBarItemStates.MBI_HOT : (int)MenuBarItemStates.MBI_NORMAL;
				return hot ? (int)MenuBarItemStates.MBI_DISABLEDHOT : (int)MenuBarItemStates.MBI_DISABLED;
			}
		}

		#endregion

		#region Theme subclasses

		public ToolbarTheme Theme {
			get; set;
		}

		private string RebarClass {
			get {
				return SubclassPrefix + "Rebar";
			}
		}

		private string ToolbarClass {
			get {
				return SubclassPrefix + "ToolBar";
			}
		}

		private string MenuClass {
			get {
				return SubclassPrefix + "Menu";
			}
		}

		private string SubclassPrefix {
			get {
				switch (Theme) {
					case ToolbarTheme.MediaToolbar: return "Media::";
					case ToolbarTheme.CommunicationsToolbar: return "Communications::";
					case ToolbarTheme.BrowserTabBar: return "BrowserTabBar::";
					default: return string.Empty;
				}
			}
		}

		private VisualStyleElement Subclass(VisualStyleElement element) {
			return VisualStyleElement.CreateElement(SubclassPrefix + element.ClassName,
				element.Part, element.State);
		}

		#endregion

		VisualStyleRenderer renderer;

		public UXThemeToolStripRenderer(ToolbarTheme theme) {
			Theme = theme;
			renderer = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
		}

		#region Borders
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
			renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPBORDERS, 0);
			if (e.ToolStrip.IsDropDown) {
				Region oldClip = e.Graphics.Clip;

				//Tool strip borders are rendered *after* the content, for some reason.
				//So we have to exclude the inside of the popup otherwise we'll draw over it.
				Rectangle insideRect = e.ToolStrip.ClientRectangle;
				insideRect.Inflate(-1, -1);
				e.Graphics.ExcludeClip(insideRect);

				renderer.DrawBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);

				//Restore the old clip in case the Graphics is used again (does that ever happen?)
				e.Graphics.Clip = oldClip;
			}
		}
		#endregion

		#region Backgrounds
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
			int partId = e.Item.Owner.IsDropDown ? (int)MenuParts.MENU_POPUPITEM : (int)MenuParts.MENU_BARITEM;
			renderer.SetParameters(MenuClass, partId, GetItemState(e.Item));
			
			Rectangle bgRect = e.Item.ContentRectangle;

			Padding content = GetThemeMargins(e.Graphics, MarginTypes.TMT_CONTENTMARGINS),
					sizing = GetThemeMargins(e.Graphics, MarginTypes.TMT_SIZINGMARGINS),
					caption = GetThemeMargins(e.Graphics, MarginTypes.TMT_CAPTIONMARGINS);

			if (!e.Item.Owner.IsDropDown) {
				bgRect.Y = 0;
				bgRect.Height = e.ToolStrip.Height;
				bgRect.Inflate(-1, -1); //GetMargins here perhaps?
			}

			renderer.DrawBackground(e.Graphics, bgRect, bgRect);
		}

		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e) {
			//Draw the background using Rebar & RP_BACKGROUND (or, if that is not available, fall back to
			//Rebar.Band.Normal)
			if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement(RebarClass, RP_BACKGROUND, 0))) {
				renderer.SetParameters(RebarClass, RP_BACKGROUND, 0);
			} else {
				renderer.SetParameters(RebarClass, 0, 0);
				//renderer.SetParameters(VisualStyleElement.Taskbar.BackgroundBottom.Normal);
				//renderer.SetParameters(Subclass(VisualStyleElement.Rebar.Band.Normal));
			}

			if (renderer.IsBackgroundPartiallyTransparent())
				renderer.DrawParentBackground(e.Graphics, e.ToolStripPanel.ClientRectangle, e.ToolStripPanel);

			renderer.DrawBackground(e.Graphics, e.ToolStripPanel.ClientRectangle);

			//Draw the etched edges of each row.
			//renderer.SetParameters(Subclass(VisualStyleElement.Rebar.Band.Normal));
			//foreach (ToolStripPanelRow row in e.ToolStripPanel.Rows) {
			//    Rectangle rowBounds = row.Bounds;
			//    rowBounds.Offset(0, -1);
			//    renderer.DrawEdge(e.Graphics, rowBounds, Edges.Top, EdgeStyle.Etched, EdgeEffects.None);
			//}

			e.Handled = true;
		}

		//Render the background of an actual menu bar, dropdown menu or toolbar.
		protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e) {
			if (e.ToolStrip.IsDropDown) {
				renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPBACKGROUND, 0);
			} else {
				//It's a MenuStrip or a ToolStrip. If it's contained inside a larger panel, it should have a
				//transparent background, showing the panel's background.

				if (e.ToolStrip.Parent is ToolStripPanel) {
					//The background should be transparent, because the ToolStripPanel's background will be visible.
					//(Of course, we assume the ToolStripPanel is drawn using the same theme, but it's not my fault
					//if someone does that.)
					return;
				} else {
					//A lone toolbar/menubar should act like it's inside a toolbox, I guess.
					//Maybe I should use the MenuClass in the case of a MenuStrip, although that would break
					//the other themes...
					if(VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement(RebarClass, RP_BACKGROUND, 0)))
						renderer.SetParameters(RebarClass, RP_BACKGROUND, 0);
					else
						renderer.SetParameters(RebarClass, 0, 0);
				}
			}

			if (renderer.IsBackgroundPartiallyTransparent())
				renderer.DrawParentBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.ToolStrip);

			renderer.DrawBackground(e.Graphics, e.ToolStrip.ClientRectangle, e.AffectedBounds);
		}

		protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e) {
			//e.Graphics.FillRectangle(Brushes.RosyBrown, e.ToolStripContentPanel.ClientRectangle);
			//base.OnRenderToolStripContentPanelBackground(e);
		}

		//Some sort of chevron thing?
		//protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e) {
		//    base.OnRenderOverflowButtonBackground(e);
		//}
		#endregion

		#region Text
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
			int partId = e.Item.Owner.IsDropDown ? (int)MenuParts.MENU_POPUPITEM : (int)MenuParts.MENU_BARITEM;
			renderer.SetParameters(MenuClass, partId, GetItemState(e.Item));
			Color color = renderer.GetColor(ColorProperty.TextColor);

			if(e.Item.Owner.IsDropDown || e.Item.Owner is MenuStrip)
				e.TextColor = color;

			base.OnRenderItemText(e);
		}
		#endregion

		#region Glyphs

		//protected override void OnRenderGrip(ToolStripGripRenderEventArgs e) {
		//    if (e.GripStyle == ToolStripGripStyle.Visible) {
		//        renderer.SetParameters(VisualStyleElement.Rebar.Gripper.Normal);
		//        renderer.DrawBackground(e.Graphics, e.GripBounds, e.AffectedBounds);
		//    }
		//}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {
			if (e.ToolStrip.IsDropDown) {
				renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPGUTTER, 0);
				Rectangle displayRect = e.ToolStrip.DisplayRectangle,
					marginRect = new Rectangle(0, displayRect.Top, displayRect.Left, displayRect.Height);
				//e.Graphics.DrawRectangle(Pens.Black, marginRect);
				renderer.DrawBackground(e.Graphics, marginRect, marginRect);
			}
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
			if (e.ToolStrip.IsDropDown) {
				renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPSEPARATOR, 0);
				Rectangle rect = new Rectangle(e.ToolStrip.DisplayRectangle.Left, 0, e.ToolStrip.DisplayRectangle.Width, e.Item.Height);
				renderer.DrawBackground(e.Graphics, rect, rect);
			} else {
				base.OnRenderSeparator(e);
			}
		}

		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) {
			ToolStripMenuItem item = e.Item as ToolStripMenuItem;
			if (item != null) {
				if (item.Checked) {
					Rectangle rect = e.Item.ContentRectangle;
					rect.Width = rect.Height;

					//Center the checkmark horizontally in the gutter (looks ugly, though)
					//rect.X = (e.ToolStrip.DisplayRectangle.Left - rect.Width) / 2;

					renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPCHECKBACKGROUND, e.Item.Enabled ? (int)MenuPopupCheckBackgroundStates.MCB_NORMAL : (int)MenuPopupCheckBackgroundStates.MCB_DISABLED);
					renderer.DrawBackground(e.Graphics, rect);
					
					Padding margins = GetThemeMargins(e.Graphics, MarginTypes.TMT_SIZINGMARGINS);

					rect = new Rectangle(rect.X + margins.Left, rect.Y + margins.Top,
						rect.Width - margins.Horizontal,
						rect.Height - margins.Vertical);

					//I don't think ToolStrip even supports radio box items. So no need to render them.
					renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPCHECK, e.Item.Enabled ? (int)MenuPopupCheckStates.MC_CHECKMARKNORMAL : (int)MenuPopupCheckStates.MC_CHECKMARKDISABLED);

					renderer.DrawBackground(e.Graphics, rect);
				}
			} else {
				base.OnRenderItemCheck(e);
			}
		}

		//This is broken for RTL
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
			int stateId = e.Item.Enabled ? (int)MenuPopupSubMenuStates.MSM_NORMAL : (int)MenuPopupSubMenuStates.MSM_DISABLED;
			renderer.SetParameters(MenuClass, (int)MenuParts.MENU_POPUPSUBMENU, stateId);
			renderer.DrawBackground(e.Graphics, e.ArrowRectangle);
		}

		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e) {
			renderer.SetParameters(RebarClass, VisualStyleElement.Rebar.Chevron.Normal.Part, VisualStyleElement.Rebar.Chevron.Normal.State);
			renderer.DrawBackground(e.Graphics, e.Item.ContentRectangle);

			//base.OnRenderOverflowButtonBackground(e);
		}
		#endregion

        private static bool? _isSupportedCache = null;

		public static bool IsSupported {
			get {
                if (_isSupportedCache.HasValue)
                    return _isSupportedCache.Value;

                if (!VisualStyleRenderer.IsSupported) {
                    _isSupportedCache = false;
                    return false;
                }

				_isSupportedCache = VisualStyleRenderer.IsElementDefined(VisualStyleElement.CreateElement("MENU", (int)MenuParts.MENU_BARBACKGROUND, (int)MenuBarStates.MB_ACTIVE));
                return _isSupportedCache.Value;
			} 
		}
	}

	/// <summary>
	/// Renders a toolstrip using UXTheme if possible, and switches back to the default
	/// ToolStripRenderer when UXTheme-based rendering is not available.
	/// Designed for menu bars and context menus - it is not guaranteed to work with anything else.
	/// </summary>
	/// <example>
	/// NativeToolStripRenderer.SetToolStripRenderer(toolStrip1, toolStrip2, contextMenuStrip1);
	/// </example>
	/// <example>
	/// toolStrip1.Renderer = new NativeToolStripRenderer();
	/// </example>
	public class NativeToolStripRenderer : ToolStripRenderer {
		UXThemeToolStripRenderer nativeRenderer;
		ToolStripRenderer defaultRenderer;
		ToolStrip toolStrip;

		//NativeToolStripRenderer looks best with no padding - but keep the old padding in case the
		//visual styles become unsupported again (e.g. user changes to windows classic skin)
		Padding defaultPadding;

		#region Constructors
		/// <summary>
		/// Creates a NativeToolStripRenderer for a particular ToolStrip. NativeToolStripRenderer  will subscribe to some events
		/// of this ToolStrip.
		/// </summary>
		/// <param name="toolStrip">The toolstrip for this NativeToolStripRenderer. NativeToolStripRenderer  will subscribe to some events
		/// of this ToolStrip.</param>
		public NativeToolStripRenderer(ToolStrip toolStrip, ToolbarTheme theme) {
			if (toolStrip == null)
				throw new ArgumentNullException("toolStrip", "ToolStrip cannot be null.");

			Theme = theme;

			this.toolStrip = toolStrip;
			defaultRenderer = toolStrip.Renderer;

			defaultPadding = toolStrip.Padding;
			toolStrip.SystemColorsChanged += new EventHandler(toolStrip_SystemColorsChanged);

			//Can't initialize here - constructor throws if visual styles not enabled
			//nativeRenderer = new NativeToolStripRenderer();
		}

		public NativeToolStripRenderer(ToolStripPanel panel, ToolbarTheme theme) {
			if (panel == null)
				throw new ArgumentNullException("panel", "Panel cannot be null.");

			Theme = theme;

			this.toolStrip = null;
			defaultRenderer = panel.Renderer;
		}
		#endregion

		public ToolbarTheme Theme { get; set; }

		void toolStrip_SystemColorsChanged(object sender, EventArgs e) {
			if (toolStrip == null)
				return;

			if (UXThemeToolStripRenderer.IsSupported)
				toolStrip.Padding = Padding.Empty;
			else
				toolStrip.Padding = defaultPadding;
		}

		//This is indeed called every time a menu part is rendered, but I can't
		//find a way of caching it that I can be sure has no race conditions.
		//The check is no longer very costly, anyway.
		protected ToolStripRenderer ActualRenderer {
			get {
				bool nativeSupported = UXThemeToolStripRenderer.IsSupported;
				
				if (nativeSupported) {
					if (nativeRenderer == null)
						nativeRenderer = new UXThemeToolStripRenderer(Theme);
					return nativeRenderer;
				}

				return defaultRenderer;
			}
		}

		#region InitializeXXX
		protected override void Initialize(ToolStrip toolStrip) {
			base.Initialize(toolStrip);

			toolStrip.Padding = Padding.Empty;

			if (/*!(toolStrip is MenuStrip) &&*/ toolStrip.Parent is ToolStripPanel) {
				toolStrip.BackColor = Color.Transparent;
			}
		}

		protected override void InitializePanel(ToolStripPanel toolStripPanel) {
			base.InitializePanel(toolStripPanel);
		}

		protected override void InitializeItem(ToolStripItem item) {
			base.InitializeItem(item);
		}
		#endregion

		#region SetToolStripRenderer
		/// <summary>
		/// Sets the renderer of each ToolStrip to a NativeToolStripRenderer. A convenience method.
		/// </summary>
		/// <param name="toolStrips">A parameter list of ToolStrips.</param>
		[SuppressMessage("Microsoft.Design", "CA1062")] //The parameter array is actually checked.
		public static void SetToolStripRenderer(ToolbarTheme theme, params Control[] toolStrips) {
			foreach (Control ts in toolStrips) {
				if (ts == null)
					throw new ArgumentNullException("toolStrips", "ToolStrips cannot contain a null reference.");
			}

			foreach (Control ts in toolStrips) {
				if (ts is ToolStrip) {
					ToolStrip t = (ToolStrip)ts;
					t.Renderer = new NativeToolStripRenderer(t, theme);
				} else if (ts is ToolStripPanel) {
					ToolStripPanel t = (ToolStripPanel)ts;
					t.Renderer = new NativeToolStripRenderer(t, theme);
				}  else
					throw new ArgumentException("Can't set the renderer for a " + ts.GetType().Name);
			}
		}

		public static void SetToolStripRenderer(params Control[] toolStrips) {
			SetToolStripRenderer(ToolbarTheme.Toolbar, toolStrips);
		}
		#endregion

		#region Overridden Methods - Deferred to actual renderer
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
			ActualRenderer.DrawArrow(e);
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) {
			ActualRenderer.DrawButtonBackground(e);
		}

		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e) {
			ActualRenderer.DrawDropDownButtonBackground(e);
		}

		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e) {
			ActualRenderer.DrawGrip(e);
		}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {
			ActualRenderer.DrawImageMargin(e);
		}

		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e) {
			ActualRenderer.DrawItemBackground(e);
		}

		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) {
			ActualRenderer.DrawItemCheck(e);
		}

		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e) {
			ActualRenderer.DrawItemImage(e);
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
			ActualRenderer.DrawItemText(e);
		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
			ActualRenderer.DrawMenuItemBackground(e);
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
			ActualRenderer.DrawSeparator(e);
		}

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
			ActualRenderer.DrawToolStripBackground(e);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
			ActualRenderer.DrawToolStripBorder(e);
		}

		protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e) {
			ActualRenderer.DrawToolStripContentPanelBackground(e);
		}

		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e) {
			ActualRenderer.DrawToolStripPanelBackground(e);
		}
		#endregion
	}
}
