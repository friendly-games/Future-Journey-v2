using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NineBitByte.Common;
using NineBitByte.FutureJourney.World;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Editor.Drawers
{
  /// <summary>
  ///  Render <see cref="GridBasedSize"/> as a "WxH" string
  /// </summary>
  [CustomPropertyDrawer(typeof(GridBasedSize))]
  public class GridBasedSizeDrawer : PropertyDrawer
  {
    /// <inheritdoc />
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // if it's different from the prefab, go ahead and bold it
      if (property.isInstantiatedPrefab)
      {
        EditorUtils.SetBoldDefaultFont(property.prefabOverride);
      }

      var propertyWidth = property.FindPropertyRelative(nameof(GridBasedSize.Width));
      var propertyHeight = property.FindPropertyRelative(nameof(GridBasedSize.Height));

      var originalSize = $"{propertyWidth.intValue}x{propertyHeight.intValue}";
      
      var newSize = EditorGUI.TextField(position, property.name, originalSize);
      if (newSize == originalSize)
        return;

      if (Regex.Match(newSize, @"(\d+)(?:x|,| )(\d+)") is Match match && match.Success)
      {
        propertyWidth.intValue = int.Parse(match.Groups[1].Value);
        propertyHeight.intValue = int.Parse(match.Groups[2].Value);
      }
    }
  }
}