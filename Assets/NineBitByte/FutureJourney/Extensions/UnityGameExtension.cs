using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Extensions
{
  /// <summary>
  ///  Extensions specific to Unity and FutureJourney
  /// </summary>
  public static class UnityGameExtension
  {
    /// <summary>
    ///   Convert the given int3 into a GridCoordinate
    /// </summary>
    public static GridCoordinate ToGrid(this Vector3Int it)
      => new GridCoordinate(it.x, it.y);
  }
}