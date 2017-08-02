using System;
using System.Linq;
using System.Collections.Generic;

namespace NineBitByte.Common.Util
{
  internal static class MathUtil
  {
    /// <summary> Gets the of the value, always ensuring the value is positive. </summary>
    /// <param name="value"> The value to take the remainder of.. </param>
    /// <param name="divisor"> The divisor. </param>
    /// <returns> The positive modulus of the value. </returns>
    public static int PositiveRemainder(int value, int divisor)
    {
      return ((value % divisor) + divisor) % divisor;
    }

    public static void Swap<T>(ref T leftValue, ref T rightValue)
    {
      var temp = leftValue;
      leftValue = rightValue;
      rightValue = temp;
    }
  }
}