using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.MessagePumpProcessors {
    abstract class BaseMessagePumpProcessor : IMessagePumpProcessor {

        protected MainForm Form { get; private set; }

        #region IMessagePumpProcessor Members

        public virtual void Initialize(MainForm form) {
            Form = form;
        }

        public abstract bool Process(ref Message msg);

        #endregion

        protected abstract void Shutdown();

        bool _isDisposed = false;

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
