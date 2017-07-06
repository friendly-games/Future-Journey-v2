using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.Items
{
  public class SpawnItemBehavior : BaseBehavior
  {
    public Creature ItemToSpawn;

    public Allegiance ItemAllegiance;

    public RelativeOffset[] Offsets;

    public void Start()
    {
      foreach (var offset in Offsets)
      {
        var initialPos = offset.ToLocation(transform);
        ItemToSpawn.Build(initialPos, ItemAllegiance);
      }
    }
  }
}
