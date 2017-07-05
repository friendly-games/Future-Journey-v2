using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineBitByte.Assets.Source_Project;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  public class SpawnItemBehavior : BaseBehavior
  {
    public EnemyTemplate ItemToSpawn;

    public Allegiance ItemAllegiance;

    [RelativeOffset]
    public Vector3[] Offsets;

    public void Start()
    {
      foreach (var offset in Offsets)
      {
        ItemToSpawn.Build(offset + transform.position, ItemAllegiance);
      }

    }
  }
}
