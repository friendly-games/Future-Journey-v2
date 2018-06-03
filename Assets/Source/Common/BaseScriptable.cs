using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common.Structures;
using UnityEngine;

namespace NineBitByte.Common
{
  public abstract class BaseScriptable : ScriptableObject
  {
    /// <summary> Method to be invoked on startup, before anyone uses the scriptable. </summary>
    public virtual void OnStartup()
    {
      
    }
    
    // protected TBehavior AttachTemplate<TBehavior>(GameObject template, Transform parent, PositionAndRotation initialLocation)
    // where TBehavior : BaseProgrammableBehavior<>
    // {
    //   var instance = template.CreateInstance(
    //     parent,
    //     initialLocation
    //     );
    //
    //   var weaponBehavior = instance
    //     .GetComponent<TBehavior>()
    //     .Init(this);
    //
    //   // TODO
    //   // instance.transform.localPosition -= weaponBehavior.HeldPosition.Offset;
    //   return weaponBehavior;
    // }
  }
}