using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> All properties for a weapon that can be fired. </summary>
  [CreateAssetMenu(menuName = "Items/Weapon")]
  public class ProjectileWeapon : BaseUsable
  {
    private GameObject _localTemplateCopy;

    [Range(1, 150)]
    [Tooltip("The number of shots in each magazine of the weapon")]
    public int ClipSize = 1;

    [Tooltip("How much damage is done per pellet that hits")]
    public int DamagePerShot = 10;

    [Tooltip("The location where the user should hold the weapon")]
    public RelativeOffset HeldPosition;

    [Tooltip("The location at which the projectile is fired from the location of the owner GameObject")]
    public RelativeOffset MuzzleOffset;

    [Range(1, 20)]
    [Tooltip("The number of pellets that are fired from this weapon")]
    public int NumberOfPellets = 1;

    [Tooltip("Projectile information")]
    public Projectile Projectile;

    [Range(0, 1)]
    [Tooltip("How far apart each pellet can vary from muzzle")]
    public float Spread = .75f;

    [Tooltip("The Prefab to use when creating an instance of the weapon in unity")]
    public GameObject WeaponTemplate;

    /// <inheritdoc />
    public override void OnStartup()
    {
      base.OnStartup();

      // clone it because otherwise we'll be directly destroying the prefab
      _localTemplateCopy = WeaponTemplate.CreateInstance();

      var weaponBehavior = _localTemplateCopy.GetComponent<WeaponBehavior>();
      MuzzleOffset = weaponBehavior.MuzzleOffset;
      HeldPosition = weaponBehavior.HeldPosition;

      UnityExtensions.Destroy(weaponBehavior);
      _localTemplateCopy.AddComponent<ProjectileWeaponBehavior>();
    }

    /// <inheritdoc />
    public override bool Act(PlayerBehavior actor, object instanceData)
      => ((ProjectileWeaponBehavior)instanceData).Act(actor);

    /// <inheritdoc />
    public override object Attach(PlayerBehavior actor, Transform parent, PositionAndRotation location)
    {
      var instance = _localTemplateCopy.CreateInstance(parent, location);
      var projectileWeapon = instance.GetComponent<ProjectileWeaponBehavior>();
      projectileWeapon.Initialize(this);

      instance.transform.localPosition -= HeldPosition.Offset;

      return projectileWeapon;
    }

    /// <inheritdoc />
    public override void Detach(PlayerBehavior actor, object instance)
    {
      UnityExtensions.Destroy(((ProjectileWeaponBehavior)instance).gameObject);
    }

    public override void Reload(object instance)
      => ((ProjectileWeaponBehavior)instance).Reload();

    public override EquippedItemInformation? GetEquippedItemInformation(object instance)
      => ((ProjectileWeaponBehavior)instance).GetEquippedItemInformation();

    /// <summary>
    ///   Provides the actual behavior for firing the projectile weapon and reloading it.
    /// </summary>
    private class ProjectileWeaponBehavior : BaseBehavior
    {
      private ProjectileWeapon _shared;

      public int CurrentClip { get; set; }

      /// <inheritdoc />
      public void Initialize(ProjectileWeapon shared)
      {
        _shared = shared;

        CurrentClip = 0;
      }

      public bool Act(PlayerBehavior actor)
      {
        if (CurrentClip == 0)
          return false;

        CurrentClip -= 1;

        for (var i = 0; i < _shared.NumberOfPellets; i++)
        {
          var positionAndRotation = _shared.MuzzleOffset.ToLocation(transform);
          var projectileInstance = _shared.Projectile.ProjectileTemplate.CreateInstance(positionAndRotation);

          var randomAngle = Random.Range(-15, (float)15) * _shared.Spread;
          projectileInstance.transform.rotation *= Quaternion.Euler(0, 0, randomAngle);

          var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
          behavior.Initialize(_shared, _shared.Projectile, actor.Allegiance);
        }

        return true;
      }

      public void Reload()
      {
        CurrentClip = _shared.ClipSize;
      }

      public EquippedItemInformation? GetEquippedItemInformation()
      {
        return new EquippedItemInformation
               {
                 CurrentAmount = CurrentClip,
                 TotalInInventory = 99,
                 TotalGroupSize = _shared.ClipSize
               };
      }
    }
  }
}