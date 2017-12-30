using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Editor.AllAssetsWindowEditor
{
  /// <summary> Tells the window to refresh the nodes on asset change. </summary>
  public class RefreshAssetWindowOnPostProcessor : AssetPostprocessor
  {
    public static void OnPostprocessAllAssets(string[] importedAssets,
                                              string[] deletedAssets,
                                              string[] movedAssets,
                                              string[] movedFromAssetPaths)
    {
      Debug.Log("Asset Refresh");
      AssetWindow.GetWindow().MarkOutOfDate();
    }
  }
}