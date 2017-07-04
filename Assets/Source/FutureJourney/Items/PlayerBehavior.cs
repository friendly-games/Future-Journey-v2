using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source_Project;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class PlayerBehavior : BaseBehavior
  {
    [Tooltip("The team to which the player belongs")]
    public Allegiance Allegiance;

    [Tooltip("All of the weapons that are available to the player")]
    public WeaponScriptable[] AvailableWeapons;

    [RelativeOffset]
    public Vector3 WeaponOffset;

    private WeaponScriptable _currentWeapon;

    private Ownership<WeaponScriptable, WeaponBehavior> _selectedWeapon;

    private Rigidbody2D _rigidBody;
    private Vector2 _desiredSpeed;

    public void Start()
    {
      _selectedWeapon = new Ownership<WeaponScriptable, WeaponBehavior>(gameObject);

      SelectWeapon(AvailableWeapons.FirstOrDefault());
    }

    private void SelectWeapon(WeaponScriptable weapon)
    {
      _selectedWeapon.Destroy();
      weapon.Attach(ref _selectedWeapon, WeaponOffset);

      _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        _selectedWeapon.Programming?.Act(_selectedWeapon.Behavior, Allegiance);
      }

      var movementVector = CalculateMovementVector();
      _desiredSpeed = movementVector;
    }

    public void FixedUpdate()
    {
      _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _desiredSpeed, .8f);
    }

    private Vector2 CalculateMovementVector()
    {
      var movement = new Vector2();

      if (Input.GetKey(KeyCode.A))
      {
        movement.x -= 1;
      }

      if (Input.GetKey(KeyCode.D))
      {
        movement.x += 1;
      }

      if (Input.GetKey(KeyCode.W))
      {
        movement.y += 1;
      }

      if (Input.GetKey(KeyCode.S))
      {
        movement.y -= 1;
      }

      if (movement.sqrMagnitude > 0)
      {
        movement.Normalize();
      }

      return movement * 3;
    }
  }
}