using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  [CreateAssetMenu(menuName = "Items/Projectile")]
  public class ProjectileScriptable : BaseScriptable
  {
    [Tooltip("The amount of damage that one pellet does to a target")]
    public int BaseDamage;

    [Range(5, 10000)]
    [Tooltip("The speed at which the projectile is projected")]
    public float InitialVelocity;

    [Tooltip("The Prefab associated with the Projectile")]
    public GameObject ProjectileTemplate;
  }
}