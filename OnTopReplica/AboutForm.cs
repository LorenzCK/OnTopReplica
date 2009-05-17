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

namespace OnTopReplica {
	public partial class AboutForm : GlassForm {
		
		public AboutForm() {
			InitializeComponent();

			//Add link areas (localized text)
			linkLabel1.LinkArea = new LinkArea(linkLabel1.Text.IndexOf("Lorenz Cuno Klopfenstein"), "Lorenz Cuno Klopfenstein".Length);
			int linkStart = linkLabel2.Text.IndexOf("www");
			linkLabel2.LinkArea = new LinkArea(linkStart, linkLabel2.Text.Length - linkStart - 1);

			//Glassify
			GlassEnabled = true;
			GlassMargins = new VistaControls.Dwm.Margins(0, 0, themedLabel1.Size.Height, 0);

			//Remove title and icon from title bar
			//  Code taken from: https://secure.codeproject.com/KB/vista/HideCaptionIcon.aspx
			NativeMethods.WTA_OPTIONS ops = new NativeMethods.WTA_OPTIONS();
			ops.Flags = NativeMethods.WTNCA_NODRAWCAPTION | NativeMethods.WTNCA_NODRAWICON;
			ops.Mask = NativeMethods.WTNCA_NODRAWCAPTION | NativeMethods.WTNCA_NODRAWICON;
			NativeMethods.SetWindowThemeAttribute(this.Handle,
				NativeMethods.WindowThemeAttributeType.WTA_NONCLIENT,
				ref ops,
				(uint)Marshal.SizeOf(typeof(NativeMethods.WTA_OPTIONS))
			);

			themedLabel2.Text = "v" + Application.ProductVersion.Substring(0, 3);

			//Create event handlers
			handlerProgressChange = new DeploymentProgressChangedEventHandler(CurrentDeployment_CheckForUpdateProgressChanged);
			handlerProgressComplete = new CheckForUpdateCompletedEventHandler(CurrentDeployment_CheckForUpdateCompleted);

			handlerUpdateChange = new DeploymentProgressChangedEventHandler(deployment_UpdateProgressChanged);
			handlerUpdateComplete = new AsyncCompletedEventHandler(deployment_UpdateCompleted);
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
			button1.Image = IsExpanded ? Resources.xiao_up : Resources.xiao_down;
		}

