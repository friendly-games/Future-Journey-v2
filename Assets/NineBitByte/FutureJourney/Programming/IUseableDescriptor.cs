using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Items;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> An object that is usable. </summary>
  public interface IUseableDescriptor
  {
    /// <summary> The name of the item. </summary>
    string Name { get; }

    /// <summary> The amount of time that must elapse before the item can be used again. </summary>
    TimeSpan TimeToRecharge { get; }

    /// <summary>
    ///   Initializes an instance of the item which the specific actor can "use" to do some sort of
    ///   action in the world, such as firing a bullet, placing an object onto the grid, or swinging
    ///   an axe.
    /// </summary>
    /// <param name="actor"> The actor to which the item is being attached. This serves as a visual indicator that
    ///    the actor has the current usable "equipped". </param>
    /// <param name="parent"> The owner of any GameObject that should be added. </param>
    /// <param name="location"> The absolute position at which any GameObjects should be added. </param>
    /// <returns> A usable which can be *used* by invoking the <see cref="IUsable.Act"/> method. </returns>
    IUsable CreateAndAttachUsable(PlayerBehavior actor, Transform parent, PositionAndRotation location);
  }
}