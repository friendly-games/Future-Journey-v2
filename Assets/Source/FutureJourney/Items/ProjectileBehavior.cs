using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> Base behavior for all ammunition that gets fired </summary>
  public class ProjectileBehavior : BaseBehavior
  {
    private WeaponScriptable _weaponTemplate;
    private ProjectileScriptable _projectileScriptable;
    private Allegiance _allegiance;

    public void Initialize(WeaponScriptable weaponTemplate, ProjectileScriptable projectile, Allegiance allegiance)
    {
      _weaponTemplate = weaponTemplate;
      _projectileScriptable = projectile;
      _allegiance = allegiance;
    }

    [UsedImplicitly]
    private void Start()
    {
      GetComponent<Rigidbody2D>().velocity = Vector2.up * _projectileScriptable.InitialVelocity;
      gameObject.layer = _allegiance.AssociatedLayer.LayerId;
    }

    [UsedImplicitly]
    private void OnCollisionEnter2D(Collision2D collision)
    {
      UnityExtensions.Destroy(collision.gameObject);
    }

    public void TryApplyDamage(GameObject instance)
    {
      
    }
  }
}