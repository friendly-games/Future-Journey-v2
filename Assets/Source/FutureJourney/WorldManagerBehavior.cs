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
  public class WorldManagerBehavior : MonoBehaviour
  {
    public int Width;

    [Tooltip("All of the available tiles for the world generation")]
    public TileType[] AvailableTiles;

    [Tooltip("The layer to which all tiles should be added")]
    public Layer TileLayer;

    [Tooltip("The player game object")]
    public GameObject Player;

    private WorldGrid _grid;

    private WorldGridSlice<TileBehavior> _slice;

    public void Start()
    {
      _grid = new WorldGrid(AvailableTiles);

      _slice = new WorldGridSlice<TileBehavior>(_grid, 20, 20);
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
      SliceUnitData<TileBehavior> olddata,
      ref SliceUnitData<TileBehavior> newdata
    )
    {
      if (olddata.Data != null)
      {
        UnityExtensions.Destroy(olddata.Data.gameObject);
      }

      var item = newdata.GridItem;
      int x = newdata.Position.X;
      int y = newdata.Position.Y;


      var instance = AvailableTiles[item.Type].Construct(
        new PositionAndRotation(new Vector3(x, y, 0), PossibleRotations[item.Rotation]),
        _grid,
        new GridCoordinate(x, y)
      );

      newdata.Data = instance;
    }
  }
}