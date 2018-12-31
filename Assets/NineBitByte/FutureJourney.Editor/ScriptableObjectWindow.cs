using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace NineBitByte.FutureJourney.Editor
{
  /// <summary>
  /// Helper class for instantiating ScriptableObjects.
  /// </summary>
  public class ScriptableObjectWindow : EditorWindow
  {
    [MenuItem("Assets/Create Scriptable Object", priority = 3)]
    [MenuItem("Project/Create Scriptable Object")]
    public static void Create()
    {
      var assembly = Assembly.Load(new AssemblyName("Assembly-CSharp"));

      // Get all classes derived from ScriptableObject
      var allScriptableObjects =
          from t in assembly.GetTypes()
          where t.IsSubclassOf(typeof(ScriptableObject))
          where !t.IsGenericType && !t.IsAbstract
          select t
        ;

      // Show the selection window.
      var window = GetWindow<ScriptableObjectWindow>(
        true,
        "Create a new ScriptableObject",
        true);

      window.Types = allScriptableObjects.ToArray();
      window.ShowPopup();
    }

    private int _selectedIndex;

    public Type[] Types { get; set; }

    private string[] GetNames()
    {
      return Types.Select(
        t => $"{ObjectNames.NicifyVariableName(t.Name)} ({t.FullName})"
      ).ToArray();
    }

    public void OnGUI()
    {
      GUILayout.Label("ScriptableObject Class");
      _selectedIndex = EditorGUILayout.Popup(_selectedIndex, GetNames());

      var selectedType = Types[_selectedIndex];
      var defaultName = ObjectNames.NicifyVariableName(selectedType.Name) + ".asset";

      if (GUILayout.Button("Create"))
      {
        var asset = CreateInstance(Types[_selectedIndex]);

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
          asset.GetInstanceID(),
          CreateInstance<ScriptableObjectWindowEndNameEdit>(),
          defaultName,
          AssetPreview.GetMiniThumbnail(asset),
          null);

        Close();
      }
    }

    public class ScriptableObjectWindowEndNameEdit : EndNameEditAction
    {
      public override void Action(int instanceId, string pathName, string resourceFile)
      {
        AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId),
                                  AssetDatabase.GenerateUniqueAssetPath(pathName));
      }
    }
  }
}