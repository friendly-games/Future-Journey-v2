using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> Base class for weapons. </summary>
  public abstract class BaseWeaponScriptable<TBehavior> : BaseScriptable
    where TBehavior : BaseBehavior
  {
    /// <summary> Performs the weapon's action. </summary>
    public abstract void Act(TBehavior behavior);
  }
}