using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineBitByte.Common.Behaviors
{
  public class DestroyOnLoad : BaseBehavior
  {
    public void Awake()
    {
      gameObject.SetActive(false);
    }


  }
}
