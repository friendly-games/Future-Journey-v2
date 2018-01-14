using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary>
  ///  The behavior that sits above the <see cref="PlayerBodyBehavior"/> controlling it and
  ///  providing a non-rotating body on which other gui elements (such as the cursor or health) can
  ///  live.
  /// </summary>
  public class PlayerBehavior : BaseBehavior, IOwner
  {
    [Tooltip("The team to which the player belongs")]
    public Allegiance Allegiance;

    [Tooltip("All of the weapons that are available to the player")]
    public ProjectileWeapon[] AvailableWeapons;

    [Tooltip("All of the buildings that are available to the player")]
    public Placeable[] AvailablePlaceables;

    private WeaponBehavior _selectedWeapon;

    private Rigidbody2D _rigidBody;

    private Transform _reticule;

    private PlayerInputHandler _playerInputHandler;

    private RateLimiter _reloadLimiter;

    private HudInformationBehavior _hudInformationBehavior;

    private int _numberOfRemainingShots;
    private bool _reloadRequested = true;

    private Transform _playerBody;
    private PlayerBodyBehavior _playerBodyBehavior;
    private WorldGrid _worldGrid;

    public void Start()
    {
      _worldGrid = FindObjectOfType<WorldManagerBehavior>().WorldGrid;

      _playerInputHandler = new PlayerInputHandler();
      _reloadLimiter = new RateLimiter(allowFirst: true);

      _reticule = transform.Find("Reticle");
      _playerBodyBehavior = transform.Find("Body").GetComponent<PlayerBodyBehavior>();
      _playerBodyBehavior.Initalize(_playerInputHandler);

      _playerBody = _playerBodyBehavior.transform;

      _selectedWeapon = null;

      _rigidBody = transform.GetComponent<Rigidbody2D>();
      _hudInformationBehavior = transform.GetComponent<HudInformationBehavior>();

      SelectWeapon(AvailableWeapons.FirstOrDefault());
    }

    Allegiance IOwner.Allegiance
      => Allegiance;

    WorldGrid IOwner.AssociatedGrid
      => _worldGrid;

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
      if (weapon == _selectedWeapon?.Programming)
        return;

      UnityExtensions.Destroy(_selectedWeapon?.gameObject);
      _selectedWeapon = weapon.Attach(
        _playerBody.gameObject.transform,
        _playerBodyBehavior.WeaponOffset.ToLocation(_playerBody)
       );

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

      if (Input.GetMouseButtonDown(1))
      {
        AvailablePlaceables[0].PlaceOnGrid(this, new GridCoordinate(transform.position));
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

      _selectedWeapon?.Programming.Act(_selectedWeapon, Allegiance);
      NumberOfRemainingShots--;
    }

    public void FixedUpdate()
    {
      ResetOverallRotation();

      _rigidBody.angularVelocity = 0;
      _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _playerInputHandler.DesiredVelocity, .8f);
      _reticule.transform.localPosition = _playerInputHandler.DesiredTrackerLocation;
    }
  }
}
