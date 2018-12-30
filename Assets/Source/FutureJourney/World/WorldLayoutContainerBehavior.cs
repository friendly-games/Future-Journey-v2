using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

namespace NineBitByte.FutureJourney.World
{
  public class WorldLayoutContainerBehavior : BaseBehavior
  {
    [Tooltip("The layout that should be edited when this container is modified")]
    public WorldLayout AssociatedLayout;
  }
}