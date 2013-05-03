using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Platforms {
    
#if DEBUG

    /// <summary>
    /// Fake platform for debugging.
    /// </summary>
    class DebugPlatform : PlatformSupport {
        
        public override bool CheckCompatibility() {
            return true;
        }

    }

#endif

}
