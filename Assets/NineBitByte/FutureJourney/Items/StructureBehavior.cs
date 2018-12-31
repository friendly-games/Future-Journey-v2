using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;

namespace NineBitByte.FutureJourney.Items
{
  public class StructureBehavior : BaseProgrammableBehavior<StructureDescriptor>
  {
    public virtual StructureBehavior Initialize(StructureDescriptor descriptor, IOwner owner, GridCoordinate coordinate)
    {
      Initialize(descriptor);
      return this;
    }
  }
}