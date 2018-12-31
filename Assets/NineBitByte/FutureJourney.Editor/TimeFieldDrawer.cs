using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NineBitByte.FutureJourney.Items;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Editor
{
  [CustomPropertyDrawer(typeof(TimeField))]
  public class TimeFieldDrawer : PropertyDrawer
  {
    /// <inheritdoc />
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // if it's different from the prefab, go ahead and bold it
      if (property.isInstantiatedPrefab)
      {
        EditorUtils.SetBoldDefaultFont(property.prefabOverride);
      }

      // get the current value of the field
      var timeValue = property.FindPropertyRelative(TimeField.NameOfSerializedField);

      var timeFieldAttribute = fieldInfo.GetCustomAttribute<TimeFieldAttribute>() 
        ?? TimeFieldAttribute.Default;

      float multiplier;
      string unitLabel;

      timeFieldAttribute.SpecifiedIn.GetInfo(out multiplier, out unitLabel);

      var sliderLabel = $"{ObjectNames.NicifyVariableName(property.name)} (in {unitLabel})";

      // convert from ms
      var modifiedValue = timeValue.longValue / multiplier;

      // let the user edit the value
      modifiedValue = EditorGUI.Slider(
        position,
        sliderLabel,
        modifiedValue,
        timeFieldAttribute.Minimum,
        timeFieldAttribute.Maximum
      );

      // convert back into ms
      modifiedValue = modifiedValue * multiplier;

      // save it
      timeValue.longValue = (long)modifiedValue;
    }
  }
}