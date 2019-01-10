using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary>
  ///   A type of tile that makes up the ground.
  /// </summary>
  [CreateAssetMenu(menuName = "Items/Tile")]
  public class TileDescriptor : TileBase
  {
    [Tooltip("The sprite to use when rendering the object in various 2d places")]
    public Sprite Sprite;
    
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
      tileData.sprite = Sprite;
    }
  }
}