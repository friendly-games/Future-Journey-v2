using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  public class TileType : BaseScriptable
  {
    [Tooltip("The name of the tile type")]
    public string Name;

    [Tooltip("The unity object to create copies of when creating the tile")]
    public GameObject Template;

    [Tooltip("The amount of health that the tile has.  Zero if the tile cannot be destroyed")]
    public int InitialHealth;

    [Tooltip("The type to morph into if the tile is destroyed")]
    public TileType MorphType;

    public TileBehavior Construct(
      PositionAndRotation location,
      WorldGrid grid,
      GridCoordinate gridCoordinate)
    {
      return Template
        .CreateInstance(location)
        .GetComponent<TileBehavior>()
        .Initialize(this, grid, gridCoordinate);
    }
  }
}