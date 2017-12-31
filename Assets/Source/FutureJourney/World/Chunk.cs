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
      get { return _items[CalculateIndex(coordinate)]; }
      set { new GridCellReference(this, CalculateIndex(coordinate)).Set(value); }
    }

    internal void UpdateItem<T>(InnerChunkGridCoordinate coordinate,
                                UpdateGridItemPropertyCallback<T> callback,
                                GridItemPropertyChange changeType,
                                T value)
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

    private void OnGridItemChanged(Chunk chunk,
                                   GridCoordinate coordinate,
                                   GridItem oldValue,
                                   GridItem newValue,
                                   GridItemPropertyChange changeType)
      => GridItemChanged?.Invoke(chunk, coordinate, oldValue, newValue, changeType);

    /// <summary> Calculates the array index for the given x/y coordinate </summary>
    private static int CalculateIndex(int x, int y)
      => x + y * NumberOfGridItemsWide;

    /// <summary> Calculates the array index for the given coordinate </summary>
    private static int CalculateIndex(InnerChunkGridCoordinate coordinate)
      => CalculateIndex(coordinate.X, coordinate.Y);

    private static InnerChunkGridCoordinate GetCoordinate(int absoluteIndex)
    {
      int x = absoluteIndex % NumberOfGridItemsWide;
      int y = (absoluteIndex - x) / NumberOfGridItemsWide;

      return new InnerChunkGridCoordinate(x, y);
    }

    /// <summary> Gets a cell reference that can be used to get and retrieve the value of the given cell. </summary>
    /// <param name="coordinate"> The coordinate representing the cell to retrieve. </param>
    /// <returns> The cell reference. </returns>
    public GridCellReference GetCellReference(InnerChunkGridCoordinate coordinate)
      => new GridCellReference(this, CalculateIndex(coordinate.X, coordinate.Y));

    /// <summary> Allows getting and setting a <see cref="GridItem"/> in a more efficient manner. </summary>
    public struct GridCellReference
    {
      private readonly Chunk _chunk;
      private readonly int _cellIndex;

      internal GridCellReference(Chunk chunk, int cellIndex)
      {
        _chunk = chunk;
        _cellIndex = cellIndex;
      }

      /// <summary> Gets the <see cref="GridItem"/> value associated with the cell. </summary>
      public GridItem Get()
        => _chunk._items[_cellIndex];

      /// <summary> Sets the <see cref="GridItem"/> value associated with the cell </summary>
      /// <param name="value"> The value to set the grid item to. </param>
      /// <param name="changeType"> (Optional) The type of the change that was made. </param>
      public void Set(GridItem value, GridItemPropertyChange changeType = GridItemPropertyChange.All)
      {
        var arr = _chunk._items;
        var oldValue = arr[_cellIndex];
        arr[_cellIndex] = value;

        FireChanged(oldValue, value, changeType);
      }

      /// <summary> Sets the health of the given item to the given value. </summary>
      /// <param name="health"> The health of the given item. </param>
      public void UpdateHealth(int health) 
        => UpdateItem((ref GridItem it, int value) => it.Health = value, GridItemPropertyChange.HealthChange, health);

      /// <summary> Removes the object associated with the given GridItem. </summary>
      public void ClearObject() 
        => UpdateItem((ref GridItem item, object _) => item = item.WithoutObject(), GridItemPropertyChange.All, null);

      private void UpdateItem<T>(UpdateGridItemPropertyCallback<T> callback,
                                  GridItemPropertyChange changeType,
                                  T value)
      {
        var arr = _chunk._items;
        var oldValue = arr[_cellIndex];
        callback.Invoke(ref arr[_cellIndex], value);
        var newValue = arr[_cellIndex];

        FireChanged(oldValue, newValue, changeType);
      }

      private void FireChanged(GridItem oldValue, GridItem newValue, GridItemPropertyChange changeType)
      {
        var innerCoordinate = GetCoordinate(_cellIndex);

        _chunk.OnGridItemChanged(_chunk,
                                 new GridCoordinate(_chunk.Position, innerCoordinate),
                                 oldValue,
                                 newValue,
                                 changeType);
      }
    }
  }
}