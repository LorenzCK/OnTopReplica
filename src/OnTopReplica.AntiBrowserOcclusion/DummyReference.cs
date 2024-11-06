#if NET47
namespace System {
  [Obsolete("This is just a dummy for a clean net47 build, don't use.")]
  internal class ReadOnlySpan<T> {
    public unsafe ReadOnlySpan(char* p1, int p2) {

    }
  }

  [Obsolete("This is just a dummy for a clean net47 build, don't use.")]
  internal class Span<T> {
    public unsafe Span(char* p1, int p2) {

    }
  }
}
#else
namespace OnTopReplica.AntiBrowserOcclusion {
  using System;
  using System.Reflection;
  using System.Runtime.CompilerServices;

  internal class DummyReference {
    public static Assembly ForUnsafe() {
      return typeof(Unsafe).Assembly;
    }

    public static Assembly ForMemory() {
      return typeof(Memory<>).Assembly;
    }
  }
}
#endif
