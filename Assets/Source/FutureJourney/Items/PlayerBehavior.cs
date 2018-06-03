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

    private SelectedItemData _selectedActionable;

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

      _rigidBody = transform.GetComponent<Rigidbody2D>();
      _hudInformationBehavior = transform.GetComponent<HudInformationBehavior>();

      SelectActable(AvailableWeapons.FirstOrDefault());
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

        if (_selectedActionable.Actable is ProjectileWeapon weapon)
        {
          _hudInformationBehavior.WeaponInfo = $"{NumberOfRemainingShots}/{weapon.ClipSize}";
        }
        else
        {
          _hudInformationBehavior.WeaponInfo = "???";
        }
      }
    }

    /// <summary />
    private void SelectActable(IUsable actable)
    {
      if (_selectedActionable.Actable == actable)
        return;

      _selectedActionable.Actable?.Detach(this, _selectedActionable.InstanceData);
      
      var location = _playerBodyBehavior.WeaponOffset.ToLocation(_playerBody);
      var instanceData = actable.Attach(this, _playerBody.gameObject.transform,location);
      _selectedActionable = new SelectedItemData(actable, instanceData);
    
      _hudInformationBehavior.EquipmentName = actable?.Name ?? "<>";

      Reload();
      
      _reloadLimiter.RechargeRate = actable.TimeToRecharge;
    }

    /// <summary />
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
        if (_selectedActionable.Actable is ProjectileWeapon weapon)
        {
          NumberOfRemainingShots = weapon.ClipSize;
        }
        else
        {
          NumberOfRemainingShots = 10;
        }
        
        _reloadRequested = false;
      }

      if (Input.GetMouseButtonDown(0))
      {
        ActWithCurrentlyEquippedItem();
      }

      if (Input.GetMouseButtonDown(1))
      {
        ActWithCurrentlyEquippedPlacable();
      }

      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
        SelectActable(AvailableWeapons[0]);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        SelectActable(AvailableWeapons[1]);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        SelectActable(AvailablePlaceables[0]);
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

      _selectedActionable.Actable?.Act(this, _selectedActionable.InstanceData);
      NumberOfRemainingShots--;
    }

    private void ActWithCurrentlyEquippedPlacable()
    {
      AvailablePlaceables[0].PlaceOnGrid(this, new GridCoordinate(_reticule.position));
    }

    public void FixedUpdate()
    {
      ResetOverallRotation();

      _rigidBody.angularVelocity = 0;
      _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, _playerInputHandler.DesiredVelocity, .8f);
      UpdateReticuleLocation(_playerInputHandler.DesiredTrackerLocation, true);
    }
    
    /// <summary />
    private void UpdateReticuleLocation(Vector3 relativeLocation, bool shouldNormalizeToGrid)
    {
      var absoluteLocation = _reticule.transform.parent.TransformPoint(relativeLocation);
      
      if (shouldNormalizeToGrid)
      {
        absoluteLocation = GridCoordinate.NormalizeToGrid(absoluteLocation);
      }

      _reticule.transform.position = absoluteLocation;

      ReticulePosition = absoluteLocation;
    }

    public Vector3 ReticulePosition { get; set; }

    private struct SelectedItemData
    {
      public SelectedItemData(IUsable actable, GameObject instanceData)
      {
        Actable = actable;
        InstanceData = instanceData;
      }
      
      public IUsable Actable { get; }
      public object InstanceData { get; }
    }
  }
}
