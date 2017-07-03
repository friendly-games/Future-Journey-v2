using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Assets.Source
{
  public abstract class Damagable : BaseScriptable
  {
    public abstract void Damage(IHealth damageable);

    public interface IHealth
    {
      float Health { get; set;  }
      float MaxHealth { get; }
    }
  }

  public class ExplosiveDamageable : Damagable
  {
    public float Radius = 1.0f;
    public float TotalDamage = 100.0f;

    public override void Damage(IHealth damageable) 
      => damageable.Health -= 10f;
  }
}