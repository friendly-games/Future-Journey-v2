using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary>
  ///   Contains a snapshot of part of a map, which can be imported into the world if need be.
  /// </summary>
  [CreateAssetMenu(menuName = "Items/World Layout")]
  public class WorldLayout : BaseScriptable
  {
    [Tooltip("The size of the layout")]
    public GridBasedSize Size;

    [Tooltip("The tiles within the world")]
    public GridItem[] GridItems;

    public bool IsValidPosition(Vector3Int position)
    {
      return position.x >= 0
             && position.x < Size.Width
             && position.y >= 0
             && position.y < Size.Width;
    }
  }
}