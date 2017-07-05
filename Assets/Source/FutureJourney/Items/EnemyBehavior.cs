using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class EnemyBehavior : BaseBehavior, IHealth
  {
    private EnemyTemplate _template;
    private Allegiance _allegiance;

    public void Construct(EnemyTemplate template, Allegiance allegiance)
    {
      _template = template;
      _allegiance = allegiance;

      Health = _template.InitialHealth;
    }

    /// <inheritdoc />
    public float Health { get; set; }

    public void Start()
    {
      gameObject.layer = _allegiance.AssociatedLayer.LayerId;
    }
  }
}