using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices {
    /// <summary>
    /// Fake extension attribute that adds extension method support to C# 2 (without System.Core.dll reference).
    /// </summary>
    class ExtensionAttribute : Attribute {
    }
}
