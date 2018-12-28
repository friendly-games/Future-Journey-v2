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

    private IUsable _selectedActionable;

    private Rigidbody2D _rigidBody;

    private Transform _reticule;

    private PlayerInputHandler _playerInputHandler;

    private RateLimiter _reloadLimiter;

    private int _numberOfRemainingShots;
    private bool _reloadRequested = true;

    private Transform _playerBody;

    private PlayerBodyBehavior _playerBodyBehavior;

    private WorldGrid _worldGrid;

    /// <summary> The current hud for the player. </summary>
    private IEquippedHud _hud;

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
      _hud = transform.GetComponent<HudInformationBehavior>();

      SelectActable(AvailableWeapons.FirstOrDefault());

    }

    /// <inheritdoc />
    Allegiance IOwner.Allegiance
      => Allegiance;

    /// <inheritdoc />
    WorldGrid IOwner.AssociatedGrid
      => _worldGrid;

    /// <summary />
    private void SelectActable(IUsableTemplate actable)
    {
      if (_selectedActionable != null)
      if (_selectedActionable?.Shared == actable)
        return;

      _selectedActionable?.Detach(this);
      
      var location = _playerBodyBehavior.WeaponOffset.ToLocation(_playerBody);
      var usable = actable.Attach(this, _playerBody.gameObject.transform,location);
      _selectedActionable = usable;
    
      _hud.EquipmentName = actable?.Name ?? "<>";

      Reload();
      
      _reloadLimiter.RechargeRate = actable.TimeToRecharge;
    }

    /// <summary />
    public void Reload()
    {
      if (_reloadLimiter.TryRestart())
      {
        UpdateEquippedHud();
        _reloadRequested = true;
      }
    }

    public void Update()
    {
      _hud.ReloadPercentage = (int)(_reloadLimiter.PercentComplete * 100);

      if (_reloadRequested && _reloadLimiter.CanRestart)
      {
        _selectedActionable?.Reload(this);
        UpdateEquippedHud();

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

    private void UpdateEquippedHud()
    {
      var equippedItemInformation = _selectedActionable?.GetEquippedItemInformation(this);

      _hud.UpdateEquippedStatus(equippedItemInformation);
    }

    private void ActWithCurrentlyEquippedItem()
    {
      _selectedActionable?.Act(this);
      UpdateEquippedHud();
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
    
    /// <summary> Holds the current selected item. </summary>
    private struct SelectedItemData
    {
      public SelectedItemData(IUsableTemplate item, IUsable instanceData)
      {
        Item = item;
        InstanceData = instanceData;
      }

      /// <summary> The current item/programming. </summary>
      public IUsableTemplate Item { get; }

      /// <summary> The data associated with the item. </summary>
      public IUsable InstanceData { get; }
    }
  }
}
