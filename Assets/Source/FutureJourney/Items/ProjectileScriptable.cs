using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  [CreateAssetMenu(menuName = "Items/Projectile")]
  public class ProjectileScriptable : BaseScriptable
  {
    public int BaseDamage;

    public GameObject ProjectileTemplate;

    public void CreateInstance()
    {
      
    }
  }
}