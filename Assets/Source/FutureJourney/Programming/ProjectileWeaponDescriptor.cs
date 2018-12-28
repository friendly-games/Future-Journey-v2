using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> All properties for a weapon that can be fired. </summary>
  [CreateAssetMenu(menuName = "Items/Weapon")]
  public class ProjectileWeaponDescriptor : BaseUseableDescriptor
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

    [FormerlySerializedAs("Projectile")]
    [Tooltip("Projectile information")]
    public ProjectileDescriptor ProjectileDescriptor;

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
    public override IUsable Attach(PlayerBehavior actor, Transform parent, PositionAndRotation location)
    {
      var instance = _localTemplateCopy.CreateInstance(parent, location);
      var projectileWeapon = instance.GetComponent<ProjectileWeaponBehavior>();
      projectileWeapon.Initialize(this);

      instance.transform.localPosition -= HeldPosition.Offset;

      return projectileWeapon;
    }

    /// <summary>
    ///   Provides the actual behavior for firing the projectile weapon and reloading it.
    /// </summary>
    private class ProjectileWeaponBehavior : BaseBehavior, IUsable
    {
      private ProjectileWeaponDescriptor _shared;

      public void Initialize(ProjectileWeaponDescriptor shared)
      {
        _shared = shared;

        CurrentClip = 0;
      }

      // the # of bullets left in the current clip
      private int CurrentClip { get; set; }

      /// <inheritdoc />
      IUseableDescriptor IUsable.Shared
        => _shared;

      /// <inheritdoc />
      public bool Act(PlayerBehavior actor)
      {
        if (CurrentClip == 0)
          return false;

        CurrentClip -= 1;

        for (var i = 0; i < _shared.NumberOfPellets; i++)
        {
          var positionAndRotation = _shared.MuzzleOffset.ToLocation(transform);
          var projectileInstance = _shared.ProjectileDescriptor.ProjectileTemplate.CreateInstance(positionAndRotation);

          var randomAngle = Random.Range(-15, (float)15) * _shared.Spread;
          projectileInstance.transform.rotation *= Quaternion.Euler(0, 0, randomAngle);

          var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
          behavior.Initialize(_shared, _shared.ProjectileDescriptor, actor.Allegiance);
        }

        return true;
      }

      /// <inheritdoc />
      public void Reload(PlayerBehavior actor)
      {
        CurrentClip = _shared.ClipSize;
      }

      /// <inheritdoc />
      public EquippedItemInformation? GetEquippedItemInformation(PlayerBehavior actor)
      {
        return new EquippedItemInformation
               {
                 CurrentAmount = CurrentClip,
                 TotalInInventory = 99,
                 TotalGroupSize = _shared.ClipSize
               };
      }

      /// <inheritdoc />
      public void Detach(PlayerBehavior actor)
      {
        UnityExtensions.Destroy(gameObject);
      }
    }
  }
}