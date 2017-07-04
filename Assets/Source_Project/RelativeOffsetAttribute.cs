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
      style.fontStyle = FontStyle.Bold;
      style.normal.textColor = Color.white;
    }

    public void OnSceneGUI()
    {
      var owner = ((MonoBehaviour)serializedObject.targetObject).gameObject;
      var offset = owner.transform.position;

      var targetObjectType = serializedObject.targetObject.GetType();

      var validProperties =
        from property in GetProperties(serializedObject)
        where property.propertyType == SerializedPropertyType.Vector3
        where targetObjectType.GetField(property.name)
                              ?.GetCustomAttribute<RelativeOffsetAttribute>() != null
        select property;

      foreach (var property in validProperties)
      {
        ShowHandle(property, offset);
      }
    }

    private void ShowHandle(SerializedProperty property, Vector3 offset)
    {
      // get value
      Vector3 handlePosition = property.vector3Value;
      handlePosition += offset;

      // show editor/value
      Handles.Label(handlePosition, property.name);
      handlePosition = Handles.PositionHandle(handlePosition, Quaternion.identity);

      // update value
      handlePosition -= offset;

      property.vector3Value = handlePosition;

      // persist
      serializedObject.ApplyModifiedProperties();
    }

    private static IEnumerable<SerializedProperty> GetProperties(SerializedObject serializedObject)
    {
      var property = serializedObject.GetIterator();

      while (property.Next(true))
      {
        yield return property;
      }
    }
  }
#endif
}
