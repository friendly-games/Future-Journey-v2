using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace NineBitByte.Assets.Source.Common
{
  /// <summary> Designates a specific layer. </summary>
  [Serializable]
  public struct Layer : IEquatable<Layer>
  {
    /// <summary> The default layer (LayerId=0). </summary>
    public static readonly Layer Default = new Layer(0);

    [SerializeField]
    [UsedImplicitly]
    [FormerlySerializedAs("LayerBit")]
    private int _layerBit;

    /// <summary> Creates a new layer object. </summary>
    /// <param name="layerId"> The id of the layer in the unity system. </param>
    private Layer(int layerId)
    {
      _layerBit = layerId;
    }

    /// <summary> Gets or sets the layer id that unity uses to identify the layer. </summary>
    public int LayerId
    {
      get { return _layerBit; }
      set { _layerBit = value; }
    }

    /// <summary> Gets the name of the layer. </summary>
    public string Name
    {
      get { return LayerMask.LayerToName(LayerId); }
      set { _layerBit = LayerMask.NameToLayer(value); }
    }

    /// <summary> Check if the given layer is contained in the layer mask. </summary>
    /// <param name="layerMask"> The layer mask to check. </param>
    /// <returns> True if the layer is contained in the layer mask, false otherwise </returns>
    public bool IsIn(LayerMask layerMask)
    {
      return (layerMask.value & LayerMaskValue) > 0;
    }

    /// <summary> A mask representation of the layer if it were turned into a layer mask. </summary>
    public int LayerMaskValue
    {
      get { return (1 << _layerBit); }
    }

    /// <summary> Tests if this Layer is considered equal to another. </summary>
    /// <param name="other"> The layer to compare to this object. </param>
    /// <returns> true if the objects are considered equal, false if they are not. </returns>
    public bool Equals(Layer other)
    {
      return _layerBit == other._layerBit;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is Layer && Equals((Layer)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      return _layerBit;
    }

    /// <summary> Equality operator. </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==(Layer left, Layer right)
    {
      return left.Equals(right);
    }

    /// <summary> Inequality operator. </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=(Layer left, Layer right)
    {
      return !left.Equals(right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return Name;
    }

    /// <summary> Create a new layer from a layer id </summary>
    /// <param name="layerId"> The layer id of the layer to wrap. </param>
    /// <returns> A layer representing the given layer id. </returns>
    public static Layer FromLayerId(int layerId)
    {
      return new Layer(layerId);
    }

    /// <summary> Create a new layer from a layer name </summary>
    /// <param name="name"> The layer name of the layer to wrap. </param>
    /// <returns> A layer representing the given layer name. </returns>
    public static Layer FromName(string name)
    {
      return new Layer(LayerMask.NameToLayer(name));
    }

    /// <summary> Gets all of the layers that unity knows about. </summary>
    /// <returns> An array of all of the layers in unity. </returns>
    public static Layer[] GetAllLayers()
    {
      return Enumerable.Range(0, 32)
                       .Where(i => !String.IsNullOrEmpty(LayerMask.LayerToName(i)))
                       .Select(i => new Layer(i))
                       .ToArray();
    }
  }
}
