using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney
{
  public class WorldManagerBehavior : MonoBehaviour, IOwner
  {
    public int Width;

    public TileTypesLookup TileLookup;

    [Tooltip("The layer to which all tiles should be added")]
    public Layer TileLayer;

    [Tooltip("The player game object")]
    public GameObject Player;

    private WorldGridSlice<UnityWorldData> _slice;

    public WorldGrid WorldGrid { get; private set; }

    public void Start()
    {
      WorldGrid = new WorldGrid();

      _slice = new WorldGridSlice<UnityWorldData>(WorldGrid, 20, 20);
      _slice.DataChanged += HandleDataChanged;

      _slice.Initialize(new GridCoordinate(Player.transform.position));
    }

    public void Update()
    {
      _slice.Recenter(new GridCoordinate(Player.transform.position));
    }

    private static readonly Quaternion[] PossibleRotations =
      new Quaternion[4]
      {
        Quaternion.AngleAxis(0, Vector3.back),
        Quaternion.AngleAxis(90, Vector3.back),
        Quaternion.AngleAxis(180, Vector3.back),
        Quaternion.AngleAxis(270, Vector3.back),
      };

    private void HandleDataChanged(
      SliceUnitData<UnityWorldData> oldData,
      ref SliceUnitData<UnityWorldData> newData,
      GridItemPropertyChange changeType
    )
    {
      // TODO should we do something?
      if (changeType == GridItemPropertyChange.HealthChange)
      {
        newData.Data = oldData.Data;
        return;
      }

      if (oldData.Data?.Structure != null)
      {
        UnityExtensions.Destroy(oldData.Data.Structure);
      }

      if (oldData.Data?.Tile != null)
      {
        UnityExtensions.Destroy(oldData.Data.Tile.gameObject);
      }

      var item = newData.GridItem;

      var worldPosition = newData.Position.ToVector3();

      var tileInstance = TileLookup.AvailableTiles[item.TileType].Construct(
        new PositionAndRotation(worldPosition, PossibleRotations[(byte)item.TileRotation]),
        WorldGrid,
        newData.Position
      );

      GameObject placeableInstance = null;

      if (item.StructureType != 0)
      {
        Debug.Log($"Looked for structure with id of {item.StructureType}");
        
        var associatedStructure = TileLookup.FindStructureOrNull(item.StructureType);
        if (associatedStructure == null)
        {
          Debug.Log($"No object type of «{item.StructureType}» found while building world");
        }
        else
        {
          placeableInstance = associatedStructure.CreateInstanceInWorld(this, newData.Position);
        }
      }

      newData.Data = new UnityWorldData
                     {
                       Tile = tileInstance,
                       Structure = placeableInstance
                     };
    }

    Allegiance IOwner.Allegiance 
      => null;

    WorldGrid IOwner.AssociatedGrid
      => WorldGrid;

    // TODO remove
    public Vector3 ReticulePosition { get; set; }

    private class UnityWorldData
    {
      public TileBehavior Tile;
      public GameObject Structure;
    }
  }
}