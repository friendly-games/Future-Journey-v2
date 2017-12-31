using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;

namespace NineBitByte.FutureJourney.Items
{
  public class PlaceableBehavior : BaseProgrammableBehavior<Buildable>
  {
    public virtual void Initialize(Buildable programming, IOwner owner, GridCoordinate coordinate)
    {
      Initialize(programming);
    }
  }
}