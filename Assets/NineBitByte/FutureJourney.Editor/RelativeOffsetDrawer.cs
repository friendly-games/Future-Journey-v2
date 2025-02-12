using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common.Structures;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.FutureJourney.Editor
{
  [CustomPropertyDrawer(typeof(RelativeOffset))]
  public class RelativeOffsetDrawer : PropertyDrawer
  {
    /// <inheritdoc />
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var offsetProperty = property.FindPropertyRelative(RelativeOffset.SerializedFieldName);
      offsetProperty.vector3Value = EditorGUI.Vector3Field(position, label, offsetProperty.vector3Value);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      var offsetProperty = property.FindPropertyRelative(RelativeOffset.SerializedFieldName);
      return EditorGUI.GetPropertyHeight(offsetProperty);
    }
  }
}