using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {

    partial class MainForm {

        //SidePanel _currentSidePanel = null;
        SidePanelContainer _sidePanelContainer = null;

        /// <summary>
        /// Opens a new side panel.
        /// </summary>
        /// <param name="panel">The side panel to embed.</param>
        public void SetSidePanel(SidePanel panel) {
            if (IsSidePanelOpen) {
                CloseSidePanel();
            }

            _sidePanelContainer = new SidePanelContainer(this);
            _sidePanelContainer.SetSidePanel(panel);
            _sidePanelContainer.Location = ComputeSidePanelLocation(_sidePanelContainer);
            _sidePanelContainer.Show(this);
        }

        /// <summary>
        /// Closes the current side panel.
        /// </summary>
        public void CloseSidePanel() {
            if (_sidePanelContainer == null || _sidePanelContainer.IsDisposed) {
                _sidePanelContainer = null;
                return;
            }

            _sidePanelContainer.Hide();
            _sidePanelContainer.FreeSidePanel();
        }

        /// <summary>
        /// Gets whether a side panel is currently shown.
        /// </summary>
        public bool IsSidePanelOpen {
            get {
                if (_sidePanelContainer == null)
                    return false;
                if (_sidePanelContainer.IsDisposed) {
                    _sidePanelContainer = null;
                    return false;
                }

                return _sidePanelContainer.Visible;
            }
        }

        /// <summary>
        /// Moves the side panel based on the main form's current location.
        /// </summary>
        protected void AdjustSidePanelLocation() {
            if (!IsSidePanelOpen)
                return;

            _sidePanelContainer.Location = ComputeSidePanelLocation(_sidePanelContainer);
        }

        /// <summary>
        /// Computes the target location of a side panel form that ensures it is visible on the current
        /// screen that contains the main form.
        /// </summary>
        private Point ComputeSidePanelLocation(Form sidePanel) {
            //Check if moving the panel on the form's right would put it off-screen
            var screen = Screen.FromControl(this);
            if (Location.X + Width + sidePanel.Width > screen.WorkingArea.Right) {
                return new Point(Location.X - sidePanel.Width, Location.Y);
            }
            else {
                return new Point(Location.X + Width, Location.Y);
            }
        }

        void SidePanel_RequestClosing(object sender, EventArgs e) {
            CloseSidePanel();
        }

        void Thumbnail_CloneClick(object sender, CloneClickEventArgs e) {
            Win32Helper.InjectFakeMouseClick(CurrentThumbnailWindowHandle.Handle, e);
        }

    }

}
