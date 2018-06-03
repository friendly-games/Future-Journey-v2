using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NineBitByte.Common;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Editor
{
  public static class MarkObjectsDirty
  {
    [MenuItem("Project/Tools/Mark Selected Assets Dirty")]
    private static void MarkSelectedDirty()
    {
      foreach (var o in Selection.objects)
      {
        EditorUtility.SetDirty(o);
      }
      
      AssetDatabase.SaveAssets();
    }
    
    [MenuItem("Project/Tools/Mark All Assets Dirty")]
    private static void MarkAllDrity()
    {
      var root = "Assets/Programming";
      var assets = AssetDatabase.FindAssets("*", new[] { root });
      
      foreach (var assetGuid in assets)
      {
        var path = AssetDatabase.GUIDToAssetPath(assetGuid);
        var scriptable = AssetDatabase.LoadAssetAtPath<BaseScriptable>(path);
        
        if (scriptable != null)
        {
          Debug.Log(scriptable);
          EditorUtility.SetDirty(scriptable);
        }
      }

      AssetDatabase.SaveAssets();
    }
  }
}