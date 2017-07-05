using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Assets.Source.FutureJourney.Items
{
  /// <summary> A simple structure that holds both the position and rotation of an item. </summary>
  public struct PositionAndRotation
  {
    /// <summary> The position of the item. </summary>
    public Vector3 Position { get; }
    /// <summary> The rotation of the item. </summary>
    public Quaternion Rotation { get; }

    /// <summary> Uses the position and rotation from the given transform. </summary>
    public PositionAndRotation(Transform transform)
    {
      Position = transform.position;
      Rotation = transform.rotation;
    }

    /// <summary>
    ///  Uses the position and rotation from the given transform, applying the
    ///  <paramref name="flatOffset"/> to the position in local space.
    /// </summary>
    public PositionAndRotation(Transform transform, Vector3 flatOffset)
    {
      Position = transform.rotation * flatOffset + transform.position;
      Rotation = transform.rotation;
    }
  }

  [Serializable]
  public struct RelativeOffset
  {
    public const string SerializedFieldName = nameof(_offset);

    [SerializeField]
    private Vector3 _offset;

    public Vector3 Offset
    {
      get { return _offset; }
      set { _offset = value; }
    }

    public PositionAndRotation ToLocation(Transform transform)
      => new PositionAndRotation(transform, _offset);
  }
}