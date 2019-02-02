using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NineBitByte.Common.Statistics;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  public interface IOwner
  {
    Allegiance Allegiance { get; }

    WorldGrid AssociatedGrid { get; }
    
    [NotNull]
    IStatisticContainer Statistics { get; }
  }
}