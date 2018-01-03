using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.View;

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

    /// <summary />
    public void Start()
    {
      gameObject.layer = _allegiance.AssociatedLayer.LayerId;

      var healthBar = FindObjectOfType<UiManagerBehavior>().CreateHealthBar();

      healthBar.Initialize(gameObject, this);
    }

    public void OnHealthDepleted()
    {
      // TODO make the enemies exist elsewhere
      UnityExtensions.Destroy(gameObject);
    }

    protected void FixedUpdate()
    {
      ResetOverallRotation();
    }
  }
}