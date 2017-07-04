using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> Base behavior for all ammunition that gets fired </summary>
  public class ProjectileBehavior : BaseBehavior
  {
    private WeaponScriptable _weaponTemplate;
    private ProjectileScriptable _projectileScriptable;

    public void Initialize(WeaponScriptable weaponTemplate, ProjectileScriptable projectile)
    {
      _weaponTemplate = weaponTemplate;
      _projectileScriptable = projectile;
    }

    public void Start()
    {
      GetComponent<Rigidbody2D>().velocity = Vector2.up * _projectileScriptable.InitialVelocity;
    }
  }
}