using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary> Represents a single team that is working together. </summary>
  [CreateAssetMenu(menuName = "Items/Allegiance")]
  public class Allegiance : BaseScriptable
  {
    [Tooltip("The layer that should be assigned when an object from this allegiance is created")]
    public Layer AssociatedLayer;

    [Tooltip("The layers that are damaged by this team")]
    public LayerMask DamageMask;

    public bool DoesDamageTo(Allegiance allegiance) 
      => allegiance.AssociatedLayer.IsIn(DamageMask);
  }
}