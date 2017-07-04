using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NineBitByte.Assets.Source.Common.Behaviors
{
  public class DestroyOnLoad : BaseBehavior
  {
    public void Awake()
    {
      gameObject.SetActive(false);
    }


  }
}
