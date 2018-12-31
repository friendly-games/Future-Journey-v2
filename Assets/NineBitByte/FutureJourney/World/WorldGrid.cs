using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Holds all tiles that exist in the system. </summary>
  public class WorldGrid
  {
    // TODO make this bigger (bigger makes it slower to start)
    public const int NumberOfChunksWide = 4;

    public const int NumberOfChunksHigh = 4;

    private readonly Chunk[] _chunks;

    public WorldGrid()
    {
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
    public ref Chunk this[ChunkCoordinate coordinate]
      => ref _chunks[CalculateAbsoluteChunkIndex(coordinate.X, coordinate.Y)];

    /// <summary> Get the chunk and grid item data at the specified position. </summary>
    public (Chunk, GridItem) GetChunkAndCellData((ChunkCoordinate chunkCoordinate, InnerChunkGridCoordinate innerCoordinate) subCoordinate)
    {
      return GetChunkAndCellData(subCoordinate.chunkCoordinate, subCoordinate.innerCoordinate);
    }
    
    /// <summary> Get the chunk and grid item data at the specified position. </summary>
    public (Chunk, GridItem) GetChunkAndCellData(GridCoordinate coordinate)
    {
      var (chunkCoordinate, innerCoordinate) = coordinate;
      return GetChunkAndCellData(chunkCoordinate, innerCoordinate);
    }
    
    /// <summary> Get the chunk and grid item data at the specified position. </summary>
    public (Chunk, GridItem) GetChunkAndCellData(ChunkCoordinate chunkCoordinate, InnerChunkGridCoordinate innerCoordinate)
    {
      var chunk = this[chunkCoordinate];
      var data = chunk[innerCoordinate];
      return (chunk, data);
    }

    /// <summary> Gets a cell reference to the cell at the given coordinate. </summary>
    public Chunk.GridCellReference this[GridCoordinate coordinate]
    {
      get
      {
        ChunkCoordinate chunkCoordinate;
        InnerChunkGridCoordinate innerCoordinate;

        coordinate.Deconstruct(out chunkCoordinate, out innerCoordinate);

        var chunk = _chunks[CalculateAbsoluteChunkIndex(chunkCoordinate.X, chunkCoordinate.Y)];
        return chunk.GetCellReference(innerCoordinate);
      }
    }

    internal static int CalculateAbsoluteChunkIndex(int x, int y)
      => x + y * NumberOfChunksWide;
  }
}