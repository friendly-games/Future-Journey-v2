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

    [Tooltip("The location to which the weapon should be placed on the player")]
    public RelativeOffset WeaponOffset;

    private Ownership<ProjectileWeapon, WeaponBehavior> _selectedWeapon;

    private Rigidbody2D _rigidBody;

    private Transform _reticule;

    private Transform _overallBody;

    private PlayerInputHandler _playerInputHandler;

    private RateLimiter _reloadLimiter;

    private HudInformationBehavior _hudInformationBehavior;

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
      _hudInformationBehavior = _overallBody.GetComponent<HudInformationBehavior>();

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
        _hudInformationBehavior.WeaponInfo = $"{NumberOfRemainingShots}/{_selectedWeapon.Programming.ClipSize}";
      }
    }

    private void SelectWeapon(ProjectileWeapon weapon)
    {
      if (weapon == _selectedWeapon.Programming)
        return;

      _selectedWeapon.Destroy();
      weapon.Attach(ref _selectedWeapon, WeaponOffset.ToLocation(transform));

      _hudInformationBehavior.EquipmentName = _selectedWeapon.Programming.Name;

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
      _hudInformationBehavior.ReloadPercentage = (int)(_reloadLimiter.PercentComplete * 100);

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
      ResetOverallRotation();
    }

    private void ActWithCurrentlyEquippedItem()
    {
      if (NumberOfRemainingShots <= 0)
        return;

      _selectedWeapon.Programming?.Act(_selectedWeapon.Behavior, Allegiance);
      NumberOfRemainingShots--;
    }

    private Quaternion _lastRotation
      = Quaternion.identity;

    private void ResetOverallRotation()
    {
      // the rotation of the overall body should never change
      _overallBody.rotation = Quaternion.identity;
    }

    public void FixedUpdate()
    {
      _rigidBody.angularVelocity = 0;
      _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _playerInputHandler.DesiredVelocity, .8f);
      transform.rotation = Quaternion.Lerp(_lastRotation, _playerInputHandler.DesiredRotation, .8f);
      _reticule.transform.localPosition = _playerInputHandler.DesiredTrackerLocation;

      _lastRotation = transform.rotation;
    }
  }
}