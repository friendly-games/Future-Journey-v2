using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
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

    [Tooltip("The amount of time it takes to reload this weapon")]
    [TimeField(TimeSpecifiedIn.Seconds, Minimum = 0, Maximum = 2)]
    public TimeField TimeToReload;
    
    [Tooltip("The location at which the projectile is fired from the location of the owner GameObject")]
    public RelativeOffset MuzzleOffset;

    [Tooltip("The location where the user should hold the weapon")]
    public RelativeOffset HeldPosition;

    /// <summary> Creates a projectile for the given weapon. </summary>
    public override void Act(WeaponBehavior weaponInstance, Allegiance allegiance)
    {
      for (int i = 0; i < NumberOfPellets; i++)
      {
        var projectileInstance = Projectile.ProjectileTemplate.CreateInstance(
          weaponInstance.MuzzleOffset.ToLocation(weaponInstance.transform)
        );

        var randomAngle = UnityEngine.Random.Range(-15, (float)15) * Spread;
        projectileInstance.transform.rotation *= Quaternion.Euler(0, 0, randomAngle);

        var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
        behavior.Initialize(this, Projectile, allegiance);
      }
    }

    public WeaponBehavior Attach(Transform parent, PositionAndRotation initialLocation)
    {
      var instance = WeaponTemplate.CreateInstance(
        parent,
        initialLocation
      );

      var weaponBehavior = instance
        .GetComponent<WeaponBehavior>()
        .Initialize(this);

      instance.transform.localPosition -= weaponBehavior.HeldPosition.Offset;
      return weaponBehavior;
    }
  }

  /// <summary>
  ///   Interface used for Scriptables that need to be initialized on startup.
  /// </summary>
  public interface IScriptableInitialable
  {
    void Intialize();
  }
}