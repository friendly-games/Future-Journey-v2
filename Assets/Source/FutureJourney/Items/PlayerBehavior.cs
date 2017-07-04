using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source_Project;
using UnityEngine;
using UnityEngine.Playables;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class PlayerBehavior : BaseBehavior
  {
    public WeaponScriptable[] AvailableWeapons;

    [RelativeOffset]
    public Vector3 WeaponOffset;

    private WeaponScriptable _currentWeapon;

    private Ownership<WeaponScriptable, WeaponBehavior> _selectedWeapon;

    public void Start()
    {
      _selectedWeapon = new Ownership<WeaponScriptable, WeaponBehavior>(gameObject);

      SelectWeapon(AvailableWeapons.FirstOrDefault());
    }

    private void SelectWeapon(WeaponScriptable weapon)
    {
      _selectedWeapon.Destroy();
      weapon.Attach(ref _selectedWeapon, WeaponOffset);
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        _selectedWeapon.Programming?.Act(_selectedWeapon.Behavior);
      }
    }
  }
}