using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.FutureJourney.Programming
{
  public class ScriptableInitializerBehavior : BaseBehavior
  {
    public void Start()
    {
      var assets = AssetDatabase.FindAssets("*", new[] { "Assets/Programming" });
      var scriptables = (BaseScriptable[])Resources.FindObjectsOfTypeAll(typeof(BaseScriptable));
      
      foreach (var scriptable in scriptables)
      {
        scriptable.OnStartup();
      }
    }
  }
}