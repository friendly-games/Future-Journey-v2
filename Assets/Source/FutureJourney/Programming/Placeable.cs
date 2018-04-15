using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> An item that can be built in the world. </summary>
  [CreateAssetMenu(menuName = "Items/Placeable")]
  public class Placeable : BaseScriptable
  {
    [Tooltip("The size of the item in the world")]
    public GridBasedSize Size;

    [Tooltip("The unique id for the type of this tile")]
    public short ObjectId;

    [Tooltip("The Unity object that should be placed in the world when the object is placed")]
    public GameObject Template;

    [Tooltip("The sprite template to use when the item is inventory")]
    public Sprite Sprite;

    [Tooltip("The initial amount of health that the built has")]
    public int InitialHealth;

    public void PlaceOnGrid(IOwner owner, GridCoordinate coordinate)
    {
      var grid = owner.AssociatedGrid;

      InnerChunkGridCoordinate innerCoordinate;
      ChunkCoordinate chunkCoordinate;
      coordinate.Deconstruct(out chunkCoordinate, out innerCoordinate);

      var chunk = grid[chunkCoordinate];
      var oldItem = chunk[innerCoordinate];
      var newItem = oldItem.AddObject(ObjectId, oldItem.TileRotation, InitialHealth);

      chunk[innerCoordinate] = newItem;
    }

    public PlaceableBehavior CreateInstance(IOwner owner, GridCoordinate coordinate)
    {
      var instance = Template.CreateInstance(new PositionAndRotation(coordinate.ToVector3(), Quaternion.identity));
      var placeable = instance.GetComponent<PlaceableBehavior>();

      placeable.Initialize(this, owner, coordinate);

      return placeable;
    }
  }
}