using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {

    partial class MainForm {

        EventHandler RequestClosingHandler;

        const int SidePanelMargin = 1;

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
            _sidePanelContainer.Enabled = true;
            _sidePanelContainer.Show();
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
            FixPositionAndSize();

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

            //Unhook listener to make panel freeable
            _currentSidePanel.RequestClosing -= RequestClosingHandler;

            //Remove side panel and container
            _currentSidePanel.OnClosing(this);
            _currentSidePanel.Hide();
            _sidePanelContainer.Controls.Clear();
            _sidePanelContainer.Visible = _sidePanelContainer.Enabled = false;

            //Free panel
            _currentSidePanel.Dispose();
            _currentSidePanel = null;

            //Resize
            MinimumSize = new Size(20, 20);
            ClientSize = new Size(
                ClientSize.Width - _sidePanelContainer.Width - SidePanelMargin,
                ClientSize.Height
            );
            ExtraPadding = new Padding(0);
            _thumbnailPanel.Size = ClientSize;

            //Put focus back to main form
            this.Focus();
        }

        void SidePanel_RequestClosing(object sender, EventArgs e) {
            CloseSidePanel();
        }

        void Thumbnail_CloneClick(object sender, CloneClickEventArgs e) {
            Win32Helper.InjectFakeMouseClick(CurrentThumbnailWindowHandle.Handle, e);
        }

    }

}
