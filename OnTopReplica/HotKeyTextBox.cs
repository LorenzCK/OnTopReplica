using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {
    
    class HotKeyTextBox : TextBox {

        protected override void OnCreateControl() {
            ReadOnly = true;

            base.OnCreateControl();
        }

        readonly Keys[] IgnoredKeys = new Keys[] {
            Keys.ControlKey,
            Keys.Control,
            Keys.Alt,
            Keys.Menu,
            Keys.ShiftKey,
            Keys.Shift,
            Keys.LWin,
            Keys.RWin
        };

        readonly Keys[] CancelKeys = new Keys[] {
            Keys.Back,
            Keys.Escape
        };

        protected override void OnKeyUp(KeyEventArgs e) {
            if (CancelKeys.Contains(e.KeyCode)) {
                Text = string.Empty;
            }
            else if (!IgnoredKeys.Contains(e.KeyCode)) {
                var sb = new StringBuilder();
                if (e.Control)
                    sb.Append("[CTRL]+");
                if (e.Alt)
                    sb.Append("[ALT]+");
                if (e.Shift)
                    sb.Append("[SHIFT]+");
                sb.Append(e.KeyCode.ToString());

                Text = sb.ToString();
            }
            
            e.Handled = true;
            base.OnKeyUp(e);
        }

    }

}
