using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using NineBitByte.Common.Structures;
using NineBitByte.Common.Util;
using Debug = UnityEngine.Debug;

namespace NineBitByte.FutureJourney.World
{
  /// <summary>
  ///  Takes a portion of the WorldGrid and watches a movable rectangle of units in the world for
  ///  changes.  Useful for watching the change around the player as he moves.
  /// </summary>
  /// <remarks>
  ///  This class uses a rolling buffer to only update the tiles that changed.  This means that when
  ///  the player moves right, the left-most row is removed and replaced with the row that came into
  ///  view on the right.
  /// </remarks>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public sealed class WorldGridSlice<T>
  {
    private readonly WorldGrid _worldGrid;
    private readonly int _numUnitsWideHalfThreshold;
    private readonly int _numUnitsHighHalfThreshold;

    private readonly Array2D<SliceUnitData<T>> _visibleGridItems;
    private GridCoordinate _currentPosition;
    private Array2DIndex _viewCurrentPosition;
    private readonly ChunkTracker _chunkTracker;

    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="worldGrid"> The world grid which should provide the grid data to be watched. </param>
    /// <param name="numUnitsWide"> How many units wide the area that should be watched. </param>
    /// <param name="numUnitsHigh"> How many units high the area that should be watched. </param>
    public WorldGridSlice(WorldGrid worldGrid, int numUnitsWide, int numUnitsHigh)
    {
      if (worldGrid == null)
        throw new ArgumentNullException(nameof(worldGrid));

      numUnitsWide *= GridCoordinate.WorldToGridMultiplier;
      numUnitsHigh *= GridCoordinate.WorldToGridMultiplier;

      // we always need to be odd (isn't that a statement of life)
      MathUtil.MakeOdd(ref numUnitsWide);
      MathUtil.MakeOdd(ref numUnitsHigh);

      // let's make sure we always have valid values
      numUnitsWide = Math.Max(numUnitsWide, 5);
      numUnitsHigh = Math.Max(numUnitsHigh, 5);

      _worldGrid = worldGrid;
      _visibleGridItems = new Array2D<SliceUnitData<T>>(numUnitsWide, numUnitsHigh);
      _chunkTracker = new ChunkTracker();

      // these are used for converting an array index back into a world coordinate
      _numUnitsWideHalfThreshold = (_visibleGridItems.Width - 1) / 2;
      _numUnitsHighHalfThreshold = (_visibleGridItems.Height - 1) / 2;
    }

    public Array2D<SliceUnitData<T>> VisibleGridItems
      => _visibleGridItems;

    /// <summary>
    ///  Reads data from the world grid.  Not done in the constructor to allow callers to subscribe to
    ///  the events that will be fired.
    /// </summary>
    /// <param name="position"> The position. </param>
    public void Initialize(GridCoordinate position)
    {
      _currentPosition = position;
      _viewCurrentPosition = ConvertToGridItemIndex(position);

      InitializeUnits();
    }

    /// <summary> Initialize each grid unit. </summary>
    private void InitializeUnits()
    {
      for (var y = 0; y < _visibleGridItems.Height; y++)
      {
        for (var x = 0; x < _visibleGridItems.Width; x++)
        {
          var index = new Array2DIndex(x, y);
          var coordinate = ConvertToGridCoordinate(index);

          var chunkCoordinate = coordinate.ChunkCoordinate;
          if (_worldGrid.IsValid(chunkCoordinate))
          {
            var chunk = _worldGrid[chunkCoordinate];
            var data = chunk[coordinate.InnerChunkGridCoordinate];

            UpdateData(index, data, coordinate, chunk, GridItemPropertyChange.All);
          }
        }
      }
    }

    /// <summary> Changes the position being observed. </summary>
    public void Recenter(GridCoordinate center)
    {
      var originalPosition = _currentPosition;

      _currentPosition = center;
      _viewCurrentPosition = ConvertToGridItemIndex(_currentPosition);

      UpdateChangedUnits(originalPosition);
    }

    /// <summary> Updates the cells that have changed because the target has changed. </summary>
    /// <param name="previousCoordinate"> The last position we were at. </param>
    private void UpdateChangedUnits(GridCoordinate previousCoordinate)
    {
      var xDiff = _currentPosition.X - previousCoordinate.X;
      var yDiff = _currentPosition.Y - previousCoordinate.Y;

      if (xDiff == 0 && yDiff == 0)
      {
        return;
      }

      if (Math.Abs(xDiff) >= _visibleGridItems.Width
          || Math.Abs(yDiff) >= _visibleGridItems.Height
      )
      {
        // if we ever move more units than we have, then just invalidate the whole thing. 
        UpdateAllUnits();
        return;
      }

      // if we're at (x,y) from (x-1, y-1), then (x+halfWidth, y+halfHeight) just came into view (of
      // course if we just came from (x+1, y+1), then (x-halfWidth, y-halfWidth) just came into view.

      // This used to be `amountToOffsetX = MathUtils.Signed(xDiff, _numUnitsWideHalfThreshold)` but
      // then when we moved right it would update the top-right corner one higher than when we moved
      // up. I'm not sure why we need to invalidate one higher when moving in the positive direction
      // but it seems to work.
      // 
      // My current theory as to why is that it has something to do with one of the conversion
      // algorithms preferring the positive direction over the negative. 
      var amountToOffsetX = xDiff > 0 ? _numUnitsWideHalfThreshold + 1 : -_numUnitsWideHalfThreshold;
      var amountToOffsetY = yDiff > 0 ? _numUnitsHighHalfThreshold + 1 : -_numUnitsHighHalfThreshold;

      var start = previousCoordinate.OffsetBy(amountToOffsetX, amountToOffsetY);
      var end = _currentPosition.OffsetBy(amountToOffsetX, amountToOffsetY);

      var startColumn = start.X;
      var endColumn = end.X;

      // make sure our for loop will work :: )
      if (startColumn > endColumn)
      {
        MathUtil.Swap(ref startColumn, ref endColumn);
      }

      var startRow = start.Y;
      var endRow = end.Y;

      if (startRow > endRow)
      {
        MathUtil.Swap(ref startRow, ref endRow);
      }

      // OPTIMIZATION 1 - Instead of using PositiveRemainder, we could just have loop through twice
      // (when needed) because we know that we can only have to dis-jointed sets of rows[/columns]
      // 
      // OPTIMIZATION 2 - We're currently always hitting the area where the rows/columns overlap
      // twice.  We could optimize this out (especially if/when we do #1 above). 

      // update the rows that need to be updated
      for (var y = startRow; y < endRow; y++)
      {
        for (var x = _currentPosition.X - _numUnitsWideHalfThreshold;
             x <= _currentPosition.X + _numUnitsWideHalfThreshold;
             x++)
        {
          InvalidatePosition(new GridCoordinate(x, y));
        }
      }

      // update that columns that need to be updated
      for (var y = _currentPosition.Y - _numUnitsHighHalfThreshold;
           y <= _currentPosition.Y + _numUnitsHighHalfThreshold;
           y++)
      {
        for (var x = startColumn; x < endColumn; x++)
        {
          InvalidatePosition(new GridCoordinate(x, y));
        }
      }
    }

    /// <summary> Updates every item in our viewable grid. </summary>
    private void UpdateAllUnits()
    {
      for (var y = 0; y < _visibleGridItems.Height; y++)
      {
        for (var x = 0; x < _visibleGridItems.Width; x++)
        {
          InvalidateIndex(new Array2DIndex(x, y));
        }
      }
    }

    private void Validate(GridCoordinate coordinate)
    {
      var index = ConvertToGridItemIndex(coordinate);
      var backAgain = ConvertToGridCoordinate(index);

      if (backAgain != coordinate)
      {
        var builder = new StringBuilder();
        builder.AppendFormat("Expected: {0}, but got {1}", coordinate, backAgain);
        Debug.Log(builder);
      }
    }

    /// <summary>
    ///  Checks the array index/position to verify that it has the correct position information and if
    ///  it does not, re-reads the data from the world and updates the data.
    /// </summary>
    /// <param name="coordinate"> The coordinate representing the position to update. </param>
    private void InvalidatePosition(GridCoordinate coordinate)
    {
      Validate(coordinate);
      Invalidate(ConvertToGridItemIndex(coordinate), coordinate);
    }

    /// <summary>
    ///  Checks the array index/position to verify that it has the correct position information and if
    ///  it does not, re-reads the data from the world and updates the data.
    /// </summary>
    /// <param name="index"> The index position to update. </param>
    private void InvalidateIndex(Array2DIndex index)
    {
      Invalidate(index, ConvertToGridCoordinate(index));
    }

    private void Invalidate(Array2DIndex index, GridCoordinate coordinate)
      => Invalidate(index, coordinate, bypassSamePositionCheck: false, changeType: GridItemPropertyChange.All);

    /// <summary>
    ///  Checks the array index/position to verify that it has the correct position information and if
    ///  it does not, re-reads the data from the world and updates the data.
    /// </summary>
    private void Invalidate(Array2DIndex index, GridCoordinate coordinate, bool bypassSamePositionCheck, GridItemPropertyChange changeType)
    {
      var existingData = _visibleGridItems[index];

      if (!bypassSamePositionCheck && existingData.Position == coordinate)
        return;

      var chunkCoordinate = coordinate.ChunkCoordinate;

      if (!_worldGrid.IsValid(chunkCoordinate))
        return;

      var chunk = _worldGrid[chunkCoordinate];
      var data = chunk[coordinate.InnerChunkGridCoordinate];

      UpdateData(index, data, coordinate, chunk, changeType);
    }

    /// <summary> Updates the data at the specified index inside the grid. </summary>
    /// <param name="index"> Zero-based index of the. </param>
    /// <param name="data"> The grid data to store. </param>
    /// <param name="position"> The coordinate of the original GridItem where the data was retrieved
    ///   from. </param>
    /// <param name="chunk"></param>
    /// <param name="changeType"></param>
    private void UpdateData(Array2DIndex index, GridItem data, GridCoordinate position, Chunk chunk, GridItemPropertyChange changeType)
    {
      var newData = new SliceUnitData<T>(chunk, position, data, default(T));
      var rawIndex = _visibleGridItems.CalculateRawArrayIndex(index);

      var oldData = _visibleGridItems.Data[rawIndex];
      _visibleGridItems.Data[rawIndex] = newData;

      DataChanged?.Invoke(oldData, ref _visibleGridItems.Data[rawIndex], changeType);
      MarkChunkChanged(oldData.Chunk, chunk);
    }

    private void MarkChunkChanged(Chunk oldChunk, Chunk newChunk)
    {
      if (_chunkTracker.StopTracking(oldChunk))
      {
        oldChunk.GridItemChanged -= HandleChunkItemChanged;
      }

      if (_chunkTracker.StartTracking(newChunk))
      {
        newChunk.GridItemChanged += HandleChunkItemChanged;
      }
    }

    private void HandleChunkItemChanged(Chunk chunk, GridCoordinate coordinate, GridItem oldvalue, GridItem newvalue, GridItemPropertyChange changeType)
    {
      Invalidate(ConvertToGridItemIndex(coordinate), coordinate, bypassSamePositionCheck: true, changeType: changeType);
    }

    public delegate void Callback(SliceUnitData<T> oldData, ref SliceUnitData<T> newData, GridItemPropertyChange changeType);

    public event Callback DataChanged;

    /// <summary>
    ///  Converts the given grid coordinate into a an index that can be used to get the data in
    ///  <see cref="_visibleGridItems"/>.
    /// </summary>
    /// <remarks>
    ///  Multiple GridCoordinates map to the same ArrayIndex, so this is technically an irreversible
    ///  operation. However, provided we always remain within the correct GridCoordinates (e.g. always
    ///  use grid coordinates that we generate or which are validated first), we should never really
    ///  have a situation where a conversion to an array index and back results in a different
    ///  coordinate.
    /// </remarks>
    /// <param name="coordinate"> The coordinate. </param>
    /// <returns> The given data converted to a grid item index. </returns>
    private Array2DIndex ConvertToGridItemIndex(GridCoordinate coordinate)
    {
      var correctedX = MathUtil.PositiveRemainder(coordinate.X, _visibleGridItems.Width);
      var correctedY = MathUtil.PositiveRemainder(coordinate.Y, _visibleGridItems.Height);

      var arrayIndex = new Array2DIndex(correctedX, correctedY);

      return arrayIndex;
    }

    /// <summary>
    ///  Converts an array index into the expected grid coordinate for the given index.
    /// </summary>
    /// <param name="arrayIndex"> Zero-based index of the array. </param>
    /// <returns> The given data converted to a grid coordinate. </returns>
    private GridCoordinate ConvertToGridCoordinate(Array2DIndex arrayIndex)
    {
      var difViewX = _viewCurrentPosition.X - arrayIndex.X;
      var signX = GetSign(difViewX);

      // Normalize the offset from the "current" view position.
      // 
      // This explanation just talks about X, the same theory holds for Y. Imagine a number line
      // ranging from 0 to maxViewX).
      // 
      // If the current position is on the right (so it's position is maxViewX), then the very left
      // position (position 0) of the view really just represents one world unit to the right of the
      // current position.  If we simply subtracted these two positions, however, it would be
      // reported it as a difference of -ViewWidth units away We want to convert it to a difference
      // of (1).
      // 
      // We know we have to do convert values if it's more than half the size away (that's the only
      // time that wrapping occurs when putting the values into the view). The conversion is fairly
      // straight forward when given examples:
      //  - When we're `maxViewX` view units away, we're really only 1 world unit away.
      //  - When we're `maxViewX - 1` units away we're really only 2 world units away.
      //  - When we're `maxViewX - N` units away, we're really only N world units away.
      // 
      // The pattern to find N (the world units away) is to take `ViewWidth` and "subtract"
      // `(maxViewX - N)`. `maxViewX - N` happens to be diffViewX, so then we just have to "subtract"
      // it from `ViewWidth`. "subtract" is in quotes because actually, when the difference is
      // negative, we want to add `ViewWidth` (otherwise we end up in very negative territory). So we
      // add when `maxViewX - N` is negative, subtract when `maxViewX - N` is positive. 
      var offsetX = Math.Abs(difViewX) > _numUnitsWideHalfThreshold
        ? difViewX + -signX * _visibleGridItems.Width
        : difViewX;

      // all of the above but now for Y
      var difViewY = _viewCurrentPosition.Y - arrayIndex.Y;
      var signY = difViewY >= 0 ? 1 : -1;

      var offsetY = Math.Abs(difViewY) > _numUnitsHighHalfThreshold
        ? difViewY + -signY * _visibleGridItems.Height
        : difViewY;

      var gridPosition = _currentPosition.OffsetBy(-offsetX, -offsetY);

      ValidateConversion(gridPosition, arrayIndex);

      return gridPosition;
    }

    private static int GetSign(int difViewX)
    {
      return GetSigned(difViewX, 1);
    }

    private static int GetSigned(int originalValue, int inverted)
    {
      return originalValue >= 0 ? inverted : -inverted;
    }

    /// <summary>
    ///  (Only available in DEBUG builds) Verifies that the conversion from an ArrayIndex to a
    ///  GridCoordinate was successful.
    /// </summary>
    [Conditional("DEBUG")]
    private void ValidateConversion(GridCoordinate gridPosition, Array2DIndex arrayIndex)
    {
      var worked = ConvertToGridItemIndex(gridPosition).X == arrayIndex.X
                   && ConvertToGridItemIndex(gridPosition).Y == arrayIndex.Y;

      if (worked)
        return;

      // to aid in debugging
      worked = ConvertToGridItemIndex(gridPosition).X == arrayIndex.X
               && ConvertToGridItemIndex(gridPosition).Y == arrayIndex.Y;

      var message = new StringBuilder();
      message.AppendFormat("Array Index ({0}) was converted into Grid Position ({1})", arrayIndex, gridPosition);
      message.AppendFormat("But we expected ({0},{1})",
                           ConvertToGridItemIndex(gridPosition).X,
                           ConvertToGridItemIndex(gridPosition).Y);

      Debug.Log(message);
    }

    /// <summary>
    ///  Maintains a reference count of how many times a chunk has been marked as tracked (and
    ///  untracked), indicating when its added/removed whether or not it is for the first or last
    ///  time. This allows callers to know when to add or to remove events from chunks.
    /// </summary>
    internal sealed class ChunkTracker
    {
      private readonly Dictionary<ChunkCoordinate, int> _chunkCount
        = new Dictionary<ChunkCoordinate, int>();

      /// <summary>
      ///  Adds the chunk to the collection, returning true if the chunk was added for the first time.
      /// </summary>
      public bool StartTracking([CanBeNull] Chunk chunk)
      {
        if (chunk == null)
          return false;

        var position = chunk.Position;
        int count;
        if (_chunkCount.TryGetValue(position, out count))
        {
          _chunkCount[position] = count + 1;
          return false;
        }

        _chunkCount[position] = 1;
        return true;
      }

      /// <summary>
      ///  Removes a chunk from the collection, returning true if the chunk was actually removed.
      /// </summary>
      public bool StopTracking([CanBeNull] Chunk chunk)
      {
        if (chunk == null)
          return false;

        var position = chunk.Position;
        int count;
        if (!_chunkCount.TryGetValue(position, out count))
          return false;

        if (count != 1)
        {
          _chunkCount[position] = count - 1;
          return false;
        }

        _chunkCount.Remove(position);
        return true;
      }
    }
  }
}