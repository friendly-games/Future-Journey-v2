using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common.Util;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> An enum that indicates the type of change that occurred. </summary>
  public enum GridItemPropertyChange
  {
    All,
    ObjectChange,
    HealthChange,
  }

  /// <summary> Extensions that modify a <see cref="GridItem"/> within a chunk. </summary>
  public static class ChunkExtensions
  {
    /// <summary> Updates the health of a given <see cref="GridItem"/> </summary>
    public static void UpdateHealth(this Chunk chunk, InnerChunkGridCoordinate coordinate, int health)
      => chunk.UpdateItem(coordinate, (ref GridItem it, int value) => it.Health = value, GridItemPropertyChange.HealthChange, health);
  }

}