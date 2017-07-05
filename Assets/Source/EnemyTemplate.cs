using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source.FutureJourney.Items;
using UnityEngine;

namespace NineBitByte.Assets.Source
{
  [CreateAssetMenu(menuName = "Items/Enemy")]
  public class EnemyTemplate : BaseScriptable
  {
    [Tooltip("The team to which the enemy belongs")]
    public Allegiance Allegiance;

    public int InitialHealth;

    public int MaxHealth;

    /// <summary> The template for an enemy. </summary>
    public GameObject Template;

    public void Build(PositionAndRotation initialPosition, Allegiance allegiance)
    {
      var clone = Template.CreateInstance(initialPosition);
      clone.GetComponent<EnemyBehavior>().Construct(this, allegiance);
    }

    public void ApplyDamage(EnemyBehavior enemy, float damage)
    {
      
    }
  }
}