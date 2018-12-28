using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Items;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary>
  ///   An instance of a Usable object in the world.
  /// </summary>
  public interface IUsable
  {
    /// <summary> The data that's shared between all instances of this usable. </summary>
    IUsableTemplate Shared { get; }
      
    /// <summary>
    ///   Initiates the "Use" behavior.
    /// </summary>
    /// <param name="actor"> The actor that is using the given item. </param>
    /// <returns> True if the instance was used, false if it was not. </returns>
    bool Act(PlayerBehavior actor);

    /// <summary>
    ///   Reload any type of inventory that comes from the given actor.
    /// </summary>
    void Reload(PlayerBehavior actor);

    /// <summary>
    ///   Gets statistics about the currently equipped item.
    /// </summary>
    EquippedItemInformation? GetEquippedItemInformation(PlayerBehavior actor);

    /// <summary>
    ///   Performs any cleanup that needs to be done as part of being removed from the character
    /// </summary>
    void Detach(PlayerBehavior actor);
  }
}