using System;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.FutureJourney.Editor.AllAssetsWindowEditor
{
  [Serializable]
  public class AssetWindowOptions
  {
    public AssetWindowOptions()
    {
      ExpandStates = new Dictionary<string, bool>();
      ScrollPosition = new Vector2();
    }

    [fsProperty]
    public Dictionary<string, bool> ExpandStates { get; private set; }

    public Vector2 ScrollPosition { get; set; }

    public void Save()
    {

      var existingJson = EditorPrefs.GetString(typeof(AssetWindowOptions).Name);
      string json = StringSerializationAPI.Serialize(this);

      if (existingJson != json)
      {
        Debug.Log(json);
        EditorPrefs.SetString(typeof(AssetWindowOptions).Name, json);
      }
    }

    public void Load()
    {
      var json = EditorPrefs.GetString(typeof(AssetWindowOptions).Name);
      if (json != null)
      {
        StringSerializationAPI.DeserializeInto(typeof(AssetWindowOptions), json, this);
      }
    }

    public static class StringSerializationAPI
    {
      private static readonly fsSerializer _serializer = new fsSerializer();


      public static string Serialize(object value)
      {
        // serialize the data
        fsData data;
        _serializer.TrySerialize(value.GetType(), value, out data).AssertSuccessWithoutWarnings();

        // emit the data via JSON
        return fsJsonPrinter.CompressedJson(data);
      }

      public static object Deserialize(Type type, string serializedState)
      {
        // step 2: deserialize the data
        object deserialized = null;
        Deserialize(type, serializedState, ref deserialized);
        return deserialized;
      }

      public static void DeserializeInto(Type type, string serializedState, object existing)
      {
        // step 2: deserialize the data
        Deserialize(type, serializedState, ref existing);
      }

      public static void Deserialize(Type type, string serializedState, ref object deserialized)
      {
        // step 1: parse the JSON data
        fsData data = fsJsonParser.Parse(serializedState);

        // step 2: deserialize the data
        _serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();
      }
    }
  }
}