using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> An object that is usable. </summary>
  public interface IUsable
  {
    /// <summary> The name of the item. </summary>
    string Name { get; }

    /// <summary> The amount of time that must elapse before the item can be used again. </summary>
    TimeSpan TimeToRecharge { get; }

    /// <summary>
    ///   Initiates the "Use" behavior.
    /// </summary>
    /// <param name="playerBehavior"> The actor that is using the given item. </param>
    /// <param name="instance"> Any instance data which was previously created via <see cref="BaseUsable.Attach"/>. </param>
    bool Act(PlayerBehavior playerBehavior, object instance);

    /// <summary>
    ///   Initializes an instance of the item which the specific actor can "use".
    /// </summary>
    /// <param name="actor"> The actor to which the item is being attached. </param>
    /// <param name="parent"> The owner of any GameObject that should be added. </param>
    /// <param name="location"> The absolute position at which any GameObjects can be added. </param>
    /// <returns> Instance data that will be passed into all consuming methods. </returns>
    object Attach(PlayerBehavior actor, Transform parent, PositionAndRotation location);

    /// <summary>
    ///   Parallel to <see cref="BaseUsable.Attach"/> except for when the item is being disposed.
    /// </summary>
    void Detach(PlayerBehavior actor, object instance);

    /// <summary> Method that refills any inventory that the item has. </summary>
    void Reload(object instance);

    /// <summary> Gets the currently equipd information about the given item. </summary>
    EquippedItemInformation? GetEquippedItemInformation(object instance);
  }
}