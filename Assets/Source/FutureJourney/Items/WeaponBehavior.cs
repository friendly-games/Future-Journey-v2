using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Behavior for an instance of weapon. </summary>
  public class WeaponBehavior : BaseBehavior
  {
    [Tooltip("The location at which the projectile is fired from the location of the owner GameObject")]
    public RelativeOffset MuzzleOffset;

    [Tooltip("The location where the user should hold the weapon")]
    public RelativeOffset HeldPosition;

    public ProjectileWeapon Programming { get; private set; }

    public WeaponBehavior Initialize(ProjectileWeapon projectileWeapon)
    {
      Programming = projectileWeapon;
      return this;
    }
  }
}