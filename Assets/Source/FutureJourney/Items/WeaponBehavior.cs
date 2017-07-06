using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Behavior for an instance of weapon. </summary>
  public class WeaponBehavior : BaseBehavior
  {
    [Tooltip("The location at which the projectile is fired from the location of the owner GameObject")]
    public RelativeOffset MuzzleOffset;
  }
}