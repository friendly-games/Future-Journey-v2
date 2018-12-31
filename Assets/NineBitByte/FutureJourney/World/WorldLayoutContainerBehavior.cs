using System;
using System.Linq;
using System.Collections.Generic;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

namespace NineBitByte.FutureJourney.World
{
  public class WorldLayoutContainerBehavior : BaseBehavior
  {
    [Tooltip("The layout that should be edited when this container is modified")]
    public WorldLayout AssociatedLayout;

    /// <summary>
    ///   Draws a solid rectangle in the grid-area that this layout is composed of, + a border, so that the area
    ///   can be easily identified. 
    /// </summary>
    private void DrawGizmo()
    {
      var grid = transform.parent?.GetComponent<Grid>();
      if (grid == null)
        return;

      Gizmos.color = new Color(0, .5f, 0, .1f);
      var cellSize = grid.cellSize;

      var size = new Vector3(cellSize.x * AssociatedLayout.Size.Width,
                             cellSize.y * AssociatedLayout.Size.Width);

      var gridPosition = grid.transform.position;
      var center = size / 2 + gridPosition;
      Gizmos.DrawCube(center, size);

      var bottomLeft = center - size / 2;
      var topRight = center + size / 2;

      Gizmos.color = new Color(1f, 1f, 0, 0.5f);
      Gizmos.DrawLine(new Vector3(bottomLeft.x, gridPosition.y), new Vector3(bottomLeft.x, topRight.y));
      Gizmos.DrawLine(new Vector3(bottomLeft.x, gridPosition.y), new Vector3(topRight.x, bottomLeft.y));
      
      Gizmos.DrawLine(new Vector3(topRight.x, topRight.y), new Vector3(bottomLeft.x, topRight.y));
      Gizmos.DrawLine(new Vector3(topRight.x, topRight.y), new Vector3(topRight.x, bottomLeft.y));
    }

    private void OnDrawGizmos()
    {
      #if UNITY_EDITOR
      var active = UnityEditor.Selection.activeGameObject;
      
      if (active?.GetAncestorsAndSelf().Any(it => it == gameObject) == true)
      {
        DrawGizmo();
      }
      #endif
    }
  }
}