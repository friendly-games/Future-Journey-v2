using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.World
{
  /// <summary>
  ///  Data that is stored for each unit inside of a <see cref="WorldGridSlice{T}"/>.
  /// </summary>
  /// <typeparam name="T"> The type of additional data stored by the consumer of the
  ///  <see cref="WorldGridSlice{T}"/>. </typeparam>
  public struct SliceUnitData<T>
  {
    public SliceUnitData(Chunk chunk, GridCoordinate position, GridItem gridItem, T data)
    {
      Chunk = chunk;
      Position = position;
      GridItem = gridItem;
      Data = data;
    }

    /// <summary> The chunk that the datum belongs to. </summary>
    public readonly Chunk Chunk;

    /// <summary> The position of the unit in the WorldGrid for which this data is valid.. </summary>
    public readonly GridCoordinate Position;

    /// <summary> The actual GridItem data stored here. </summary>
    public readonly GridItem GridItem;

    /// <summary> The unit specific data. </summary>
    public T Data;
  }
}