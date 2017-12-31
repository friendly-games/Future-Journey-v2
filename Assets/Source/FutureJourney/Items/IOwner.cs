using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;

namespace NineBitByte.FutureJourney.Items
{
  public interface IOwner
  {
    Allegiance Allegiance { get; }

    WorldGrid AssociatedGrid { get; }
  }
}