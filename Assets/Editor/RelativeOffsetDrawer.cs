using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Items;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Assets.Editor
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
  }
}