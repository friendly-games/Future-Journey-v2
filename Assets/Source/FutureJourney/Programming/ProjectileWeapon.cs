using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using UnityEngine;
using Random = System.Random;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> All properties for a weapon that can be fired. </summary>
  [CreateAssetMenu(menuName = "Items/Weapon")]
  public class ProjectileWeapon : BaseWeaponScriptable<WeaponBehavior>
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
    public Projectile Projectile;


    /// <summary> Creates a projectile for the given weapon. </summary>
    public override void Act(WeaponBehavior weaponInstance, Allegiance allegiance)
    {
      for (int i = 0; i < NumberOfPellets; i++)
      {
        var projectileInstance = Projectile.ProjectileTemplate.CreateInstance(
          weaponInstance.MuzzleOffset.ToLocation(weaponInstance.transform)
        );

        var randomAngle = RandomBetween(-15, 15) * Spread;
        projectileInstance.transform.rotation *= Quaternion.Euler(0, 0, randomAngle);

        var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
        behavior.Initialize(this, Projectile, allegiance);
      }
    }

    private float RandomBetween(float minimum, float maximum)
    {
      return UnityEngine.Random.value * (maximum - minimum) + minimum;
    }

    public void Attach(ref Ownership<ProjectileWeapon, WeaponBehavior> owner, PositionAndRotation initialLocation)
    {
      var instance = WeaponTemplate.CreateInstance(
        owner.Owner.transform,
        initialLocation
        );

      owner.Assign(this, instance);
    }
  }
}