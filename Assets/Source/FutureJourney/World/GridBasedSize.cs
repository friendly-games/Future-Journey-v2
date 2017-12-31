using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Represents the size of an item in the world grid. </summary>
  [Serializable]
  public struct GridBasedSize
  {
    public int Width;

    public int Height;

    public bool IsValid
      => Width > 0 && Height > 0;

    public GridBasedSize(int width, int height)
    {
      Width = width;
      Height = height;
    }
  }
}