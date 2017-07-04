using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> All properties for a weapon that can be fired. </summary>
  [CreateAssetMenu(menuName = "Items/Weapon")]
  public class WeaponScriptable : BaseScriptable
  {
    public string Name;

    public int DamagePerShot = 10;

    [Range(1, 20)]
    public int NumberOfPelots = 1;

    [Range(0, 1)]
    public float Spread = .75f;

    [Range(1, 150)]
    public int ClipSize = 1;

    public GameObject WeaponTemplate;

    public ProjectileScriptable Projectile;

    /// <summary> Creates a projectile for the given weapon. </summary>
    public void Fire(WeaponBehavior weaponInstance)
    {
      var projectileInstance = Projectile.ProjectileTemplate.CreateInstance(
        weaponInstance.transform.position + weaponInstance.MuzzleOffset,
        weaponInstance.transform.rotation
        );

      var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
      behavior.Initialize(this, Projectile);
    }
  }
}