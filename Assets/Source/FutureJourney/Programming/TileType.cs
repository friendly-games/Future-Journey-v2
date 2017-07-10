using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  public class TileType : BaseScriptable
  {
    [Tooltip("The name of the tile type")]
    public string Name;

    [Tooltip("The unity object to create copies of when creating the tile")]
    public GameObject Template;

    public GameObject Construct(PositionAndRotation location)
    {
      var clone = Template.CreateInstance(location);
      clone.GetComponent<TileBehavior>().Initialize(this);
      return clone;
    }
  }
}