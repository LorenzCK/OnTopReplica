using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Properties;

namespace OnTopReplica {
    /// <summary>
    /// Notification icon that installs itself in the "tray" and manipulates the OnTopReplica main form.
    /// </summary>
    class NotificationIcon : IDisposable {

        public NotificationIcon(MainForm form) {
            Form = form;
            Install();
        }

        public MainForm Form { get; private set; }

        NotifyIcon _taskIcon;
        ContextMenuStrip _contextMenu;

        private void Install() {
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem(Strings.MenuOpen, Resources.icon, TaskIconOpen_click) {
                    ToolTipText = Strings.MenuOpenTT,
                },
                new ToolStripMenuItem(Strings.MenuWindows, Resources.list){
                    DropDown = Form.MenuWindows,
                    ToolTipText = Strings.MenuWindowsTT
                },
                new ToolStripMenuItem(Strings.MenuReset, null, TaskIconReset_click){
                    ToolTipText = Strings.MenuResetTT
                },
                new ToolStripMenuItem(Strings.MenuClose, Resources.close_new, TaskIconExit_click){
                    ToolTipText = Strings.MenuCloseTT
                }
            });
            Asztal.Szótár.NativeToolStripRenderer.SetToolStripRenderer(_contextMenu);

            _taskIcon = new NotifyIcon {
                Text = Strings.ApplicationName,
                Icon = Resources.icon_new,
                Visible = true,
                ContextMenuStrip = _contextMenu
            };
            _taskIcon.DoubleClick += new EventHandler(TaskIcon_doubleclick);
        }

        #region IDisposable Members

        public void Dispose() {
            //Destroy NotifyIcon
            if (_taskIcon != null) {
                _taskIcon.Visible = false;
                _taskIcon.Dispose();
                _taskIcon = null;
            }
        }

        #endregion

        #region Task Icon events

        void TaskIcon_doubleclick(object sender, EventArgs e) {
            Form.EnsureMainFormVisible();
        }

        private void TaskIconOpen_click(object sender, EventArgs e) {
            Form.EnsureMainFormVisible();
        }

        private void TaskIconReset_click(object sender, EventArgs e) {
            Form.ResetMainFormWithConfirmation();
        }

        private void TaskIconExit_click(object sender, EventArgs e) {
            Form.Close();
        }

        #endregion

    }
}
