using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  [CreateAssetMenu(menuName = "Items/Enemy")]
  public class Creature : BaseScriptable
  {
    [Tooltip("The team to which the creature belongs")]
    public Allegiance Allegiance;

    public int InitialHealth;

    public int MaxHealth;

    public GameObject Template;

    public GameObject Construct(PositionAndRotation initialPosition, Allegiance allegiance)
    {
      var clone = Template.CreateInstance(initialPosition);
      clone.GetComponent<EnemyBehavior>().Initialize(this, allegiance);
      return clone;
    }

    public void ApplyDamage(EnemyBehavior enemy, float damage)
    {
      
    }
  }
}