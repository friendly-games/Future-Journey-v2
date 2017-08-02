using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using UnityEditorInternal;

namespace NineBitByte.FutureJourney.Items
{
  public class TileBehavior : BaseBehavior, IDamageReceiver
  {
    private TileType _tileType;
    private WorldGrid _grid;

    public TileBehavior Initialize(TileType tileType, WorldGrid grid, GridCoordinate coordinate)
    {
      Health = tileType.InitialHealth;

      _grid = grid;
      _tileType = tileType;
      Coordinate = coordinate;

      return this;
    }

    public GridCoordinate Coordinate { get; private set; }

    public int Health { get; set; }

    public void OnHealthDepleted()
    {
      ChunkCoordinate chunkCoordinate;
      InnerChunkGridCoordinate innerCoordinate;

      Coordinate.Deconstruct(out chunkCoordinate, out innerCoordinate);

      var chunk = _grid[chunkCoordinate];

      chunk[innerCoordinate] = new GridItem(0, chunk[innerCoordinate].Rotation);
    }
  }

}