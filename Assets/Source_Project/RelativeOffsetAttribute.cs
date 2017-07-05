using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NineBitByte.Assets.Source.FutureJourney.Items;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Assets.Source_Project
{
  public class RelativeOffsetAttribute : PropertyAttribute
  {
  }

#if UNITY_EDITOR
  [CustomEditor(typeof(MonoBehaviour), true)]
  public class RelativeOffsetDrawer : Editor
  {
    private const int RoundPrecision = 3;

    private readonly GUIStyle _style = new GUIStyle();

    [UsedImplicitly]
    public void OnEnable()
    {
      _style.fontStyle = FontStyle.Normal;
      _style.fontSize = 10;
      _style.normal.textColor = Color.white;
      _style.alignment = TextAnchor.UpperCenter;
    }

    private IEnumerable<SerializedProperty> GetProperties(SerializedObject instance)
    {
      var prop = instance.GetIterator();

      while (prop.Next(true))
      {
        yield return prop;
      }
    }

    [UsedImplicitly]
    public void OnSceneGUI()
    {
      var owner = ((MonoBehaviour)serializedObject.targetObject).gameObject;
      var transform = owner.transform;

      var targetObjectType = serializedObject.targetObject.GetType();

      bool wasChanged = false;

      foreach (var field in targetObjectType.GetFields().Where(IsValidField))
      {
        wasChanged |= ProcessField(field, transform);
      }

      if (wasChanged)
      {
        // persist
        serializedObject.ApplyModifiedProperties();
      }
    }

    private bool IsValidField(FieldInfo field)
    {
      bool hasAttribute = field.GetCustomAttribute<RelativeOffsetAttribute>() != null;

      if (hasAttribute && field.FieldType == typeof(Vector3)
          || field.FieldType == typeof(Vector3[]))
        return true;

      if (field.FieldType == typeof(RelativeOffset))
        return true;

      return false;
    }

    private bool ProcessField(FieldInfo field, Transform transform)
    {
      if (field.FieldType == typeof(Vector3))
      {
        // just a single Vector3 field, easy
        var property = serializedObject.FindProperty(field.Name);
        return ShowHandle(property, transform, property.name);
      }

      if (field.FieldType == typeof(RelativeOffset))
      {
        // just a single Vector3 field, easy
        var property = serializedObject.FindProperty(field.Name + "." + RelativeOffset.SerializedFieldName);
        return ShowHandle(property, transform, property.name);
      }

      if (field.FieldType == typeof(Vector3[]))
      {
        var property = serializedObject.FindProperty(field.Name);
        for (int i = 0; i < property.arraySize; i++)
        {
          var childProp = serializedObject.FindProperty($"{property.name}.Array.data[{i}]");
          return ShowHandle(childProp, transform, $"{property.name}[{i}]");
        }
      }

      return false;
    }

    /// <summary> Shows a handle for the given property. </summary>
    /// <param name="property"> The property to show a draggable handle for. </param>
    /// <param name="transform"> The transform of the object whose property is being editted. </param>
    /// <param name="handleName"> The name that should be displayed underneath the handle so that
    ///  different handles can be told apart. </param>
    /// <returns> True if the property changed and needs to be persisted, false otherwise. </returns>
    private bool ShowHandle(SerializedProperty property, Transform transform, string handleName)
    {
      handleName = ObjectNames.NicifyVariableName(handleName);

      var rotation = transform.rotation;
      var offset = transform.position;

      // convert relative position to world position
      Vector3 handlePosition = rotation * property.vector3Value + offset;

      // show editor/value
      Handles.Label(handlePosition, handleName, _style);
      var newValue = Handles.PositionHandle(handlePosition, rotation);

      // update value (back to world relative)
      handlePosition = Quaternion.Inverse(rotation) * (newValue - offset);

      // because of the rotational multiplications above, it's possible that even without moving the
      // handle we'll experience value changes (floating point maths), if we do not force some sort
      // of rounding the editor values will flux by minute values (e.g. very close to zero). 
      handlePosition.x = (float)Math.Round(handlePosition.x, RoundPrecision);
      handlePosition.y = (float)Math.Round(handlePosition.y, RoundPrecision);
      handlePosition.z = (float)Math.Round(handlePosition.z, RoundPrecision);

      if (handlePosition != property.vector3Value)
      {
        property.vector3Value = handlePosition;
        return true;
      }

      return false;
    }
  }
#endif
}