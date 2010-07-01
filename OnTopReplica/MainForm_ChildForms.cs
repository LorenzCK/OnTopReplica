using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {
    partial class MainForm {

        EventHandler RequestClosingHandler;

        const int SidePanelMargin = 1;
        const int ScreenBorderMargin = 10;

        bool _sidePanelDidMoveForm = false;
        Point _sidePanelPreviousFormLocation;

        /// <summary>
        /// Opens a new side panel.
        /// </summary>
        /// <param name="panel">The side panel to embed.</param>
        public void SetSidePanel(SidePanel panel) {
            if (_currentSidePanel != null) {
                CloseSidePanel();
            }

            _currentSidePanel = panel;
            _currentSidePanel.OnFirstShown(this);

            //Add and show
            _sidePanelContainer.Controls.Add(panel);
            _sidePanelContainer.Visible = _sidePanelContainer.Enabled = true;
            panel.Show();

            int intHorzMargin = panel.Margin.Horizontal + _sidePanelContainer.Padding.Horizontal; //internal margins for sidepanel
            int intVertMargin = panel.Margin.Vertical + _sidePanelContainer.Padding.Vertical;

            //Resize container
            _sidePanelContainer.ClientSize = new Size(
                panel.MinimumSize.Width + intHorzMargin,
                ClientSize.Height
            );

            int rightHorzMargin = _sidePanelContainer.Width + SidePanelMargin; //horz margin between the two controls

            //Resize rest
            MinimumSize = new Size(
                20 + panel.MinimumSize.Width + SidePanelMargin + intHorzMargin,
                panel.MinimumSize.Height + intVertMargin
            );
            ClientSize = new Size(
                ClientSize.Width + rightHorzMargin,
                ClientSize.Height
            );
            ExtraPadding = new Padding(0, 0, rightHorzMargin, 0);
            _thumbnailPanel.Size = new Size(
                ClientSize.Width - rightHorzMargin,
                ClientSize.Height
            );
            _sidePanelContainer.Location = new Point(
                ClientSize.Width - rightHorzMargin,
                0
            );

            //Move window if needed
            var screenCurr = Screen.FromControl(this);
            int pRight = Location.X + Width + ScreenBorderMargin;
            if (pRight >= screenCurr.Bounds.Width) {
                _sidePanelPreviousFormLocation = Location;
                _sidePanelDidMoveForm = true;

                Location = new Point(screenCurr.WorkingArea.Width - Width - ScreenBorderMargin, Location.Y);
            }
            else {
                _sidePanelDidMoveForm = false;
            }

            //Hook event listener
            if (RequestClosingHandler == null)
                RequestClosingHandler = new EventHandler(SidePanel_RequestClosing);
            panel.RequestClosing += RequestClosingHandler;
        }

        /// <summary>
        /// Closes the current side panel.
        /// </summary>
        public void CloseSidePanel() {
            if (_currentSidePanel == null)
                return;

            //Unhook listener
            _currentSidePanel.RequestClosing -= RequestClosingHandler;

            //Remove side panel
            _currentSidePanel.OnClosing(this);
            _sidePanelContainer.Controls.Clear();
            _sidePanelContainer.Visible = _sidePanelContainer.Enabled = false;
            _currentSidePanel = null;

            //Resize
            MinimumSize = new Size(20, 20);
            ClientSize = new Size(
                ClientSize.Width - _sidePanelContainer.Width - SidePanelMargin,
                ClientSize.Height
            );
            ExtraPadding = new Padding(0);
            _thumbnailPanel.Size = ClientSize;

            //Move window back if needed
            if (_sidePanelDidMoveForm) {
                Location = _sidePanelPreviousFormLocation;
                _sidePanelDidMoveForm = false;
            }
        }

        void SidePanel_RequestClosing(object sender, EventArgs e) {
            CloseSidePanel();
        }

        void Thumbnail_CloneClick(object sender, CloneClickEventArgs e) {
            //TODO: handle other mouse buttons
            Win32Helper.InjectFakeMouseClick(_lastWindowHandle.Handle, e.ClientClickLocation, e.IsDoubleClick);
        }

    }
}
