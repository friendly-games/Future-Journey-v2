using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.World;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NineBitByte.FutureJourney.Programming
{
  public class TileType : TileBase
  {
    [Tooltip("The name of the tile type")]
    public string Name;

    [Tooltip("The sprite template to use when the item is inventory")]
    public Sprite Sprite;
    
    [Tooltip("The unique id for the type of this tile")]
    public short Id;

    [Tooltip("The unity object to create copies of when creating the tile")]
    public GameObject Template;

    public TileBehavior Construct(
      PositionAndRotation location,
      WorldGrid grid,
      GridCoordinate gridCoordinate)
    {
      var behavior =  Template.CreateInstance(location)
                              .GetComponent<TileBehavior>();
      return behavior;
    }

    /// <inheritdoc />
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
      tileData.sprite = Sprite;
    }

    public GridItem CreateGridItem()
      => new GridItem(Id, ItemRotation.None);
  }
}