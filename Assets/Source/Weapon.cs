using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source
{
  [CreateAssetMenu(menuName = "Weapon")]
  public class Weapon : BaseScriptable
  {
    public string Name;
    public int DamagePerShot = 10;

    [Range(1, 20)]
    public int NumberOfPelots = 1;

    [Range(0, 1)]
    public float Spread = .75f;

    [Range(1, 150)]
    public int ClipSize = 1;

    public GameObject WeaponTemplate;
    public GameObject AmmoTemplate;

    public void Fire(GameObject instance)
    {
      
    }
  }
}