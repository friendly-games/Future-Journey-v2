using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.Common.Structures
{
  [Serializable]
  public struct RelativeOffset
  {
    public const string SerializedFieldName = nameof(_offset);

    [SerializeField]
    private Vector3 _offset;

    public RelativeOffset(Vector3 offset)
    {
      _offset = offset;
    }

    public Vector3 Offset
    {
      get { return _offset; }
      set { _offset = value; }
    }

    public PositionAndRotation ToLocation(Transform transform)
      => new PositionAndRotation(transform, _offset);
  }

  /// <summary> Determines how a relative offset should be edited. </summary>
  public class RelativeOffsetTypeAttribute : Attribute
  {
    /// <summary>
    ///  True if rotation should not be taken into account when the offset is edited.
    /// </summary>
    /// <remarks>
    ///  If false (default), then when the offset is 90 and the owning game object is rotated 180, the
    ///  offset should now be located at 270.  If true, then the offset should still be at 90.
    /// </remarks>
    public bool IsRotationIndependent { get; set; }
  }
}