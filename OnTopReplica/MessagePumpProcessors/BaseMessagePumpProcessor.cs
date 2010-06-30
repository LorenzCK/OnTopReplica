using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.MessagePumpProcessors {
    abstract class BaseMessagePumpProcessor : IMessagePumpProcessor {

        protected MainForm Form { get; private set; }

        #region IMessagePumpProcessor Members

        public void Initialize(MainForm form) {
            Form = form;
        }

        public abstract void Process(Message msg);

        #endregion

        bool _isDisposed = false;

        protected abstract void Shutdown();

        #region IDisposable Members

        public void Dispose() {
            if (_isDisposed)
                return;

            Shutdown();

            _isDisposed = true;
        }

        #endregion

    }
}
