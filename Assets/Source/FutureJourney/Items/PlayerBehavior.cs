using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source_Project;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class PlayerBehavior : BaseBehavior
  {
    public WeaponScriptable[] AvailableWeapons;

    [RelativeOffset]
    public Vector3 WeaponOffset;

    private WeaponScriptable _currentWeapon;
    private GameObject _selectedWeaponInstance;

    public void Start()
    {
      SelectWeapon(AvailableWeapons.FirstOrDefault());
    }

    private void SelectWeapon(WeaponScriptable weapon)
    {
      if (_selectedWeaponInstance != null)
      {
        UnityExtensions.Destroy(_selectedWeaponInstance);
      }

      _currentWeapon = weapon;

      if (_currentWeapon != null)
      {
        _selectedWeaponInstance = 
          _currentWeapon.WeaponTemplate.CreateInstance(transform, WeaponOffset, Quaternion.identity);
      }

    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        var weaponBehavior = _selectedWeaponInstance.GetComponent<WeaponBehavior>();
        _currentWeapon.Fire(weaponBehavior);
      }
    }
  }
}