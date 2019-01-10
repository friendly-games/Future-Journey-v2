using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace NineBitByte.FutureJourney.Test.Programming
{
  public class WorldLayoutTests
  {
    private WorldLayout _layout;
    private StructureDescriptor _structure1;
    private StructureDescriptor _structure2;
    private StructureDescriptor _structure3;

    [SetUp]
    public void Setup()
    {
      _layout = ScriptableObject.CreateInstance<WorldLayout>();
      _layout.Resize(new GridBasedSize(3, 3));
      _structure1 = ScriptableObject.CreateInstance<StructureDescriptor>();
      _structure2 = ScriptableObject.CreateInstance<StructureDescriptor>();
      _structure3 = ScriptableObject.CreateInstance<StructureDescriptor>();
    }

    [TearDown]
    public void Teardown()
    {
      Object.DestroyImmediate(_layout);
      Object.DestroyImmediate(_structure1);
      Object.DestroyImmediate(_structure2);
      Object.DestroyImmediate(_structure3);
    }
    
    private ref WorldLayout.MapGridItem Get(int x, int y, WorldLayout instance = null)
      => ref (instance ?? _layout)[new GridCoordinate(x, y)];
    
    private void Update(int x, int y, GridItemCallback callback)
    {
      ref var gridItem = ref Get(x, y);
      callback.Invoke(ref gridItem);
    }

    delegate void GridItemCallback(ref WorldLayout.MapGridItem item);

    [Test]
    public void TestScriptSimplePasses()
    {
      _layout.SetStructure(new GridCoordinate(0, 0), _structure1 );
      _layout.SetStructure(new GridCoordinate(1, 1), _structure2 );
      _layout.SetStructure(new GridCoordinate(2, 2), _structure3 );
      
      Assert.That(Get(0, 0).StructureId, Is.EqualTo(1));
      Assert.That(Get(1, 1).StructureId, Is.EqualTo(2));
      Assert.That(Get(2, 2).StructureId, Is.EqualTo(3));
      Assert.That(_layout.GridItems.Count(it => it.StructureId != 0), Is.EqualTo(3));
      
      _layout.Resize(new GridBasedSize(5, 5));

      Assert.That(Get(0, 0).StructureId, Is.EqualTo(1));
      Assert.That(Get(1, 1).StructureId, Is.EqualTo(2));
      Assert.That(Get(2, 2).StructureId, Is.EqualTo(3));
      Assert.That(Get(3, 3).StructureId, Is.EqualTo(0));
      Assert.That(Get(4, 4).StructureId, Is.EqualTo(0));

      Assert.That(_layout.GridItems.Count(it => it.StructureId != 0), Is.EqualTo(3));
    }

    [Test]
    public void SerializationAndDeserialization_Works()
    {
      Update(0, 0, (ref WorldLayout.MapGridItem it) => it.TileId = 1);
      Update(1, 1, (ref WorldLayout.MapGridItem it) => it.TileId = 2);
      Update(2, 2, (ref WorldLayout.MapGridItem it) => it.TileId = 3);
      Assert.That(_layout.GridItems.Count(it => it.TileId != 0), Is.EqualTo(3));
      var newInstance = Object.Instantiate(_layout);
      
      Assert.That(newInstance.GridItems.Length, Is.EqualTo(_layout.GridItems.Length));
      
      Assert.That(Get(0, 0, newInstance).TileId, Is.EqualTo(1));
      Assert.That(Get(1, 1, newInstance).TileId, Is.EqualTo(2));
      Assert.That(Get(2, 2, newInstance).TileId, Is.EqualTo(3));

      Assert.That(newInstance.GridItems.Count(it => it.TileId != 0), Is.EqualTo(3));
      
      Assert.That(newInstance.Structures, Is.EquivalentTo(_layout.Structures));
      Assert.That(newInstance.Tiles, Is.EquivalentTo(_layout.Tiles));
      Assert.That(newInstance.GridItems, Is.EquivalentTo(_layout.GridItems));
    }

    [Test]
    public void SettingStructure_AddsItToStructureList()
    {
      _layout.SetStructure(new GridCoordinate(1, 1), _structure1);

      Assert.That(_layout.Structures[0], Is.EqualTo(_structure1));
      Assert.That(Get(1, 1).StructureId, Is.EqualTo(1));
      
      _layout.SetStructure(new GridCoordinate(1, 2), _structure2);
      Assert.That(_layout.Structures[1], Is.EqualTo(_structure2));
      Assert.That(Get(1, 2).StructureId, Is.EqualTo(2));
    }
    
    [Test]
    public void OverwritingExistingStructure_RemovesOriginalStructure_AndAddsNewStructure()
    {
      _layout.SetStructure(new GridCoordinate(1, 1), _structure1);
      _layout.SetStructure(new GridCoordinate(1, 1), _structure2);

      // structure 2 is now the only structure
      Assert.That(_layout.Structures[0], Is.EqualTo(_structure2));
      Assert.That(Get(1, 1).StructureId, Is.EqualTo(1));
    }

    [Test]
    public void ClearingMiddleStructure_CanLeaveNullGap()
    {
      _layout.SetStructure(new GridCoordinate(1, 1), _structure1);
      _layout.SetStructure(new GridCoordinate(1, 2), _structure2);
      
      _layout.ClearStructure(new GridCoordinate(1, 1));
      
      Assert.That(_layout.Structures[0], Is.Null);
      Assert.That(_layout.Structures[1], Is.EqualTo(_structure2));
    }
    
    [Test]
    public void ReIndex_UpdatesIndexesOfNonNullStructures()
    {
      _layout.SetStructure(new GridCoordinate(2, 2), _structure1);
      _layout.SetStructure(new GridCoordinate(1, 1), _structure2);
      
      _layout.Resize(new GridBasedSize(2, 2));
      _layout.ReIndex();
      
      Assert.That(_layout.Structures.Count, Is.EqualTo(1));
      Assert.That(_layout.Structures[0], Is.EqualTo(_structure2));
      Assert.That(Get(1,1).StructureId, Is.EqualTo(1));
    }
  }
}