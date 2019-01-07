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
  /// <summary>
  ///   An immutable, structure that exists within the world. 
  /// </summary>
  [CreateAssetMenu(menuName = "Items/Structure")]
  public class StructureDescriptor : TileBase
  {
    [Tooltip("The sprite to use when rendering the object in various 2d places")]
    public Sprite Sprite;
    
    [Tooltip("The size of the item in the world")]
    public GridBasedSize Size;

    [Tooltip("The unique id for the type of this tile")]
    public short ObjectId;

    [Tooltip("The Unity object that should be placed in the world when the object is placed")]
    public GameObject Template;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
      tileData.sprite = Sprite;
    }

    /// <summary>
    ///   Creates an instance of this placeable in the world grid, so that the various systems (physics,
    ///   graphics, etc) can interact with it.
    /// </summary>
    public GameObject CreateInstanceInWorld(IOwner owner, GridCoordinate coordinate)
    {
      var absolutePosition = coordinate.ToVector3();
      var instance = Template.CreateInstance(new PositionAndRotation(absolutePosition, Quaternion.identity));
      var behavior = instance.GetComponent<StructureBehavior>();
      behavior?.Initialize(this, owner, coordinate);
      return instance;
    }

#if UNITY_EDITOR
    public GameObject CreateInstanceViaBrush()
    {
      var instance = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(Template);
      instance.GetComponent<StructureBehavior>().Initialize(this);
      return instance;
    }
#endif
  }

  public class DestructableStructureDescriptor : StructureDescriptor
  {
    [Tooltip("The initial amount of health that the built has")]
    public int BaseHealth;
  }
}