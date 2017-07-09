using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NineBitByte.Common;
using UnityEngine;
using UnityEngine.UI;

namespace NineBitByte.FutureJourney.Items
{
  public class HudInformationBehavior : BaseBehavior
  {
    [Tooltip("Location to write information about the current weapon/ammo")]
    [SerializeField]
    [UsedImplicitly]
    private Text EquipmentInformationTextField;

    [Tooltip("How close are we to being done with reloading")]
    [SerializeField]
    [UsedImplicitly]
    private Text ReloadProgressTextField;

    [Tooltip("The name of our currently equipped weapon")]
    [SerializeField]
    [UsedImplicitly]
    private Text EquipmentNameTextField;

    private int _reloadPercentage;

    public int ReloadPercentage
    {
      get { return _reloadPercentage; }
      set
      {
        if (_reloadPercentage == value)
          return;

        _reloadPercentage = value;
        ReloadProgressTextField.text = $"{value:00}%";
      }
    }

    public string EquipmentName
    {
      set { EquipmentNameTextField.text = value; }
    }

    public string WeaponInfo
    {
      set { EquipmentInformationTextField.text = value; }
    }
  }
}