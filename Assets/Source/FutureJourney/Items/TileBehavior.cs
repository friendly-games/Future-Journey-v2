using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.Items
{
  public class TileBehavior : BaseBehavior
  {
    private TileType _tileType;
    
    public void Initialize(TileType tileType)
    {
      _tileType = tileType;
    }
  }
}