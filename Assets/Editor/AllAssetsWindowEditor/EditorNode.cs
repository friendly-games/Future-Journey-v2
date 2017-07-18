using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEditor;

namespace NineBitByte.Assets.Editor.AllAssetsWindowEditor
{
  public class EditorNode : INode
  {
    public EditorNode(string name, string fullPath)
    {
      Name = name;
      FullPath = fullPath;
    }

    public string Name { get; }

    public string FullPath { get; }

    public void Draw(AssetWindow window)
    {
      var baseScriptable = AssetDatabase.LoadAssetAtPath<BaseScriptable>(FullPath);
      if (baseScriptable != null)
      {
        window.DrawDefaultInspectorFor(new SerializedObject(baseScriptable));
      }
    }
  }
}