using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  public class PlayerBehavior : BaseBehavior
  {
    [Tooltip("The team to which the player belongs")]
    public Allegiance Allegiance;

    [Tooltip("All of the weapons that are available to the player")]
    public ProjectileWeapon[] AvailableWeapons;

    public RelativeOffset WeaponOffset;

    private ProjectileWeapon _currentWeapon;

    private Ownership<ProjectileWeapon, WeaponBehavior> _selectedWeapon;

    private Rigidbody2D _rigidBody;
    private Transform _reticule;
    private Transform _overallBody;

    private PlayerInputHandler _playerInputHandler;

    public void Start()
    {
      _overallBody = transform.parent;
      _reticule = _overallBody.Find("Reticle");

      _rigidBody = _overallBody.GetComponent<Rigidbody2D>();

      _selectedWeapon = new Ownership<ProjectileWeapon, WeaponBehavior>(gameObject);

      _playerInputHandler = new PlayerInputHandler();

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      SelectWeapon(AvailableWeapons.FirstOrDefault());
    }

    private void SelectWeapon(ProjectileWeapon weapon)
    {
      _selectedWeapon.Destroy();
      weapon.Attach(ref _selectedWeapon, WeaponOffset.ToLocation(transform));
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        _selectedWeapon.Programming?.Act(_selectedWeapon.Behavior, Allegiance);
      }

      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
        SelectWeapon(AvailableWeapons[0]);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        SelectWeapon(AvailableWeapons[1]);
      }

      _playerInputHandler.Recalculate();
    }

    public void FixedUpdate()
    {
      _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _playerInputHandler.DesiredVelocity, .8f);
      transform.rotation = Quaternion.Lerp(transform.rotation, _playerInputHandler.DesiredRotation, .8f);
      _reticule.transform.localPosition = _playerInputHandler.DesiredTrackerLocation;
    }
  }
}