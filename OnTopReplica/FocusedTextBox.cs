using System;
using System.Collections.Generic;
using System.Text;
using VistaControls;
using System.Windows.Forms;

namespace OnTopReplica {

	class FocusedTextBox : System.Windows.Forms.TextBox {

		protected override bool IsInputChar(char charCode) {
			if (charCode == '\n' || charCode == '\r')
				return true;

			return base.IsInputChar(charCode);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			if (e.KeyCode == Keys.Return) {
				OnConfirmInput();
				e.Handled = true;
			}

			base.OnKeyUp(e);
		}

		public event EventHandler ConfirmInput;

		protected virtual void OnConfirmInput() {
			if (ConfirmInput != null)
				ConfirmInput(this, EventArgs.Empty);
		}

	}

}
