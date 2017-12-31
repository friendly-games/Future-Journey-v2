using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> A behavior for a Placable where the object simply acts as a barrier </summary>
  public class SimpleBarrierBehavior : PlaceableBehavior, IDamageReceiver
  {
    private Chunk _chunk;
    private InnerChunkGridCoordinate _coordinate;
    private WorldGrid _grid;

    public int Health
    {
      get { return _chunk[_coordinate].Health; }
      set { _chunk.UpdateHealth(_coordinate, value); }
    }

    public override void Initialize(Buildable programming, IOwner owner, GridCoordinate coordinate)
    {
      base.Initialize(programming, owner, coordinate);

      ChunkCoordinate chunkCoordinate;
      coordinate.Deconstruct(out chunkCoordinate, out _coordinate);

      _grid = owner.AssociatedGrid;
      _chunk = _grid[chunkCoordinate];
    }

    /// <inheritdoc />
    public void OnHealthDepleted()
    {
      var item = _chunk[_coordinate];
      _chunk[_coordinate] = item.WithoutObject();
    }
  }
}