using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.Items
{
  public class AllScriptableObjectsBehavior : BaseBehavior
  {
    [DisplayScriptableObjectProperties]
    public ProjectileWeapon Weapon1;

    [DisplayScriptableObjectProperties]
    public ProjectileWeapon Weapon2;
  }
}