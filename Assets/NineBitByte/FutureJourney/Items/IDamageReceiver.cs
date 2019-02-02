using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary>
  ///  When damage is going to be applied, this behavior receives the damage and processes.
  /// </summary>
  public interface IDamageReceiver
  {
    /// <summary> The amount of health the item contains. </summary>
    int Health { get; set; }

    /// <summary> Method to be invoked when <see cref="Health"/> falls below zero. </summary>
    void OnHealthDepleted();
  }

  /// <summary> Processes damage for various entities. </summary>
  public static class DamageProcessor
  {
    /// <summary> Applies damage to an object that can receive it. </summary>
    public static int ApplyDamage(IDamageReceiver receiver, int damageAmount)
    {
      int newHealthAmount = Math.Max(0, receiver.Health - damageAmount);
      int damageDone = receiver.Health - newHealthAmount;
      
      receiver.Health = newHealthAmount;
      if (receiver.Health <= 0)
      {
        receiver.OnHealthDepleted();
      }

      return damageDone;
    }
  }
}