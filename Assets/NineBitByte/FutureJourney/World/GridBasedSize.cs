using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Represents the size of an item in the world grid. </summary>
  [Serializable]
  public struct GridBasedSize : IEquatable<GridBasedSize>
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

    public bool Equals(GridBasedSize other)
      => Width == other.Width && Height == other.Height;

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      return obj is GridBasedSize other && Equals(other);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (Width * 397) ^ Height;
      }
    }

    public static bool operator ==(GridBasedSize left, GridBasedSize right)
      => left.Equals(right);

    public static bool operator !=(GridBasedSize left, GridBasedSize right)
      => !left.Equals(right);

    public override string ToString()
      => $"Width={Width}, Height={Height}";
  }
}