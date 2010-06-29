using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;
using OnTopReplica.Properties;
using VistaControls.TaskDialog;

namespace OnTopReplica.Platforms {

    class WindowsVista : PlatformSupport {
        
        public override bool CheckCompatibility() {
            if (!VistaControls.OsSupport.IsCompositionEnabled) {
                MessageBox.Show(Strings.ErrorDwmOffContent, Strings.ErrorDwmOff, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public override void InitForm(MainForm form) {
            base.InitForm(form);

            DwmManager.SetWindowFlip3dPolicy(form, Flip3DPolicy.ExcludeAbove);

            this.InitFormCore(form);
        }

        protected virtual void InitFormCore(MainForm form){
            //Do not show in task bar, but display icon
            form.ShowInTaskbar = false;

            //Install tray icon
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem(Strings.MenuOpen, Resources.icon, TaskIconOpen_click) {
                    ToolTipText = Strings.MenuOpenTT,
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
                Icon = Resources.main_icon,
                Visible = true,
                ContextMenuStrip = _contextMenu
            };
            _taskIcon.DoubleClick += new EventHandler(TaskIcon_doubleclick);
        }

        NotifyIcon _taskIcon;
        ContextMenuStrip _contextMenu;

        public override void ShutdownApp() {
            //Destroy NotifyIcon
            if (_taskIcon != null) {
                _taskIcon.Visible = false;
                _taskIcon.Dispose();
                _taskIcon = null;
            }
        }

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
