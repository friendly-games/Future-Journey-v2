using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common
{
  internal static class Utils
  {
    /// <summary>
    ///  Get a random number between <paramref name="minimum"/> and <paramref name="maximum"/>
    /// </summary>
    public static float RandomBetween(float minimum, float maximum) 
      => UnityEngine.Random.value * (maximum - minimum) + minimum;
  }
}