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
  public abstract class BaseUseableDescriptor : BaseScriptable, IUseableDescriptor
  {
    [Tooltip("The name of the item")]
    public string Name;
    
    [Tooltip("The amount of time it takes to reload this weapon")]
    [TimeField(TimeSpecifiedIn.Seconds, Minimum = 0, Maximum = 2)]
    [FormerlySerializedAs("TimeToReload")]
    public TimeField TimeToRecharge;
    
    /// <inheritdoc />
    public abstract IUsable CreateAndAttachUsable(PlayerBehavior actor, Transform parent, PositionAndRotation location);
    
    /// <inheritdoc />
    string IUseableDescriptor.Name
      => Name;

    /// <inheritdoc />
    TimeSpan IUseableDescriptor.TimeToRecharge
      => TimeToRecharge.Time;
  }
}