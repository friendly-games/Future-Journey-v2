using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;
using UnityEngine.UI;

namespace NineBitByte.FutureJourney.Items
{
  public class PlayerBehavior : BaseBehavior
  {
    [Tooltip("The team to which the player belongs")]
    public Allegiance Allegiance;

    [Tooltip("All of the weapons that are available to the player")]
    public ProjectileWeapon[] AvailableWeapons;

    [Tooltip("The location to which the weapon should be placed on the player")]
    public RelativeOffset WeaponOffset;

    [Tooltip("Location to write information about the current weapon/ammo")]
    public Text EquipmentInformation;

    [Tooltip("How close are we to being done with reloading")]
    public Text ReloadProgress;

    [Tooltip("The name of our currently equipped weapon")]
    public Text EquipmentName;

    private float _lastPercentComplete;

    private Ownership<ProjectileWeapon, WeaponBehavior> _selectedWeapon;

    private Rigidbody2D _rigidBody;

    private Transform _reticule;

    private Transform _overallBody;

    private PlayerInputHandler _playerInputHandler;

    private RateLimiter _reloadLimiter;

    private int _numberOfRemainingShots;
    private bool _reloadRequested = true;

    public void Start()
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      _reloadLimiter = new RateLimiter(allowFirst: true);

      _overallBody = transform.parent;
      _reticule = _overallBody.Find("Reticle");

      _rigidBody = _overallBody.GetComponent<Rigidbody2D>();

      _selectedWeapon = new Ownership<ProjectileWeapon, WeaponBehavior>(gameObject);

      _playerInputHandler = new PlayerInputHandler();
   
      SelectWeapon(AvailableWeapons.FirstOrDefault());
    }

    /// <summary> The total number of remaining bullets we have to fire. </summary>
    private int NumberOfRemainingShots
    {
      get { return _numberOfRemainingShots; }
      set
      {
        _numberOfRemainingShots = value;
        EquipmentInformation.text = $"{NumberOfRemainingShots}/{_selectedWeapon.Programming.ClipSize}";
      }
    }

    private void SelectWeapon(ProjectileWeapon weapon)
    {
      if (weapon == _selectedWeapon.Programming)
        return;

      _selectedWeapon.Destroy();
      weapon.Attach(ref _selectedWeapon, WeaponOffset.ToLocation(transform));

      EquipmentName.text = _selectedWeapon.Programming.Name;

      Reload();

      _reloadLimiter.RechargeRate = _selectedWeapon.Programming.TimeToReload;
    }

    public void Reload()
    {
      if (_reloadLimiter.TryRestart())
      {
        // TODO not always true (e.g. shotguns)
        NumberOfRemainingShots = 0;
        _reloadRequested = true;
      }
    }

    public void Update()
    {
      if (_reloadLimiter.PercentComplete != _lastPercentComplete)
      {
        _lastPercentComplete = _reloadLimiter.PercentComplete;
        int asInt = (int)(_lastPercentComplete * 100);
        ReloadProgress.text = $"{asInt:00}%";
      }

      if (_reloadRequested && _reloadLimiter.CanRestart)
      {
        NumberOfRemainingShots = _selectedWeapon.Programming.ClipSize;
        _reloadRequested = false;
      }

      if (Input.GetMouseButtonDown(0))
      {
        ActWithCurrentlyEquippedItem();
      }

      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
        SelectWeapon(AvailableWeapons[0]);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        SelectWeapon(AvailableWeapons[1]);
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
        Reload();
      }

      _playerInputHandler.Recalculate();
    }

    private void ActWithCurrentlyEquippedItem()
    {
      if (NumberOfRemainingShots <= 0)
        return;

      _selectedWeapon.Programming?.Act(_selectedWeapon.Behavior, Allegiance);
      NumberOfRemainingShots--;
    }

    public void FixedUpdate()
    {
      _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _playerInputHandler.DesiredVelocity, .8f);
      transform.rotation = Quaternion.Lerp(transform.rotation, _playerInputHandler.DesiredRotation, .8f);
      _reticule.transform.localPosition = _playerInputHandler.DesiredTrackerLocation;
    }
  }
}