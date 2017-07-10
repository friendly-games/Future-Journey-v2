using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.Items
{
  public class EnemyBehavior : BaseBehavior, IDamageReceiver
  {
    private Creature _template;
    private Allegiance _allegiance;

    public void Initialize(Creature template, Allegiance allegiance)
    {
      _template = template;
      _allegiance = allegiance;

      Health = _template.InitialHealth;
    }

    /// <inheritdoc />
    public int Health { get; set; }

    public void Start()
    {
      gameObject.layer = _allegiance.AssociatedLayer.LayerId;
    }

    public void OnHealthDepleted()
    {
      UnityExtensions.Destroy(gameObject);
    }
  }
}