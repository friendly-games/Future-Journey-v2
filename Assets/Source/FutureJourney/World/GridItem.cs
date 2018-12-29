using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable ConvertToAutoProperty

namespace NineBitByte.FutureJourney.World
{
  /// <summary> A single cell of data that is stored in the world grid. </summary>
  public struct GridItem
  {
    private readonly short _tileType;
    private readonly short _structureType;
    private readonly byte _rotation;
    private int _health;

    public bool IsSimpleTile
      => TileType != short.MaxValue;

    public GridItem(short tileType, ItemRotation tileRotation)
      : this(tileType, tileRotation, 0, ItemRotation.None, 0)
    {
    }

    private GridItem(short tileType, ItemRotation tileRotation, short structureType, ItemRotation objectRotation, int health)
    {
      _tileType = tileType;
      _structureType = structureType;
      _health = health;

      _rotation = (byte)((byte)tileRotation | ((byte)objectRotation) << 4);
    }

    /// <summary> Creates a GridItem that no longer has any object associated with it. </summary>
    public GridItem WithoutObject()
      => new GridItem(_tileType, TileRotation);

    /// <summary> Creates a new GridItem that represents a tile with the given object added to it. </summary>
    public GridItem AddStructure(short structureType, ItemRotation objectRotation, int health) 
      => new GridItem(_tileType, TileRotation, structureType, objectRotation, health);

    /// <summary>
    ///  Gets the type of the tile at this location.  When <see cref="StructureType"/> is not set, the
    ///  tile is directly in the world.  When <see cref="StructureType"/> is set, the object rests on top
    ///  of this tile.
    /// </summary>
    public short TileType
      => _tileType;

    /// <summary>
    ///  The type of object stored on-top of the tile.  If 0, then only the tile is shown.
    /// </summary>
    public short StructureType
      => _structureType;

    /// <summary> 0-3 for all possible rotations.  </summary>
    public ItemRotation TileRotation
      => (ItemRotation)(_rotation & 0x0F);

    public ItemRotation StructureRotation
      => (ItemRotation)(_rotation >> 4);

    /// <summary> Contains the health for the item stored in this tile. </summary>
    public int Health
    {
      get { return _health; }
      set { _health = value; }
    }
  }
}