using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary>
  ///  Base class for an Programming which allows the actor to "use" such as firing a weapon
  ///  or placing a building in the world.
  /// </summary>
  public abstract class BaseUsable : BaseScriptable, IUsable
  {
    [Tooltip("The name of the item")]
    public string Name;
    
    [Tooltip("The amount of time it takes to reload this weapon")]
    [TimeField(TimeSpecifiedIn.Seconds, Minimum = 0, Maximum = 2)]
    [FormerlySerializedAs("TimeToReload")]
    public TimeField TimeToRecharge;

    /// <inheritdoc />
    public abstract bool Act(PlayerBehavior playerBehavior, object instance);
    
    /// <inheritdoc />
    public abstract object Attach(PlayerBehavior actor, Transform parent, PositionAndRotation location);

    /// <inheritdoc />
    public abstract void Detach(PlayerBehavior actor, object instance);

    /// <summary> Gets the currently equipd information about the given item. </summary>
    public virtual EquippedItemInformation? GetEquippedItemInformation(object instance)
    {
      return null;
    }
    
    /// <inheritdoc />
    string IUsable.Name
      => Name;

    /// <inheritdoc />
    TimeSpan IUsable.TimeToRecharge
      => TimeToRecharge.Time;

    /// <summary> Method that refills any inventory that the item has. </summary>
    public abstract void Reload(object instance);
  }
}