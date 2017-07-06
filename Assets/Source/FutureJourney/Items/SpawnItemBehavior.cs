using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class SpawnItemBehavior : BaseBehavior
  {
    public EnemyTemplate ItemToSpawn;

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
