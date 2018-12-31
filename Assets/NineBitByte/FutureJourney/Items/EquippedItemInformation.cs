using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Holds information about an equipped item. </summary>
  public struct EquippedItemInformation
  {
    /// <summary> The current amount loaded into the item. </summary>
    public int CurrentAmount;

    /// <summary> The total amount remaining in the current group. </summary>
    public int TotalGroupSize;

    /// <summary> The total amount that is in the inventory. </summary>
    public int TotalInInventory;
  }

  public interface IEquippedHud
  {
    void UpdateEquippedStatus(EquippedItemInformation? info);
    int ReloadPercentage { get; set; }
    string EquipmentName { set; }
  }
}