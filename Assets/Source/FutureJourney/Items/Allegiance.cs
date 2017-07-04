using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source.Common;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> Represents a single team that is working together. </summary>
  [CreateAssetMenu(menuName = "Items/Allegiance")]
  public class Allegiance : BaseScriptable
  {
    public Layer AssociatedLayer;
  }
}