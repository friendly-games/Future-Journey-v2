using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public int ZPosition;

    public StructureDescriptor SelectedDescriptor;

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
      // Do not allow editing palettes
      if (brushTarget.layer == 31)
        return;

      var prefab = SelectedDescriptor?.Template;
      if (prefab == null)
        return;

      Erase(grid, brushTarget, position);
      
      var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
      if (instance != null)
      {
        Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
        Undo.RegisterCreatedObjectUndo(instance, "Paint Prefabs");
        instance.transform.SetParent(brushTarget.transform);
        instance.transform.position =
          grid.LocalToWorld(
            grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, ZPosition) + new Vector3(.5f, .5f, .5f)));
      }
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
      // Do not allow editing palettes
      if (brushTarget.layer == 31)
      {
        return;
      }

      var erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, ZPosition));
      if (erased != null)
        Undo.DestroyObjectImmediate(erased.gameObject);
    }

    internal static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
      var childCount = parent.childCount;
      var min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
      var max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
      var bounds = new Bounds((max + min) * .5f, max - min);

      for (var i = 0; i < childCount; i++)
      {
        var child = parent.GetChild(i);
        if (bounds.Contains(child.position))
          return child;
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
          EditorGUILayout.SelectableLabel(instance.gameObject.name, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        }
        EditorGUILayout.EndHorizontal();

        bool wasPressed = GUILayout.Button("Open Inspector");
        if (wasPressed)
        {
          Selection.activeObject = instance.gameObject;
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
  }
}