using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Assets.Editor.AllAssetsWindowEditor
{
  public class AssetWindow : EditorWindow
  {
    [MenuItem("Window/Project Assets")]
    [MenuItem("Project/Project Assets")]
    public static void ShowWindow()
    {
      GetWindow().Show();
    }

    public AssetWindowOptions Options { get; }

    public static AssetWindow GetWindow()
    {
      return (AssetWindow)GetWindow(typeof(AssetWindow));
    }

    public void OnEnable()
    {
      Options.Load();
    }

    public void OnLostFocus()
    {
      Options.Save();
    }

    public AssetWindow()
    {
      Options = new AssetWindowOptions();
      titleContent = new GUIContent("All Assets");
    }

    private const string RootPath = "Assets/Programming";
    private readonly StringBuilder _builder = new StringBuilder();

    private FolderNode _rootNode;

    public void OnGUI()
    {
      if (_rootNode == null)
      {
        _rootNode = RefreshCache();
      }

      Options.ScrollPosition = GUILayout.BeginScrollView(Options.ScrollPosition);
      _rootNode.Draw(this);

      GUILayout.EndScrollView();
    }

    public FolderNode RefreshCache()
    {
      var rootNode = new FolderNode("root", "");
      var assets = AssetDatabase.FindAssets("*", new[] { RootPath });

      foreach (var asset in assets)
      {
        var path = AssetDatabase.GUIDToAssetPath(asset);

        Add(rootNode, path);
      }

      return rootNode;
    }

    public void Add(FolderNode rootNode, string path)
    {
      var shortenedPath = path.Substring(RootPath.Length + 1);
      var prettyName = Path.GetFileNameWithoutExtension(shortenedPath);
      var parts = prettyName.Split('.');

      FolderNode currentNode = rootNode;

      _builder.Clear();

      for (var i = 0; i < parts.Length - 1; i++)
      {
        if (i > 0)
        {
          _builder.Append(".");
        }

        _builder.Append(parts[i]);

        currentNode = currentNode.GetOrCreate(parts[i], _builder.ToString());
      }

      currentNode.Add(new EditorNode(parts[parts.Length - 1], path));
    }

    internal bool DrawDefaultInspectorFor(SerializedObject obj)
    {
      EditorGUI.BeginChangeCheck();
      obj.Update();
      SerializedProperty iterator = obj.GetIterator();

      for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
      {
        if ("m_Script" != iterator.propertyPath)
          EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
      }

      obj.ApplyModifiedProperties();
      return EditorGUI.EndChangeCheck();
    }
  }
}