using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Information about a single world tile stored in the world-space. </summary>
  public struct TileInformation
  {
    public TileInformation(TileType type, byte rotation)
    {
      Type = type;
      Rotation = rotation;
    }

    public TileType Type { get; }

    // 0-3 for all possible rotations
    public byte Rotation { get; }
  }
}