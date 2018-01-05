using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NineBitByte.Common.Util;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> An unmanaged array of GridItems. Does not provide bounds checking. </summary>
  internal unsafe class UnsafeGridItemArray : IDisposable
  {
    private static readonly int SizeOfGridItem = Marshal.SizeOf<GridItem>();

    private readonly SafeUnmanagedMemoryHandle _dataArray;
    private readonly GridItem* _root;

    /// <summary> Constructor. </summary>
    /// <param name="size"> The size of the array. </param>
    public UnsafeGridItemArray(int size)
    {
      var numberOfBytes = size * SizeOfGridItem;

      _dataArray = new SafeUnmanagedMemoryHandle(numberOfBytes);
      _root = (GridItem*)_dataArray.Handle;
    }

    /// <summary> Gets the element at the given index, without bounds checking. </summary>
    public GridItem* this[int index] 
      => _root + index;

    /// <inheritdoc />
    public void Dispose()
    {
      _dataArray?.Dispose();
    }
  }
}