using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace NineBitByte.Assets.Editor.AllAssetsWindowEditor
{
  public class FolderNode : INode
  {
    private List<INode> _nodes;

    public FolderNode(string name, string fullPath)
    {
      Name = name;
      RelativePath = fullPath;
    }

    public string Name { get; }

    public string RelativePath { get; }

    public FolderNode GetOrCreate(string name, string fullPath)
    {
      var parent = Folders.FirstOrDefault(i => i.Name == name);
      if (parent == null)
      {
        parent = new FolderNode(name, fullPath);
        Add(parent);
      }

      return parent;
    }

    public void Add(INode node)
    {
      var nodes = _nodes ?? (_nodes = new List<INode>());
      int index = nodes.BinarySearch(node, NodeComparer.Instance);

      if (index < 0)
        nodes.Insert(~index, node);
      else
        nodes.Insert(index, node);
    }

    public IEnumerable<FolderNode> Folders
      => _nodes?.OfType<FolderNode>() ?? Array.Empty<FolderNode>();

    public IEnumerable<EditorNode> Editors
      => _nodes?.OfType<EditorNode>() ?? Array.Empty<EditorNode>();

    private class NodeComparer : IComparer<INode>
    {
      public static readonly NodeComparer Instance
        = new NodeComparer();

      public int Compare(INode x, INode y)
        => x.Name.CompareTo(y.Name);
    }

    public void Draw(AssetWindow window)
    {
      foreach (var folder in Folders)
      {
        Draw(window, folder);
      }

      foreach (var editor in Editors)
      {
        Draw(window, editor);
      }
    }

    private static void Draw(AssetWindow window, INode folder)
    {
      bool isExpanded;
      var expandStates = window.Options.ExpandStates;

      isExpanded = !expandStates.TryGetValue(folder.RelativePath, out isExpanded) || isExpanded;
      isExpanded = EditorGUILayout.Foldout(isExpanded, folder.Name);

      expandStates[folder.RelativePath] = isExpanded;

      if (isExpanded)
      {
        EditorGUI.indentLevel++;
        folder.Draw(window);
        EditorGUI.indentLevel--;
      }
    }

    public static IEnumerable<INode> GetNodes(FolderNode root)
    {
      foreach (var child in root.Folders)
      {
        yield return child;

        foreach (var other in GetNodes(child))
        {
          yield return other;
        }
      }

      foreach (var child in root.Editors)
      {
        yield return child;
      }
    }
  }
}