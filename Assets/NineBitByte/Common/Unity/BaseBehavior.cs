using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Common
{
  /// <summary> Base class for all behaviors in the game. </summary>
  public class BaseBehavior : MonoBehaviour
  {
    /// <summary> Resets the orientation of the current game object to zero. </summary>
    protected void ResetOverallRotation()
    {
      // the rotation of the overall body should never change
      transform.rotation = Quaternion.identity;
    }
  }

  public class BaseProgrammableBehavior<TProgramming> : MonoBehaviour
  {
    public TProgramming Programming { get; private set; }

    public virtual void Initialize(TProgramming programming)
    {
      Programming = programming;
    }
  }

  /// <summary> Extension methods for <see cref="BaseProgrammableBehavior{TProgramming}"/> </summary>
  public static class BaseProgrammableBehaviorExtensions
  {
    public static TSelf Init<TSelf, TProgramming>(
      this TSelf self, 
      TProgramming programming)
    where TSelf : BaseProgrammableBehavior<TProgramming>
    {
      self.Initialize(programming);
      return self;
    }
  }
}