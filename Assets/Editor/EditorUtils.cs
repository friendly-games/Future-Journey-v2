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
  }
}