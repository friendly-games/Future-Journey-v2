using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using UnityEngine;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("AvailableWeapons")]
    [Tooltip("All of the weapons that are available to the player")]
    public ProjectileWeaponDescriptor[] AvailableWeaponsDescriptor;

    [FormerlySerializedAs("AvailablePlaceables")]
    [Tooltip("All of the buildings that are available to the player")]
    public PlaceableDescriptor[] AvailablePlaceablesDescriptor;

    private IUsable _selectedUsable;

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

      SelectUsable(AvailableWeaponsDescriptor.FirstOrDefault());

    }

    /// <inheritdoc />
    Allegiance IOwner.Allegiance
      => Allegiance;

    /// <inheritdoc />
    WorldGrid IOwner.AssociatedGrid
      => _worldGrid;

    /// <summary />
    private void SelectUsable(IUseableDescriptor actable)
    {
      if (_selectedUsable?.Shared == actable)
        return;

      _selectedUsable?.Detach(this);
      
      var location = _playerBodyBehavior.WeaponOffset.ToLocation(_playerBody);
      var usable = actable.CreateAndAttachUsable(this, _playerBody.gameObject.transform,location);
      _selectedUsable = usable;
    
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
        _selectedUsable?.Reload(this);
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
        SelectUsable(AvailableWeaponsDescriptor[0]);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        SelectUsable(AvailableWeaponsDescriptor[1]);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        SelectUsable(AvailablePlaceablesDescriptor[0]);
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
      var equippedItemInformation = _selectedUsable?.GetEquippedItemInformation(this);

      _hud.UpdateEquippedStatus(equippedItemInformation);
    }

    private void ActWithCurrentlyEquippedItem()
    {
      _selectedUsable?.Act(this);
      UpdateEquippedHud();
    }

    private void ActWithCurrentlyEquippedPlacable()
    {
      AvailablePlaceablesDescriptor[0].PlaceOnGrid(this, new GridCoordinate(_reticule.position));
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
      public SelectedItemData(IUseableDescriptor item, IUsable instanceData)
      {
        Item = item;
        InstanceData = instanceData;
      }

      /// <summary> The current item/programming. </summary>
      public IUseableDescriptor Item { get; }

      /// <summary> The data associated with the item. </summary>
      public IUsable InstanceData { get; }
    }
  }
}
