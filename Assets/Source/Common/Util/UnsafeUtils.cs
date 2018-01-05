using System;

namespace NineBitByte.Common.Util
{
  public static unsafe class UnsafeUtils
  {
    private const int EmptyDataSize = 1024;
    private static readonly byte[] EmptyData = new byte[EmptyDataSize];

    public static void ClearMemory(byte* data, int size)
    {
      fixed (byte* empty = EmptyData)
      {
        for (int i = 0; i < size; i += EmptyDataSize)
        {
          int numBytesToCopy = Math.Min(EmptyDataSize, size - i);
          Buffer.MemoryCopy(empty, data + i, numBytesToCopy, numBytesToCopy);
        }
      }
    }
  }
}