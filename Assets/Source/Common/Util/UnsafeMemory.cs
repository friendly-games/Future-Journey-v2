using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace NineBitByte.Common.Util
{
  /// <summary>
  ///  Holds a pointer to un-managed memory allocated via <see cref="Marshal.AllocHGlobal(int)"/>
  /// </summary>
  public sealed class SafeUnmanagedMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public SafeUnmanagedMemoryHandle(int numberOfBytes, InitializationType initializationType = InitializationType.ZeroOutMemory)
      : base(true)
    {
      SetHandle(Marshal.AllocHGlobal(numberOfBytes));

      if (initializationType == InitializationType.ZeroOutMemory)
      {
        unsafe
        {
          UnsafeUtils.ClearMemory((byte*)handle, numberOfBytes);
        }
      }
    }

    /// <summary> The handle to the data. </summary>
    public IntPtr Handle 
      => handle;

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
      if (Handle == IntPtr.Zero)
        return false;

      Marshal.FreeHGlobal(handle);
      handle = IntPtr.Zero;

      return true;
    }

    public enum InitializationType
    {
      ZeroOutMemory,
      None
    }
  }
}
