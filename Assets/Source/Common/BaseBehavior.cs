using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Common
{
  /// <summary> Base class for all behaviors in the game. </summary>
  public class BaseBehavior : MonoBehaviour
  {
    
  }

  public class BaseProgrammableBehavior<TProgramming> : MonoBehaviour
  {
    public TProgramming Programming { get; private set; }

    public virtual void Initialize(TProgramming programming)
    {
      Programming = programming;
    }
  }
}