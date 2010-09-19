using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Update {
    class UpdateCheckCompletedEventArgs : EventArgs {

        public UpdateInformation Information { get; set; }

        public bool Success { get; set; }

        public Exception Error { get; set; }

    }
}
