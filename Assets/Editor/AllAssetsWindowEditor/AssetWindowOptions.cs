using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Assets.Editor.AllAssetsWindowEditor
{
  [Serializable]
  public class AssetWindowOptions
  {
    public AssetWindowOptions()
    {
      ExpandStates = new Dictionary<string, bool>();
      ScrollPosition = new Vector2();
    }

    public Dictionary<string, bool> ExpandStates { get; }

    [JsonIgnore]
    public Vector2 ScrollPosition { get; set; }

    public float ScrollX
    {
      get { return ScrollPosition.x; }
      set { ScrollPosition = new Vector2(value, ScrollPosition.y); }
    }

    public float ScrollY
    {
      get { return ScrollPosition.y; }
      set { ScrollPosition = new Vector2(ScrollPosition.x, value); }
    }

    public void Save()
    {
      string json = JsonConvert.SerializeObject(this);
      EditorPrefs.SetString(typeof(AssetWindowOptions).Name, json);
    }

    public void Load()
    {
      var json = EditorPrefs.GetString(typeof(AssetWindowOptions).Name);
      if (json != null)
      {
        JsonConvert.PopulateObject(json, this);
      }
    }
  }
}