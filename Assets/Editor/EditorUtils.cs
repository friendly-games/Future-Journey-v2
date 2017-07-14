using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace NineBitByte.Assets.Editor
{
  public static class EditorUtils
  {
    // http://answers.unity3d.com/questions/62455/how-do-i-make-fields-in-the-inspector-go-bold-when.html?sort=oldest

    private static MethodInfo _boldFontMethodInfo = null;

    public static void SetBoldDefaultFont(bool value)
    {
      if (_boldFontMethodInfo == null)
        _boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont",
                                                                 BindingFlags.Static | BindingFlags.NonPublic);
      _boldFontMethodInfo.Invoke(null, new[] { value as object });
    }

    /// <summary> Gets all of the properties for the given serialized object. </summary>
    public static IEnumerable<SerializedProperty> GetProperties(SerializedObject instance)
    {
      var prop = instance.GetIterator();

      while (prop.Next(true))
      {
        yield return prop;
      }
    }

    /// <summary> Gets all of the properties for the given serialized object. </summary>
    public static IEnumerable<SerializedProperty> GetVisibleProperties(SerializedObject instance)
    {
      var prop = instance.GetIterator();



      while (prop.NextVisible(true))
      {
        yield return prop;
      }
    }

    public static SerializedProperty FindProperty(this SerializedObject serializedObject, params string[] propertyParts)
    {
      return serializedObject.FindProperty(GetPropertyPath(propertyParts));
    }

    public static string GetArrayElementPath(string propertyName, int index) 
      => $"{propertyName}.Array.data[{index}]";

    public static string GetPropertyPath(params string[] propertyParts) 
      => string.Join(".", propertyParts);
  }
}