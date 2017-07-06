using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.Items
{
  public class EnemyBehavior : BaseBehavior, IHealth
  {
    private Creature _template;
    private Allegiance _allegiance;

    public void Construct(Creature template, Allegiance allegiance)
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