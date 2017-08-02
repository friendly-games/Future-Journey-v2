using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.FutureJourney.World
{
  /// <summary>
  ///  Generates the chunks for the world.  In the future, it should load the chunk from memory.
  /// </summary>
  public class ChunkLoader
  {
    public static Chunk LoadChunk(ChunkCoordinate chunkCoordinate)
    {
      var chunk = new Chunk(chunkCoordinate);

      for (int y = 0; y < Chunk.NumberOfGridItemsHigh; y++)
      {
        for (int x = 0; x < Chunk.NumberOfGridItemsWide; x++)
        {
          var gridPosition = new GridCoordinate(chunkCoordinate, new InnerChunkGridCoordinate(x, y));
          var gridItem = GetGridItemAt(gridPosition);

          chunk[gridPosition.InnerChunkGridCoordinate] = gridItem;
        }
      }

      return chunk;
    }

    private static GridItem GetGridItemAt(GridCoordinate gridPosition)
    {
      int x = gridPosition.X;
      int y = gridPosition.Y;

      // TODO UNITY
      // TODO load this from somewhere else
      var tileValue = GetTileIndex(x, y);

      // TODO UNITY
      // TODO remove random call
      byte variant = GetRotation(x, y);
      GridItem gridItem = new GridItem(tileValue, variant);

      return gridItem;
    }

    private static int GetTileIndex(int x, int y)
    {
      var noise = Mathf.PerlinNoise(
        30 + (10 + x) / 4.0f,
        30 + (10 + y) / 4.0f);
      int tileIndex = noise < 0.70f ? 0 : 1;
      return tileIndex;
    }

    private static byte GetRotation(int x, int y)
    {
      var noise = Mathf.PerlinNoise(
        30 + x / 2.0f,
        30 + y / 2.0f);
      return (byte)(noise * 4);
    }
  }
}