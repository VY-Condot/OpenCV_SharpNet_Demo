// This is a "magic" polyfill that tricks the .NET 4.8 compiler 
// into understanding modern C# 9 'record' types and 'init' properties.

#if NET48
namespace System.Runtime.CompilerServices
{
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}
#endif