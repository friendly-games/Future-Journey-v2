using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Policy;
using NineBitByte.Common;
using NineBitByte.Common.Util;
using NineBitByte.FutureJourney.World;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace NineBitByte.FutureJourney.Programming
{
  /// <summary>
  ///   Contains a snapshot of part of a map, which can be imported into the world if need be.
  /// </summary>
  [CreateAssetMenu(menuName = "Items/World Layout")]
  public class WorldLayout : BaseScriptable
  {
    [Tooltip("The size of the layout")]
    public GridBasedSize Size;

    // ReSharper disable once InconsistentNaming
    [SerializeField, HideInInspector]
    private GridBasedSize _lastKnownSize;

    /// <summary>
    ///   All of the structures currently in use in <see cref="GridItems"/>.  The index of the structure in the array
    ///   corresponds to the <see cref="MapGridItem.StructureId"/> + 1 (so Structure[0] has a StructureId of 1)
    /// </summary>
    public List<StructureDescriptor> Structures
      = new List<StructureDescriptor>(0);

    /// <summary>
    ///   All of the tiles currently in use in <see cref="GridItems"/>.  The index of the tile in the array
    ///   corresponds to the <see cref="MapGridItem.TileId"/> + 1 (so Tiles[0] has a TileId of 1)
    /// </summary>
    public List<TileType> Tiles
      = new List<TileType>(0);
    
    [Tooltip("The tiles within the world")]
    public MapGridItem[] GridItems;
   
    /// <summary />
    private ref MapGridItem GetGridItem(GridCoordinate coordinate)
      => ref GridItems[GetFlatArrayIndex(Size, coordinate)];
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="coordinate"></param>
    public ref MapGridItem this[GridCoordinate coordinate]
      => ref GridItems[GetFlatArrayIndex(Size, coordinate)];

    /// <summary>
    ///  Sets the size, calls <see cref="Synchronize"/>, and updates the tiles within the map.
    /// </summary>
    public void Resize(GridBasedSize size)
    {
      Size = size;
      Synchronize();
    }

    /// <summary>
    ///   Verify that the layout size matches the expected layout size.  Should be called before accessing any of the
    /// <see cref="MapGridItem"/>s in this layout.
    /// </summary>
    public void Synchronize()
    {
      SyncSizes();
      ReIndex();
    }

    /// <summary>
    ///   Sets the structure at the given coordinate to the given structure value.
    /// </summary>
    public void SetStructure(GridCoordinate coordinate, StructureDescriptor structure)
    {
      Synchronize();
      
      ref var item = ref GetGridItem(coordinate);
      int oldId = item.StructureId;
      if (oldId != 0)
      {
        // this is the last known instance of this structure, so remove it from the list
        if (GridItems.Count(it => it.StructureId == oldId) == 1)
        {
          Structures[oldId - 1] = null;
        }
      }

      if (structure != null)
      {
        var structureId = GetIdOfItem(Structures, structure);
        GridItems[GetFlatArrayIndex(Size, coordinate)].StructureId = structureId;
      }
      else
      {
        item.StructureId = 0;
      }
    }

    /// <summary>
    ///   Sets the tile at the given coordinate to the given structure value.
    /// </summary>
    public void SetTile(GridCoordinate coordinate, TileType descriptor)
    {
      Synchronize();
      
      ref var item = ref GetGridItem(coordinate);

      int oldId = item.TileId;
      if (oldId != 0)
      {
        // this is the last known instance of this tile, so remove it from the list
        if (GridItems.Count(it => it.TileId == oldId) == 1)
        {
          Tiles[oldId - 1] = null;
        }
      }

      if (descriptor != null)
      {
        var tileId = GetIdOfItem(Tiles, descriptor);
        GridItems[GetFlatArrayIndex(Size, coordinate)].TileId = tileId;
      }
      else
      {
        item.TileId = 0;
      }
    }

    /// <summary>
    ///   Clears the tile at the given coordinate.
    /// </summary>
    public void ClearTile(GridCoordinate coordinate)
      => SetTile(coordinate, null);

    /// <summary>
    ///   Clears the tile at the given coordinate.
    /// </summary>
    public void ClearStructure(GridCoordinate coordinate)
      => SetStructure(coordinate, null);

    /// <summary>
    ///   Gets the id of the given element in the given list, adding it to the list if needed.
    ///
    ///   Note that the id of an element is equivalent to the index + 1, so for instance, if the element exists
    ///   at index 0 of <paramref name="list"/>, the returned id will be 1.
    /// </summary>
    private int GetIdOfItem<T>(List<T> list, T element)
      where T : class
    {
      // NOTE: the id is the index within the array + 1

      int existingIndex = list.IndexOf(element);

      if (existingIndex != -1)
        return existingIndex + 1;

      if (list.IndexOf(null) is int emptySlotIndex
          && emptySlotIndex != -1)
      {
        list[emptySlotIndex] = element;
        return emptySlotIndex + 1;
      }

      list.Add(element);
      return list.Count;
    }

    public void ImportIntoWorld(WorldGrid grid, GridCoordinate offset)
    {
      foreach (var relativeCoordinate in Size.AsCoordinateRange())
      {
        var gridItem = this[relativeCoordinate];
        if (gridItem.TileId != 0 || gridItem.StructureId != 0)
        {
          var absolutePosition = relativeCoordinate + offset;
          var reference = grid[absolutePosition];

          var newValue = reference.Value;
            
          if (gridItem.TileId != 0)
          {
            var tile = Tiles[gridItem.TileId - 1];
            newValue = tile.CreateGridItem();
            Debug.Log("Created tile");
          }

          if (gridItem.StructureId != 0)
          {
            var structure = Structures[gridItem.StructureId - 1];
            structure.AddStructure(ref newValue);
          }
          
          reference.Set(newValue);
        }
      }
    }

    /// <summary>
    ///   True if the given position is valid for the size in here.
    /// </summary>
    public bool IsValidPosition(Vector3Int position)
    {
      return position.x >= 0
             && position.x < Size.Width
             && position.y >= 0
             && position.y < Size.Width;
    }
    
    /// <summary>
    ///   Change the size of the map part to have the new size given by <paramref name="size"/>.
    /// </summary>
    private void SyncSizes()
    {
      if (Size == _lastKnownSize)
        return;

      var newSize = Size;
      
      // copy all of the data from the old array to the new array, by copying
      // each row, 1 by 1 
      var newArray = new MapGridItem[newSize.Width * newSize.Height];

      var copyWidth = Math.Min(_lastKnownSize.Width, newSize.Width);
      var copyHeight = Math.Min(_lastKnownSize.Height, newSize.Height);

      for (int y = 0; y < copyHeight; y++)
      {
        int xOldStart = GetFlatArrayIndex(_lastKnownSize, new GridCoordinate(0, y));
        int xNewStart = GetFlatArrayIndex(newSize, new GridCoordinate(0, y));

        if (copyWidth > 0)
        {
          Debug.Log($"xOldStart: {xOldStart}, xNewStart: {xNewStart}, copyWidth: {copyWidth}, array length: {GridItems.Length}");
          Array.Copy(GridItems, xOldStart, newArray, xNewStart, copyWidth);
        }
      }

      GridItems = newArray;
      Size = newSize;
      _lastKnownSize = newSize;
      
      Debug.Log($"Last: {_lastKnownSize}, Current: {Size}");
    }

    /// <summary>
    ///  Condense the size of <see cref="Structures"/> and <see cref="Tiles"/> by removing null structures and tiles
    ///  from their respective arrays and updating the indices inside of <see cref="GridItems"/>. 
    /// </summary>
    public void ReIndex()
    {
      var newStructures = new List<StructureDescriptor>();
      var newTiles = new List<TileType>();

      for (var i = 0; i < GridItems.Length; i++)
      {
        ref var gridItem = ref GridItems[i];

        // take the old structure, and remap it using the newest index
        if (gridItem.StructureId != 0)
        {
          var structure = Structures[gridItem.StructureId - 1];
          int newId = GetIdOfItem(newStructures, structure);
          gridItem.StructureId = newId;
        }

        // take the old tile , and remap it using the newest index
        if (gridItem.TileId != 0)
        {
          var tile = Tiles[gridItem.TileId - 1];
          int newId = GetIdOfItem(newTiles, tile);
          gridItem.TileId = newId;
        }
      }

      Structures = newStructures;
      Tiles = newTiles;
    }

    /// <summary>
    ///   Convert a given <see cref="GridCoordinate"/> into an index into <see cref="GridItems"/>.
    /// </summary>
    private static int GetFlatArrayIndex(GridBasedSize size, GridCoordinate position)
      => position.Y * size.Width + position.X;

    /// <summary>
    ///   A single grid unit in the map part that can possible contain both a structure id and a tile id.
    /// </summary>
    [Serializable]
    public struct MapGridItem
    {
      public int StructureId;
      public int TileId;
    }
  }
}