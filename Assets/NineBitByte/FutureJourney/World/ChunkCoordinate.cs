﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> A coordinate to a chunk. </summary>
  public struct ChunkCoordinate : IEquatable<ChunkCoordinate>
  {
    public int X;
    public int Y;

    public ChunkCoordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public static ChunkCoordinate Invalid { get; }
      = new ChunkCoordinate(-1, -1);

    public bool Equals(ChunkCoordinate other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return obj is ChunkCoordinate && Equals((ChunkCoordinate)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Y;
      }
    }

    public static bool operator ==(ChunkCoordinate left, ChunkCoordinate right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ChunkCoordinate left, ChunkCoordinate right)
    {
      return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return X + "," + Y;
    }
  }
}