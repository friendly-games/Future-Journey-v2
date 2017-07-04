using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> All properties for a weapon that can be fired. </summary>
  [CreateAssetMenu(menuName = "Items/Weapon")]
  public class WeaponScriptable : BaseWeaponScriptable<WeaponBehavior>
  {
    [Tooltip("How much damage is done per pellet that hits")]
    public int DamagePerShot = 10;

    [Range(1, 20)]
    [Tooltip("The number of pellets that are fired from this weapon")]
    public int NumberOfPellets = 1;

    [Range(0, 1)]
    [Tooltip("How far apart each pellet can vary from muzzle")]
    public float Spread = .75f;

    [Range(1, 150)]
    [Tooltip("The number of shots in each magazine of the weapon")]
    public int ClipSize = 1;

    [Tooltip("The Prefab to use when creating an instance of the weapon in unity")]
    public GameObject WeaponTemplate;

    [Tooltip("Projectile information")]
    public ProjectileScriptable Projectile;

    /// <summary> Creates a projectile for the given weapon. </summary>
    public override void Act(WeaponBehavior weaponInstance)
    {
      var projectileInstance = Projectile.ProjectileTemplate.CreateInstance(
        weaponInstance.transform.position + weaponInstance.MuzzleOffset,
        weaponInstance.transform.rotation
        );

      var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
      behavior.Initialize(this, Projectile);
    }

    public void Attach(ref Ownership<WeaponScriptable, WeaponBehavior> owner, Vector3 offset)
    {
      var instance = WeaponTemplate.CreateInstance(
        owner.Owner.transform, 
        offset + owner.Owner.transform.position, 
        Quaternion.identity
      );

      owner.Assign(this, instance);
    }
  }
}