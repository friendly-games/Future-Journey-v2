using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
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
}