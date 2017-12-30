using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> Associates tile indices with the associated unity objects. </summary>
  [CreateAssetMenu(menuName = "Items/Tile Mapping")]
  public class TileTypesLookup : BaseScriptable
  {
    [Tooltip("All of the available tiles for the world generation")]
    public TileType[] AvailableTiles;

    [Tooltip("The layer to which all tiles should be added")]
    public Layer TileLayer;
  }
}