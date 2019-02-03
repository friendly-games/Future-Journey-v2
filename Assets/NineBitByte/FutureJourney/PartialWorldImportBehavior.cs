using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney
{
  public class PartialWorldImportBehavior : BaseBehavior
  {
    [Tooltip("The layout to import")]
    public WorldLayout Layout;
    
    [Tooltip("The position at which the partial world should be imported")]
    public GridCoordinate Position;
    
    public void Start()
    {
      var grid = GetComponent<WorldManagerBehavior>().WorldGrid;
      Layout.ImportIntoWorld(grid, Position);
    }
  }
}