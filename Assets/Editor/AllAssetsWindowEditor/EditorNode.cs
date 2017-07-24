using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEditor;

namespace NineBitByte.Assets.Editor.AllAssetsWindowEditor
{
  public class EditorNode : INode
  {
    public const string RootPath = "Assets/Programming";

    public EditorNode(string name, string relativePath)
    {
      Name = name;
      RelativePath = relativePath;
    }

    public string Name { get; }

    public string RelativePath { get; }

    public void Draw(AssetWindow window)
    {
      var baseScriptable = AssetDatabase.LoadAssetAtPath<BaseScriptable>(RootPath + "/" + RelativePath);
      if (baseScriptable != null)
      {
        window.DrawDefaultInspectorFor(new SerializedObject(baseScriptable));
      }
    }
  }
}