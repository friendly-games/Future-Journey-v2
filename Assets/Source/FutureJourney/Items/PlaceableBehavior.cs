using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;

namespace NineBitByte.FutureJourney.Items
{
  public class PlaceableBehavior : BaseProgrammableBehavior<Placeable>
  {
    public virtual PlaceableBehavior Initialize(Placeable programming, IOwner owner, GridCoordinate coordinate)
    {
      Initialize(programming);
      return this;
    }
  }
}