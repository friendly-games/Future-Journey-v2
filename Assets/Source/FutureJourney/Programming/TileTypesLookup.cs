using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> Associates tile indices with the associated unity objects. </summary>
  [CreateAssetMenu(menuName = "Items/Tile Mapping")]
  public class TileTypesLookup : BaseScriptable
  {
    [Tooltip("All of the available tiles for the world generation")]
    public TileType[] AvailableTiles;

    [FormerlySerializedAs("Placeables")]
    [Tooltip("All of the available buildables in the world")]
    [FormerlySerializedAs("Buildables")]
    public PlaceableDescriptor[] PlaceablesDescriptor;

    [Tooltip("The layer to which all tiles should be added")]
    public Layer TileLayer;

    public PlaceableDescriptor FindPlaceable(short id)
    {
      foreach (var b in PlaceablesDescriptor)
      {
        if (b.ObjectId == id)
          return b;
      }

      return null;

      //throw new ArgumentException($"No Placeable with id of {id}");
    }
  }
}