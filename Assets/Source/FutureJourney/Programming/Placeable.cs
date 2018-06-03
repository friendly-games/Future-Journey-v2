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
  public class Placeable : BaseActable
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

    /// <inheritdoc />
    public override void Act(PlayerBehavior playerBehavior, object instance) 
      => PlaceOnGrid(playerBehavior, new GridCoordinate(playerBehavior.ReticulePosition));

    /// <inheritdoc />
    public override GameObject Attach(PlayerBehavior actor, Transform parent, PositionAndRotation location)
    {
      return null;
    }

    /// <inheritdoc />
    public override void Detach(PlayerBehavior actor, object instance)
    {
      // no-op
    }

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

    /// <summary>
    ///   Creates an instance of this placable in the world grid, so that the various systems (physics,
    ///   graphics, etc) can interact with it.
    /// </summary>
    public PlaceableBehavior CreateInstanceInWorld(IOwner owner, GridCoordinate coordinate)
    {
      var absolutePosition = coordinate.ToVector3();
      
      var instance = Template.CreateInstance(new PositionAndRotation(absolutePosition, Quaternion.identity));
      var placeable = instance.GetComponent<PlaceableBehavior>();

      placeable.Initialize(this, owner, coordinate);

      return placeable;
    }
  }
}