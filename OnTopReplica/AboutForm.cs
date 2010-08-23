using System.Diagnostics;
using System.Windows.Forms;
using VistaControls.Dwm.Helpers;
using System.Drawing;
using System.Runtime.InteropServices;
using OnTopReplica.Properties;
using System.Deployment.Application;
using System;
using VistaControls.TaskDialog;
using System.ComponentModel;
using OnTopReplica.Update;

namespace OnTopReplica {
	public partial class AboutForm : GlassForm {
		
		public AboutForm() {
			InitializeComponent();

            //Tooltips
            toolTip.SetToolTip(buttonExpander, Strings.AboutButtonExpanderTT);
            toolTip.SetToolTip(buttonReset, Strings.AboutButtonResetTT);
            toolTip.SetToolTip(buttonUpdate, Strings.AboutButtonUpdateTT);

			//Add link areas (localized text)
			linkLabel1.LinkArea = new LinkArea(linkLabel1.Text.IndexOf("Lorenz Cuno Klopfenstein"), "Lorenz Cuno Klopfenstein".Length);
			int linkStart = linkLabel2.Text.IndexOf("www");
			linkLabel2.LinkArea = new LinkArea(linkStart, linkLabel2.Text.Length - linkStart - 1);

			//Glassify
			GlassEnabled = true;
			GlassMargins = new VistaControls.Dwm.Margins(0, 0, themedLabel1.Size.Height, 0);

            //Update title
			themedLabel2.Text = "v" + Application.ProductVersion.Substring(0, 3);

			//Add update event handling
            _updateManager.UpdateCheckCompleted += new EventHandler<UpdateCheckCompletedEventArgs>(UpdateManager_UpdateCheckCompleted);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape)
				this.Close();

			base.OnKeyUp(e);
		}

		private void Lck_click(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://lorenz.klopfenstein.net");
		}

		private void Homepage_click(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://www.codeplex.com/ontopreplica");
		}

		private void ShowGenericError(string title, string mainContent, Exception ex) {
			TaskDialog.Show(mainContent, title, ex.Message, TaskDialogButton.Close, TaskDialogIcon.Stop);
		}

		#region Bottom toggler

		private void Toggle_click(object sender, System.EventArgs e) {
			IsExpanded = !IsExpanded;

			//Update icon
			buttonExpander.Image = IsExpanded ? Resources.xiao_up : Resources.xiao_down;
		}

		bool _isExpanded = false;
		bool _isFirstExpansion = true;
		public bool IsExpanded {
			get { return _isExpanded; }
			set {
				if(_isExpanded != value)
					Size = new Size(Size.Width, Size.Height + ((value) ? webBrowser.Size.Height : -webBrowser.Size.Height));

				_isExpanded = value;

				if (value && _isFirstExpansion) {
					//Load text from resources
					webBrowser.DocumentText = Strings.AboutDetails;

					//Register navigation events
					webBrowser.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);

					_isFirstExpansion = false;
				}
			}
		}

		void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e) {
			if (e.Url != null && e.Url.IsAbsoluteUri) {
				Process.Start(e.Url.ToString());
				e.Cancel = true;
			}
		}

		#endregion

		#region Updating

        UpdateManager _updateManager = new UpdateManager();

		private void Update_click(object sender, System.EventArgs e) {
            CheckForUpdate();
		}

        delegate void GuiAction();

        void UpdateManager_UpdateCheckCompleted(object sender, UpdateCheckCompletedEventArgs e) {
            Invoke(new GuiAction(() => {
                if (e.Success) {
                    _updateManager.HandleUpdateCheck(this, e.Information, true);
                }
                else {
                    var dlg = new TaskDialog(Strings.ErrorUpdate, Strings.ErrorUpdate, Strings.ErrorUpdateContentGeneric) {
                        CommonIcon = TaskDialogIcon.Stop,
                        CommonButtons = TaskDialogButton.OK
                    };
                    dlg.Show(this);
                }

                UpdateStopped();
            }));
        }

		public void CheckForUpdate() {
			//Update GUI
			buttonUpdate.Visible = false;
			progressBar1.Visible = true;
            progressBar1.Value = 50;

            _updateManager.CheckForUpdate();
		}

		void UpdateStopped() {
			//Reset UI
			progressBar1.Visible = false;
			buttonUpdate.Visible = true;
		}

		#endregion

        private void ResetClick(object sender, EventArgs e) {
            var dlg = new TaskDialog(Strings.AskSettingReset, Strings.AskSettingResetTitle,
                Strings.AskSettingResetContent);
            dlg.CommonButtons = TaskDialogButton.OK | TaskDialogButton.Cancel;

            if (dlg.Show(this).CommonButton == Result.OK) {
                Settings.Default.Reset();
            }
        }

	}
}
