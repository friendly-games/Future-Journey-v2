using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Programming;

namespace NineBitByte.FutureJourney.World
{
  public class WorldGrid
  {
    // TODO make this bigger (bigger makes it slower to start)
    public const int NumberOfChunksWide = 4;

    public const int NumberOfChunksHigh = 4;

    private readonly TileType[] _tileTypes;
    private readonly Chunk[] _chunks;

    public WorldGrid(TileType[] tileTypes)
    {
      _tileTypes = tileTypes;
      _chunks = new Chunk[NumberOfChunksHigh * NumberOfChunksWide];

      for (int y = 0; y < NumberOfChunksHigh; y++)
      {
        for (int x = 0; x < NumberOfChunksWide; x++)
        {
          var chunkCoordinate = new ChunkCoordinate(x, y);
          this[chunkCoordinate] = ChunkLoader.LoadChunk(chunkCoordinate);
        }
      }
    }

    /// <summary>
    ///  Returns true if the given coordinate is a valid coordinate that can be used in this grid.
    /// </summary>
    public bool IsValid(ChunkCoordinate chunkCoordinate)
    {
      return chunkCoordinate.X >= 0 && chunkCoordinate.X < NumberOfChunksWide
             && chunkCoordinate.Y >= 0 && chunkCoordinate.Y < NumberOfChunksWide;
    }

    /// <summary>
    ///  Gets the chunk at the specified coordinate.
    /// </summary>
    public Chunk this[ChunkCoordinate coordinate]
    {
      get { return _chunks[CalculateAbsoluteChunkIndex(coordinate.X, coordinate.Y)]; }
      set { _chunks[CalculateAbsoluteChunkIndex(coordinate.X, coordinate.Y)] = value; }
    }

    internal static int CalculateAbsoluteChunkIndex(int x, int y)
      => x + y * NumberOfChunksWide;
  }

  public struct GridItem
  {
    public GridItem(int type, byte rotation)
    {
      Type = type;
      Rotation = rotation;
    }

    public int Type { get; }

    // 0-3 for all possible rotations
    public byte Rotation { get; }
  }
}