using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> Base class for weapons. </summary>
  public abstract class BaseWeaponScriptable<TBehavior> : BaseScriptable
    where TBehavior : BaseBehavior
  {
    /// <summary> Performs the weapon's action. </summary>
    public abstract void Act(WeaponBehavior weaponInstance, Allegiance allegiance);
  }
}