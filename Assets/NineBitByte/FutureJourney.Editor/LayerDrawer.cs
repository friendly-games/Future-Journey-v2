using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEditor;
using UnityEngine;

namespace NineBitByte.Editor
{
  [CustomPropertyDrawer(typeof(Layer))]
  public class LayerDrawer : PropertyDrawer
  {
    /// <inheritdoc />
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // if it's different from the prefab, go ahead and bold it
      if (property.isInstantiatedPrefab)
      {
        EditorUtils.SetBoldDefaultFont(property.prefabOverride);
      }
      
      // get the current value of the field
      SerializedProperty layerNumberProperty = property.FindPropertyRelative("_layerBit");

      // find out which one should be selected by default
      var allLayers = Layer.GetAllLayers();
      var layerNames = allLayers.Select(l => l.Name).ToArray();

      var layerIndex = Array.FindIndex(allLayers, l => l.LayerId == layerNumberProperty.intValue);

      // use 0 if the current layer was not found
      if (layerIndex == -1)
      {
        layerIndex = 0;
      }

      // get the name of the property
      var niceName = ObjectNames.NicifyVariableName(property.name);

      layerIndex = EditorGUI.Popup(position, niceName, layerIndex, layerNames);

      // store the id of the layer that was selected
      layerNumberProperty.intValue = allLayers[layerIndex].LayerId;
    }
  }
}