		bool _isExpanded = false;
		bool _isFirstExpansion = true;
		public bool IsExpanded {
			get { return _isExpanded; }
			set {
				if(_isExpanded != value)
					Size = new Size(Size.Width, Size.Height + ((value) ? webBrowser1.Size.Height : -webBrowser1.Size.Height));

				_isExpanded = value;

				if (value && _isFirstExpansion) {
					//Load text from resources
					webBrowser1.DocumentText = Resources.about;

					//Register navigation events
					webBrowser1.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);

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

		bool _isChecking = false;
		bool _isUpdating = false;

		private void Update_click(object sender, System.EventArgs e) {
			ApplicationDeployment deployment = null;
			try {
				deployment = ApplicationDeployment.CurrentDeployment;
			}
			catch (InvalidDeploymentException ex) {
				var dlg = new TaskDialog(Strings.ErrorUpdate, Strings.ErrorGenericTitle, Strings.ErrorUpdateContent);
				dlg.EnableHyperlinks = true;
				dlg.CommonIcon = TaskDialogIcon.Stop;
				dlg.CommonButtons = TaskDialogButton.Close;
				dlg.ExpandedControlText = Strings.ErrorDetailButton;
				dlg.ExpandedInformation = ex.Message;
				dlg.HyperlinkClick += new EventHandler<HyperlinkEventArgs>(dlg_HyperlinkClick);
				dlg.Show(this);

				return;
			}
			catch(Exception ex) {
				ShowGenericError(Strings.ErrorGenericTitle, Strings.ErrorUpdate, ex);

				return;
			}

			CheckForUpdate(deployment);
		}

		private void Abort_click(object sender, EventArgs e) {
			StopUpdate();
		}

		void dlg_HyperlinkClick(object sender, HyperlinkEventArgs e) {
			Process.Start(e.Url);
		}

		public void CheckForUpdate(ApplicationDeployment deployment) {
			//Add event handlers
			deployment.CheckForUpdateProgressChanged += handlerProgressChange;
			deployment.CheckForUpdateCompleted += handlerProgressComplete;

			//Update GUI
			button2.Visible = false;
			progressBar1.Visible = true;
			progressBar1.Value = 0;
			button3.Visible = true;

			_isChecking = true;

			try {
				deployment.CheckForUpdateAsync();
			}
			catch (Exception ex) {
				ShowGenericError(Strings.ErrorGenericTitle, Strings.ErrorUpdate, ex);

				StopUpdate();
			}
		}

		public void InstallUpdate(ApplicationDeployment deployment) {
			//Add event handlers
			deployment.UpdateProgressChanged += handlerUpdateChange;
			deployment.UpdateCompleted += handlerUpdateComplete;

			//Update GUI
			button2.Visible = false;
			progressBar1.Visible = true;
			progressBar1.Value = 0;
			button3.Visible = true;

			_isUpdating = true;

			try {
				deployment.UpdateAsync();
			}
			catch (Exception ex) {
				ShowGenericError(Strings.ErrorGenericTitle, Strings.ErrorUpdate, ex);

				StopUpdate();
			}
		}

		void StopUpdate() {
			//Reset UI
			progressBar1.Visible = false;
			button3.Visible = false;
			button2.Visible = true;

			try {
				ApplicationDeployment deployment = ApplicationDeployment.CurrentDeployment;

				//Remove all handlers
				deployment.CheckForUpdateProgressChanged -= handlerProgressChange;
				deployment.CheckForUpdateCompleted -= handlerProgressComplete;

				//Abort anything
				if (_isChecking)
					deployment.CheckForUpdateAsyncCancel();
				if (_isUpdating)
					deployment.UpdateAsyncCancel();
			}
			catch {
				return;
			}
			finally {
				_isChecking = false;
				_isUpdating = false;
			}
		}

		DeploymentProgressChangedEventHandler handlerProgressChange;
		CheckForUpdateCompletedEventHandler handlerProgressComplete;

		void CurrentDeployment_CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e) {
			progressBar1.Value = e.ProgressPercentage;
		}

		void CurrentDeployment_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e) {
			progressBar1.Value = 100;
			_isChecking = false;

			if (e.Error != null) {
				ShowGenericError(Strings.ErrorGenericTitle, Strings.ErrorUpdate, e.Error);

				StopUpdate();
				return;
			}

			if (e.Cancelled)
				//Already was aborted
				return;

			ApplicationDeployment deployment = null;
			try {
				deployment = ApplicationDeployment.CurrentDeployment;
			}
			catch {
				//Internal (weird?) error, simply abort
				StopUpdate();
				return;
			}

			if (e.UpdateAvailable) {
				//Install right away if required
				if (e.IsUpdateRequired)
					InstallUpdate(deployment);

				//Ask user
				var dlg = new TaskDialog(string.Format(Strings.AskUpdate, e.AvailableVersion.ToString()), Strings.AskUpdateTitle, Strings.AskUpdateContent);
				dlg.CommonIcon = TaskDialogIcon.Information;
				dlg.UseCommandLinks = true;
				dlg.CustomButtons = new CustomButton[] {
					new CustomButton(Result.OK, string.Format(Strings.AskUpdateButtonOk, e.AvailableVersion.ToString())),
					new CustomButton(Result.Cancel, Strings.AskUpdateButtonCancel)
				};
				dlg.ExpandedInformation = string.Format(Strings.AskUpdateExpanded, Application.ProductVersion, e.AvailableVersion.ToString(), e.UpdateSizeBytes);
				dlg.ExpandedControlText = Strings.ErrorDetailButton;

				if (dlg.Show(this).CommonButton == Result.OK)
					InstallUpdate(deployment);
				else
					StopUpdate();
			}
			else {
				var dlg = new TaskDialog(Strings.InfoUpToDate, Strings.InfoUpToDateTitle);
				dlg.CustomIcon = Icon.FromHandle(Resources.thumbs_up.GetHicon());
				dlg.CommonButtons = TaskDialogButton.Close;
				dlg.Show(this);

				StopUpdate();
			}
		}

		DeploymentProgressChangedEventHandler handlerUpdateChange;
		AsyncCompletedEventHandler handlerUpdateComplete;

		void deployment_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e) {
			progressBar1.Value = e.ProgressPercentage;
		}

		void deployment_UpdateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
			progressBar1.Value = 100;
			_isUpdating = false;

			if (e.Error != null) {
				ShowGenericError(Strings.ErrorGenericTitle, Strings.ErrorUpdate, e.Error);

				StopUpdate();
				return;
			}

			if (e.Cancelled)
				return;

			var dlg = new TaskDialog(Strings.InfoUpdated, Strings.InfoUpdatedTitle, Strings.InfoUpdatedContent);
			dlg.CustomIcon = Icon.FromHandle(Resources.thumbs_up.GetHicon());
			dlg.CommonButtons = TaskDialogButton.Close;
			dlg.Show(this);

			StopUpdate();
		}

		#endregion

	}
}
