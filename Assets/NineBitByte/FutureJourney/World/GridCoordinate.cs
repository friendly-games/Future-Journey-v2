﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Represents a position in the world grid. </summary>
  [Serializable]
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


    public GridCoordinate(Vector3 position)
      : this(ConvertX(position.x), ConvertY(position.y))
    {
    }

    public GridCoordinate(Vector2 position)
      : this(ConvertX(position.x), ConvertY(position.y))
    {
    }

    /// <summary> Normalize the given position to be aligned to the grid. </summary>
    public static Vector3 NormalizeToGrid(Vector3 position) 
      => (Vector3)(GridCoordinate)position;

    /// <summary> Create a new GridCoordinate that adds the given offset to this coordinate. </summary>
    public GridCoordinate OffsetBy(int x, int y) 
      => new GridCoordinate(X + x, Y + y);

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
      => left.Equals(right);

    public static bool operator !=(GridCoordinate left, GridCoordinate right)
      => !left.Equals(right);

    public static GridCoordinate operator +(GridCoordinate left, GridCoordinate right)
      => new GridCoordinate(left.X + right.X, left.Y + right.Y);
    
    public static GridCoordinate operator-(GridCoordinate left, GridCoordinate right)
      => new GridCoordinate(left.X - right.X, left.Y - right.Y);
    
    /// <inheritdoc />
    public override string ToString()
      => X + "," + Y;

    public (ChunkCoordinate ChunkCoordinate, InnerChunkGridCoordinate InnerCoordinate) AsSubCoordinates()
      => (ChunkCoordinate, InnerChunkGridCoordinate);

    public void Deconstruct(out ChunkCoordinate chunkCoordinate,
                            out InnerChunkGridCoordinate innerCoordinate)
    {
      chunkCoordinate = ChunkCoordinate;
      innerCoordinate = InnerChunkGridCoordinate;
    }
    
    /// <summary> Convert the y coordinate to a grid coordinate. </summary>
    private static int ConvertY(float y)
      => (int)Math.Floor(y * WorldToGridMultiplier);

    /// <summary> Convert the y coordinate to a grid coordinate. </summary>
    private static int ConvertX(float x)
      => (int)Math.Floor(x * WorldToGridMultiplier);

    /// <summary> Explicit cast that converts the given GridCoordinate to a Vector3. </summary>
    public static explicit operator Vector3(GridCoordinate coordinate)
      => coordinate.ToVector3();

    /// <summary> Explicit cast that converts the given Vector3 to a GridCoordinate. </summary>
    public static explicit operator GridCoordinate(Vector3 position)
      => new GridCoordinate(position);

    /// <summary> Explicit cast that converts the given Vector2 to a GridCoordinate. </summary>
    public static explicit operator GridCoordinate(Vector2 position)
      => new GridCoordinate(position);
  }

}