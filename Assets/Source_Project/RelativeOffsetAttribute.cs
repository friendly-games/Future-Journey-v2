using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
    private readonly GUIStyle style = new GUIStyle();

    public void OnEnable()
    {
      style.fontStyle = FontStyle.Normal;
      style.fontSize = 10;
      style.normal.textColor = Color.white;
      style.alignment = TextAnchor.UpperCenter;
    }

    public void OnSceneGUI()
    {
      var owner = ((MonoBehaviour)serializedObject.targetObject).gameObject;
      var transform = owner.transform;

      var targetObjectType = serializedObject.targetObject.GetType();

      var validFields =
        from field in targetObjectType.GetFields()
        where field.GetCustomAttribute<RelativeOffsetAttribute>() != null
        where field.FieldType == typeof(Vector3)
              || field.FieldType == typeof(Vector3[])
        select field;

      foreach (var field in validFields)
      {
        var property = serializedObject.FindProperty(field.Name);

        if (!property.isArray)
        {
          // just a single Vector3 field, easy
          ShowHandle(property, transform, property.name);
        }
        else
        {
          for (int i = 0; i < property.arraySize; i++)
          {
            var childProp = serializedObject.FindProperty($"{property.name}.Array.data[{i}]");
            ShowHandle(childProp, transform, $"{property.name}[{i}]");
          }
        }
      }
    }

    private void ShowHandle(SerializedProperty property, Transform transform, string name)
    {
      var rotation = transform.rotation;
      var offset = transform.position;

      // get value
      Vector3 handlePosition = property.vector3Value;
      handlePosition += offset;

      // show editor/value
      Handles.Label(handlePosition, name, style);
      handlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);

      // update value
      handlePosition -= offset;

      property.vector3Value = handlePosition;

      // persist
      serializedObject.ApplyModifiedProperties();
    }
  }
#endif
}
