using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.Programming;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace NineBitByte.Editor.Brushes
{
  [CreateAssetMenu(fileName = "Prefab Standard", menuName = "Brushes/Standard Prefab Brush")]
  [CustomGridBrush(hideAssetInstances: true, hideDefaultInstance: false, defaultBrush: false, "Standard Drawing Brush")]
  public class SimpleDrawingBrush : GridBrush
  {
    private const int ZPosition = 0;
    
    [Tooltip("The structure that will be built when the tiles are painted")]
    public StructureDescriptor SelectedDescriptor;
    
    // internal data that's used for move operations
    private GameObject[,] _moveData;

    private bool IsEditingPalettes(GameObject brushTarget)
      => brushTarget.layer == 31;
    
    /// <inheritdoc />
    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      if (IsEditingPalettes(brushTarget))
        return;

      var instance = SelectedDescriptor?.CreateInstanceViaBrush();
      if (instance == null)
        return;

      Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
      Undo.RegisterCreatedObjectUndo(instance, "Paint Prefabs");
      Erase(gridLayout, brushTarget, position);

      MoveInstanceToCellPosition(gridLayout, brushTarget, position, instance);
    }

    /// <summary>
    /// Move the given GameObject to the the given cell location.
    /// </summary>
    private void MoveInstanceToCellPosition(GridLayout gridLayout, GameObject brushTarget, Vector3Int position, GameObject instance)
    {
      var locationPosition = gridLayout.CellToLocalInterpolated(new Vector3Int(position.x, position.y, ZPosition)
                                                                + new Vector3(.5f, .5f, .5f));
      
      instance.transform.position = gridLayout.LocalToWorld(locationPosition);
      instance.transform.SetParent(brushTarget.transform);
    }

    /// <inheritdoc />
    public override void MoveStart(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if (IsEditingPalettes(brushTarget))
        return;
      
      _moveData = new GameObject[position.size.x, position.size.y];

      foreach (var childPosition in position.allPositionsWithin)
      {
        var instance = GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(childPosition.x, childPosition.y, position.zMin));
        _moveData[childPosition.x - position.xMin, childPosition.y - position.yMin] = instance;
      }
    }

    /// <inheritdoc />
    public override void MoveEnd(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if (IsEditingPalettes(brushTarget))
        return;
      
      // when moving items, we may overlap the start region, in which case, we can't blindly erase the
      // objects at the given position.  Therefore, keep track of all of the items we're moving
      // and if we encounter them when needing to erase objects, don't erase them (they'll simply
      // move position)
      var itemsToKeep = new HashSet<GameObject>(_moveData.OfType<GameObject>().Where(it => it != null));
      
      // destroy the given object if it's not in our keep list
      void DestroyIfNeeded(GameObject it)
      {
        if (it != null && !itemsToKeep.Contains(it))
        {
          Undo.DestroyObjectImmediate(it);
        }
      }

      // iterate through all of the new positions
      foreach (var childPosition in position.allPositionsWithin)
      {
        // if there's something already there, delete it
        var existingInstance = GetObjectInCell(gridLayout, brushTarget.transform, childPosition);
        DestroyIfNeeded(existingInstance);

        // and then move the old object over to the new position
        var newInstance = _moveData[childPosition.x - position.xMin, childPosition.y - position.yMin];
        if (newInstance != null)
        {
          MoveInstanceToCellPosition(gridLayout, brushTarget, childPosition, newInstance);
        }
      }

      // clear our existing move data so that we don't accidentally do anything with it later
      _moveData = null;
    }

    /// <inheritdoc />
    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart)
    {
      if (IsEditingPalettes(brushTarget))
        return;
      
      var instance = GetObjectInCell(gridLayout, brushTarget.transform, position.position);
      var descriptor = instance?.GetComponent<StructureBehavior>()?.Programming;

      if (descriptor != null)
      {
        SelectedDescriptor = descriptor;
      }
    }

    /// <inheritdoc />
    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if (IsEditingPalettes(brushTarget))
        return;
      
      foreach (var subPosition in position.allPositionsWithin)
      {
        Paint(gridLayout, brushTarget, subPosition);        
      }
    }

    /// <inheritdoc />
    public override void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
      if (IsEditingPalettes(brushTarget))
        return;
      
      foreach (var subPosition in position.allPositionsWithin)
      {
        Erase(gridLayout, brushTarget, subPosition);        
      }
    }

    /// <inheritdoc />
    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
      if (IsEditingPalettes(brushTarget))
        return;
      
      // Do not allow editing palettes
      if (brushTarget.layer == 31)
      {
        return;
      }

      var erased = GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, ZPosition));
      if (erased != null)
        Undo.DestroyObjectImmediate(erased);
    }

    /// <summary>
    ///   Get the GameObject that is currently located in the given cell position
    /// </summary>
    internal static GameObject GetObjectInCell(GridLayout gridLayout, Transform parent, Vector3Int position)
    {
      var childCount = parent.childCount;
      var min = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(position));
      var max = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(position + Vector3Int.one));
      var bounds = new Bounds((max + min) * .5f, max - min);

      for (var i = 0; i < childCount; i++)
      {
        var child = parent.GetChild(i);
        if (bounds.Contains(child.position))
          return child?.gameObject;
      }

      return null;
    }
  }
  
  [CustomEditor(typeof(SimpleDrawingBrush))]
  public class SimpleDrawingBrushEditor : GridBrushEditor
  {
    public override void OnSelectionInspectorGUI()
    {
      var tilemap = GridSelection.target.GetComponent<Tilemap>();
      var grid = GridSelection.grid;

      var instance = SimpleDrawingBrush.GetObjectInCell(grid, tilemap.transform, GridSelection.position.position);

      if (instance != null)
      {
        EditorGUILayout.LabelField("GameObject", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.LabelField("Name", GUILayout.Width(EditorGUIUtility.labelWidth - 4));
          EditorGUILayout.SelectableLabel(instance.name, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        }
        EditorGUILayout.EndHorizontal();

        bool wasPressed = GUILayout.Button("Open Inspector");
        if (wasPressed)
        {
          Selection.activeObject = instance;
        }
      }
      else
      {
        base.OnSelectionInspectorGUI();
      }
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      var them = AssetDatabase.FindAssets("t:" + typeof(StructureDescriptor).Name)
                              .Select(g => (guid: g,
                                            name: Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(g))))
                              .OrderBy(it => it.name)
                              .ToArray();

      var names = them.Select(it => it.name).ToArray();
      
      var drawingBrush = (SimpleDrawingBrush)brush;
      var selectedDescriptor = drawingBrush.SelectedDescriptor;

      int index = -1;

      
      if (selectedDescriptor != null)
      {
        index = Array.IndexOf(names, selectedDescriptor.name);
      }
      
      int newIndex = EditorGUILayout.Popup("Structure", index, names);

      if (newIndex >= 0)
      {
        var selectedGuid = them[newIndex].guid;
        var path = AssetDatabase.GUIDToAssetPath(selectedGuid);
        var structure = AssetDatabase.LoadAssetAtPath<StructureDescriptor>(path);
        drawingBrush.SelectedDescriptor = structure;
      }
    }
    
    public override void OnPaintSceneGUI(GridLayout grid,
                                         GameObject brushTarget,
                                         BoundsInt position,
                                         GridBrushBase.Tool tool,
                                         bool executing)
    {
      base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);

      var labelText = "Pos: " + new Vector3Int(position.x, position.y, 0);
      if (position.size.x > 1 || position.size.y > 1)
      {
        labelText += " Size: " + new Vector2Int(position.size.x, position.size.y);
      }

      var style = new GUIStyle() { normal = { textColor = Color.yellow }};
      Handles.Label(grid.CellToWorld(new Vector3Int(position.x, position.y, 0)), labelText, style);
    }
  }
}