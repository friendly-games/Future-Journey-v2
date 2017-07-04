using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class EnemyBehavior : BaseBehavior
  {
    [Tooltip("The team to which the enemy belongs")]
    public Allegiance Allegiance;

    public void Start()
    {
      gameObject.layer = Allegiance.AssociatedLayer.LayerId;
    }
  }
}