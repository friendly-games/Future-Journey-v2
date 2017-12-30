using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  [CreateAssetMenu(menuName = "Items/Wall")]
  public class Wall : BaseScriptable
  {
    [Tooltip("The Unity object that should be placed in the world when the wall is placed")]
    public GameObject Template;

    [Tooltip("The sprite template to use when the item is inventory")]
    public Sprite Sprite;

    public void PlaceInWorld(PositionAndRotation location, Allegiance allegiance)
    {
    }
  }
}