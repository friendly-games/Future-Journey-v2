using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NineBitByte.Common;
using NineBitByte.Common.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace NineBitByte.FutureJourney.Items
{
  public class HudInformationBehavior : BaseBehavior, IEquippedHud
  {
    [Tooltip("Location to write information about the current weapon/ammo")]
    [SerializeField]
    private Text EquipmentInformationTextField;

    [Tooltip("How close are we to being done with reloading")]
    [SerializeField]
    private Text ReloadProgressTextField;

    [Tooltip("The name of our currently equipped weapon")]
    [SerializeField]
    private Text EquipmentNameTextField;
    
    [Tooltip("Contains a running total of the amount of damage done")]
    [SerializeField]
    private Text DamageDoneTextField;

    private int _reloadPercentage;
    private IStatisticContainer _statistics;

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
      set => EquipmentNameTextField.text = value; 
    }

    public IStatisticContainer Statistics
    {
      get => _statistics;
      set
      {
        if (_statistics != null)
        {
          _statistics.StatisticChanged -= HandleStatisticsChanged;
        }
        
        _statistics = value;
        
        if (_statistics != null)
        {
          _statistics.StatisticChanged += HandleStatisticsChanged;
        }

        ClearStatistics();
      }
    }

    private void ClearStatistics()
    {
      DamageDoneInfo = "0pts";
    }

    private void HandleStatisticsChanged(IStatisticContainer container, IStatistic statistic)
    {
      if (statistic is DoubleStatistic doubleStat && doubleStat.Id == KnownStats.DamageDone)
      {
        DamageDoneInfo = $"{doubleStat.Value}pts";
      }
    }

    private string WeaponInfo
    {
      set => EquipmentInformationTextField.text = value; 
    }

    private string DamageDoneInfo
    {
      set => DamageDoneTextField.text = value;
    }

    public void UpdateEquippedStatus(EquippedItemInformation? info)
    {
      if (info is EquippedItemInformation value)
      {
        WeaponInfo = $"{value.CurrentAmount}/{value.TotalGroupSize} ({value.TotalInInventory})";
      }
      else
      {
        WeaponInfo = "∞";
      }
    }
  }
}