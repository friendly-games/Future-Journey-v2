using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source_Project;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> Behavior for an instance of weapon. </summary>
  public class WeaponBehavior : BaseBehavior
  {
    [Tooltip("The location at which the projectile is fired from the location of the owner GameObject")]
    [RelativeOffset]
    public Vector3 MuzzleOffset;
  }
}