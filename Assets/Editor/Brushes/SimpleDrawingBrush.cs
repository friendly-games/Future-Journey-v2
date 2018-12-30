using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NineBitByte.Editor.Brushes
{
  [CreateAssetMenu(fileName = "Prefab Standard", menuName = "Brushes/Standard Prefab Brush")]
  // [CustomGridBrush(false, true, false, "Standard Prefab Brush")]
  [CustomGridBrush(hideAssetInstances: true, hideDefaultInstance: false, defaultBrush: false, "Standard Drawing Brush")]
  public class SimpleDrawingBrush : GridBrush
  {
    public GameObject[] Prefabs;
    
    public int ZPosition;

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
      // Do not allow editing palettes
      if (brushTarget.layer == 31)
        return;

      var index = 0;
      var prefab = Prefabs[index];

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
  }
}