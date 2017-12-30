using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Represents a position in the world grid. </summary>
  public struct GridCoordinate : IEquatable<GridCoordinate>
  {
    public int X;
    public int Y;

    public const int WorldToGridMultiplier = 2;
    public const float WorldToGridMultiplierF = 2.0f;

    public GridCoordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public GridCoordinate(ChunkCoordinate chunkCoordinate, InnerChunkGridCoordinate innerCoordinate)
    {
      X = (chunkCoordinate.X << Chunk.XGridCoordinateToChunkCoordinateBitShift) + innerCoordinate.X;
      Y = (chunkCoordinate.Y << Chunk.YGridCoordinateToChunkCoordinateBitShift) + innerCoordinate.Y;
    }

    public GridCoordinate(Vector2 position)
    {
      X = (int)Math.Floor(position.x * WorldToGridMultiplier);
      Y = (int)Math.Floor(position.y * WorldToGridMultiplier);
    }

    /// <summary> Create a new GridCoordinate that adds the given offset to this coordinate. </summary>
    public GridCoordinate OffsetBy(int x, int y)
    {
      return new GridCoordinate(X + x, Y + y);
    }

    /// <summary> Represents the center of the grid, in world coordinates. </summary>
    [Pure]
    public Vector2 ToVector3()
    {
      return new Vector2(X / WorldToGridMultiplierF, Y / WorldToGridMultiplierF);
    }

    /// <summary>
    ///  The coordinate within the chunk identified by
    ///  <see cref="ChunkCoordinate"/> that this GridCoordinate represents.
    /// </summary>
    public InnerChunkGridCoordinate InnerChunkGridCoordinate
    {
      get
      {
        return new InnerChunkGridCoordinate(X & Chunk.GridItemsXCoordinateBitmask,
                                            Y & Chunk.GridItemsYCoordinateBitmask);
      }
    }

    /// <summary> The coordinate of the chunk that this GridCoordinate represents. </summary>
    public ChunkCoordinate ChunkCoordinate
    {
      get
      {
        return new ChunkCoordinate(X >> Chunk.XGridCoordinateToChunkCoordinateBitShift,
                                   Y >> Chunk.YGridCoordinateToChunkCoordinateBitShift);
      }
    }

    public bool Equals(GridCoordinate other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      return obj is GridCoordinate && Equals((GridCoordinate)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Y;
      }
    }

    public static bool operator ==(GridCoordinate left, GridCoordinate right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(GridCoordinate left, GridCoordinate right)
    {
      return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return X + "," + Y;
    }

    public void Deconstruct(out ChunkCoordinate chunkCoordinate,
                            out InnerChunkGridCoordinate innerCoordinate)
    {
      chunkCoordinate = ChunkCoordinate;
      innerCoordinate = InnerChunkGridCoordinate;
    }
  }
}