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
  public class ProjectileWeapon : BaseUsable
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

    [Tooltip("The location at which the projectile is fired from the location of the owner GameObject")]
    public RelativeOffset MuzzleOffset;

    [Tooltip("The location where the user should hold the weapon")]
    public RelativeOffset HeldPosition;

    private GameObject _localTemplateCopy;
    
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
      => Act((ProjectileWeaponBehavior)instanceData, actor.Allegiance);

    /// <summary> Creates a projectile for the given weapon. </summary>
    private bool Act(ProjectileWeaponBehavior weaponInstance, Allegiance allegiance)
    {
      if (weaponInstance.CurrentClip == 0)
        return false;

      weaponInstance.CurrentClip -= 1;
      
      for (int i = 0; i < NumberOfPellets; i++)
      {
        var positionAndRotation = MuzzleOffset.ToLocation(weaponInstance.transform);
        var projectileInstance = Projectile.ProjectileTemplate.CreateInstance(positionAndRotation);

        var randomAngle = UnityEngine.Random.Range(-15, (float)15) * Spread;
        projectileInstance.transform.rotation *= Quaternion.Euler(0, 0, randomAngle);

        var behavior = projectileInstance.GetComponent<ProjectileBehavior>();
        behavior.Initialize(this, Projectile, allegiance);
      }

      return true;
    }

    /// <inheritdoc />
    public override object Attach(PlayerBehavior actor, Transform parent, PositionAndRotation location)
    {
      var instance = _localTemplateCopy.CreateInstance(parent, location);
      var projectileWeapon = instance.GetComponent<ProjectileWeaponBehavior>();
      projectileWeapon.CurrentClip = 0;
      
      instance.transform.localPosition -= HeldPosition.Offset;
      
      return projectileWeapon;
    }

    /// <inheritdoc />
    public override void Detach(PlayerBehavior actor, object instance)
    {
      UnityExtensions.Destroy(((ProjectileWeaponBehavior)instance).gameObject);
    }

    public override void Reload(object instance)
    {
      var behavior = (ProjectileWeaponBehavior)instance;
      behavior.CurrentClip = ClipSize;
    }
    
    public override EquippedItemInformation? GetEquippedItemInformation(object instance)
    {
      var behavior = (ProjectileWeaponBehavior)instance;
      return new EquippedItemInformation
             {
               CurrentAmount = behavior.CurrentClip,
               TotalInInventory = 99,
               TotalGroupSize = ClipSize
             };
    }

    private class ProjectileWeaponBehavior : BaseBehavior
    {
      public int CurrentClip { get; set; }
    }
  }
}