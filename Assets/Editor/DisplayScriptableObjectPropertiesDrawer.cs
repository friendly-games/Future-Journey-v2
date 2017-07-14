using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Assets.Editor
{
  [CustomPropertyDrawer(typeof(DisplayScriptableObjectPropertiesAttribute))]
  public class DisplayScriptableObjectPropertiesDrawer : PropertyDrawer
  {
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      position.height = 16;
      EditorGUI.PropertyField(position, property);
      position.y += 20;
      position.x += 20;

      position.width -= 40;

      var so = new SerializedObject(property.objectReferenceValue);
      so.Update();

      foreach (var prop in EditorUtils.GetVisibleProperties(so))
      {
        position.height = 16;
        EditorGUI.PropertyField(position, prop);
        position.y += 20;
      }

      if (GUI.changed)
        so.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      float height = base.GetPropertyHeight(property, label);
      height += EditorUtils.GetVisibleProperties(new SerializedObject(property.objectReferenceValue))
                           .Count() * 20;
      return height;
    }
  }
}