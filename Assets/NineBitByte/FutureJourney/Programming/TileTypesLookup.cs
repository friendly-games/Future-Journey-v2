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

    [Tooltip("All of the available buildables in the world")]
    [FormerlySerializedAs("Buildables")]
    [FormerlySerializedAs("PlaceablesDescriptor")]
    public PlaceableDescriptor[] Placeables;

    [Tooltip("All of the known structures available in the world")]
    public StructureDescriptor[] Structures;

    [Tooltip("The layer to which all tiles should be added")]
    public Layer TileLayer;

    public StructureDescriptor FindStructureOrNull(short id)
    {
      foreach (var structure in Structures)
      {
        if (structure.ObjectId == id)
          return structure;
      }

      return null;
    }
    
    public TileType FindTileOrNull(short id)
    {
      foreach (var tile in AvailableTiles)
      {
        if (tile.Id == id)
          return tile;
      }

      return null;
    }
  }
}