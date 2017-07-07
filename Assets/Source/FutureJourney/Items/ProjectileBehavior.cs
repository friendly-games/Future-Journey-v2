using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Base behavior for all ammunition that gets fired </summary>
  public class ProjectileBehavior : BaseBehavior
  {
    private ProjectileWeapon _weaponTemplate;
    private Projectile _projectile;
    private Allegiance _allegiance;

    public void Initialize(ProjectileWeapon weaponTemplate, Projectile projectile, Allegiance allegiance)
    {
      _weaponTemplate = weaponTemplate;
      _projectile = projectile;
      _allegiance = allegiance;
    }

    [UsedImplicitly]
    private void Start()
    {
      GetComponent<Rigidbody2D>().velocity = transform.up * _projectile.InitialVelocity;
      gameObject.layer = _allegiance.AssociatedLayer.LayerId;
    }

    [UsedImplicitly]
    private void OnCollisionEnter2D(Collision2D collision)
    {
      var receiver = collision.gameObject.GetComponent<IDamageReceiver>();

      if (receiver != null)
      {
        DamageProcessor.ApplyDamage(receiver, 100);
      }

      UnityExtensions.Destroy(gameObject);
    }
  }
}