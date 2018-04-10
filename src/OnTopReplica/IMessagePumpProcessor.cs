using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {

    interface IMessagePumpProcessor : IDisposable {

        void Initialize(MainForm form);

        bool Process(ref Message msg);

    }
}
