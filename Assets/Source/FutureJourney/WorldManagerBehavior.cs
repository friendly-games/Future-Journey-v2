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
      SliceUnitData<UnityWorldData> olddata,
      ref SliceUnitData<UnityWorldData> newdata,
      GridItemPropertyChange changeType
    )
    {
      // TODO should we do something?
      if (changeType == GridItemPropertyChange.Health)
      {
        newdata.Data = olddata.Data;
        return;
      }

      if (olddata.Data?.Placeable != null)
      {
        UnityExtensions.Destroy(olddata.Data.Placeable.gameObject);
      }

      if (olddata.Data?.Tile != null)
      {
        UnityExtensions.Destroy(olddata.Data.Tile.gameObject);
      }

      var item = newdata.GridItem;

      var worldPosition = newdata.Position.ToVector3();

      var tileInstance = TileLookup.AvailableTiles[item.TileType].Construct(
        new PositionAndRotation(worldPosition, PossibleRotations[(byte)item.TileRotation]),
        WorldGrid,
        newdata.Position
      );

      PlaceableBehavior placeableInstance = null;

      if (item.ObjectType != 0)
      {
        var associatedBuildable = TileLookup.FindPlaceable(item.ObjectType);
        if (associatedBuildable == null)
        {
          Debug.Log($"No object type of «{item.ObjectType}» found while building world");
        }
        else
        {
          placeableInstance = associatedBuildable.CreateInstance(this, newdata.Position);
        }
      }

      newdata.Data = new UnityWorldData
                     {
                       Tile = tileInstance,
                       Placeable = placeableInstance
                     };
    }

    Allegiance IOwner.Allegiance 
      => null;

    WorldGrid IOwner.AssociatedGrid
      => WorldGrid;

    private class UnityWorldData
    {
      public TileBehavior Tile;
      public PlaceableBehavior Placeable;
    }
  }
}