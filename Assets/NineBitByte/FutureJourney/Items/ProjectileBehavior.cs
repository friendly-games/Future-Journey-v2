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
    private ProjectileWeaponDescriptor _weaponDescriptorTemplate;
    private ProjectileDescriptor _projectileDescriptor;
    private Allegiance _allegiance;

    public void Initialize(ProjectileWeaponDescriptor weaponDescriptorTemplate, ProjectileDescriptor projectileDescriptor, Allegiance allegiance)
    {
      _weaponDescriptorTemplate = weaponDescriptorTemplate;
      _projectileDescriptor = projectileDescriptor;
      _allegiance = allegiance;
    }

    [UsedImplicitly]
    private void Start()
    {
      GetComponent<Rigidbody2D>().velocity = transform.up * _projectileDescriptor.InitialVelocity;
      // TODO this layer to be automatic
      gameObject.layer = Layer.FromName("Projectile").LayerId;

      // don't go more than 300 units before disappearing (or 1 second)
      // TODO should this be specified in the template
      float timeToLive = 30 / _projectileDescriptor.InitialVelocity;

      Destroy(gameObject, timeToLive);
    }

    [UsedImplicitly]
    private void OnCollisionEnter2D(Collision2D collision)
    {
      // TODO make it so that projectiles from our team don't damage ourselves
      var receiver = collision.gameObject.GetComponent<IDamageReceiver>();

      if (receiver != null)
      {
        DamageProcessor.ApplyDamage(receiver, _weaponDescriptorTemplate.DamagePerShot);
      }

      UnityExtensions.Destroy(gameObject);
    }
  }
}