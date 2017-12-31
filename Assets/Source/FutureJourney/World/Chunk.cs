using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.World
{
  /// <summary> Callback to be invoked when a grid item changes. </summary>
  public delegate void GridItemChangedCallback(
    Chunk chunk,
    GridCoordinate coordinate,
    GridItem oldValue,
    GridItem newValue,
    GridItemPropertyChange changeType
    );

  public delegate void UpdateGridItemPropertyCallback<T>(ref GridItem gridItem, T value);

  public static class ChunkExtensions
  {
    public static void UpdateHealth(this Chunk chunk, InnerChunkGridCoordinate coordinate, int health) 
      => chunk.UpdateItem(coordinate, (ref GridItem it, int value) => it.Health = value, health, GridItemPropertyChange.Health);
  }

  /// <summary>
  ///  Represents a square portion of the map containing the tiles.
  /// </summary>
  public class Chunk
  {
    /// <summary>
    ///  How many bits to shift a <see cref="GridCoordinate"/> to get the
    ///  <see cref="ChunkCoordinate"/>.
    /// </summary>
    public const int XGridCoordinateToChunkCoordinateBitShift = 6;

    /// <summary>
    ///  How many bits to shift a <see cref="GridCoordinate"/> to get the
    ///  <see cref="ChunkCoordinate"/>.
    /// </summary>
    public const int YGridCoordinateToChunkCoordinateBitShift = XGridCoordinateToChunkCoordinateBitShift;

    /// <summary> The number of GridItems wide in each chunk. </summary>
    public const int NumberOfGridItemsWide = 1 << XGridCoordinateToChunkCoordinateBitShift;

    /// <summary> The number of GridItems high in each chunk. </summary>
    public const int NumberOfGridItemsHigh = 1 << YGridCoordinateToChunkCoordinateBitShift;

    /// <summary>
    ///  The bits of a <see cref="GridCoordinate"/> that represent the
    ///  <see cref="InnerChunkGridCoordinate"/>
    /// </summary>
    public const int GridItemsXCoordinateBitmask = NumberOfGridItemsWide - 1;

    /// <summary>
    ///  The bits of a <see cref="GridCoordinate"/> that represent the
    ///  <see cref="InnerChunkGridCoordinate"/>
    /// </summary>
    public const int GridItemsYCoordinateBitmask = NumberOfGridItemsHigh - 1;

    /// <summary> All of the items that exist in the grid. </summary>
    private readonly GridItem[] _items;

    /// <summary> Constructor. </summary>
    /// <param name="position"> The position of the given chunk. </param>
    public Chunk(ChunkCoordinate position)
    {
      Position = position;
      _items = new GridItem[NumberOfGridItemsWide * NumberOfGridItemsHigh];

      AbsoluteIndex = WorldGrid.CalculateAbsoluteChunkIndex(Position.X, Position.Y);
    }

    /// <summary> The position of the given chunk. </summary>
    public ChunkCoordinate Position { get; set; }

    /// <summary> The absolute index of the chunk within the world. </summary>
    public int AbsoluteIndex { get; }

    /// <summary>
    ///  Indexer to get the GridItem at the specified coordinates.
    /// </summary>
    /// <param name="coordinate"> The position at which the item should be set or gotten.. </param>
    /// <returns> The GridItem at the specified position. </returns>
    public GridItem this[InnerChunkGridCoordinate coordinate]
    {
      get { return _items[CalculateIndex(coordinate.X, coordinate.Y)]; }
      set
      {
        var oldValue = this[coordinate];
        _items[CalculateIndex(coordinate.X, coordinate.Y)] = value;
        OnGridItemChanged(this, new GridCoordinate(Position, coordinate), oldValue, value, GridItemPropertyChange.All);
      }
    }

    internal void UpdateItem<T>(InnerChunkGridCoordinate coordinate, UpdateGridItemPropertyCallback<T> callback, T value, GridItemPropertyChange changeType)
    {
      var index = CalculateIndex(coordinate.X, coordinate.Y);

      var oldValue = _items[index];
      callback.Invoke(ref _items[index], value);
      var newValue = _items[index];

      OnGridItemChanged(this, new GridCoordinate(Position, coordinate), oldValue, newValue, changeType);
    }

    /// <summary>
    ///  Event that occurs when a grid item changes.
    /// </summary>
    public event GridItemChangedCallback GridItemChanged;

    private void OnGridItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldValue, GridItem newValue, GridItemPropertyChange changeType)
    {
      var handler = GridItemChanged;
      if (handler != null)
        handler(chunk, coordinate, oldValue, newValue, changeType);
    }

    private int CalculateIndex(int x, int y)
    {
      return x + y * NumberOfGridItemsWide;
    }
  }
}