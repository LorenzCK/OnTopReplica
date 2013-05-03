using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsAero;
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
                if(!string.IsNullOrEmpty(Text))
				    OnConfirmInput();

				e.Handled = true;
                e.SuppressKeyPress = true;
			}
            else if (e.KeyCode == Keys.Escape) {
                OnAbortInput();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            //Console.WriteLine("{0} ({1})", e.KeyCode, e.KeyValue);

            base.OnKeyUp(e);
		}

        //List of characters to ignore on KeyPress events (because they generate bell rings)
        readonly char[] IgnoreChars = new char[] {
            (char)27, (char)13
        };

        protected override void OnKeyPress(KeyPressEventArgs e) {
            if (IgnoreChars.Contains(e.KeyChar)) {
                e.Handled = true;
            }

            base.OnKeyPress(e);
        }

		public event EventHandler ConfirmInput;

		protected virtual void OnConfirmInput() {
            var evt = ConfirmInput;
            if (evt != null)
                evt(this, EventArgs.Empty);
		}

        public event EventHandler AbortInput;

        protected virtual void OnAbortInput() {
            var evt = AbortInput;
            if (evt != null)
                evt(this, EventArgs.Empty);
        }

	}

}
