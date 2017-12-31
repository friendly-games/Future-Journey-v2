using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  public class TileType : BaseScriptable
  {
    [Tooltip("The name of the tile type")]
    public string Name;

    [Tooltip("The unique id for the type of this tile")]
    public short Id;

    [Tooltip("The unity object to create copies of when creating the tile")]
    public GameObject Template;

    public TileBehavior Construct(
      PositionAndRotation location,
      WorldGrid grid,
      GridCoordinate gridCoordinate)
    {
      return Template
        .CreateInstance(location)
        .GetComponent<TileBehavior>();
    }
  }
}