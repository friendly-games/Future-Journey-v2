using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common.Structures;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace NineBitByte.Common
{
  public static class UnityExtensions
  {
    // Clone an existing unity object
    public static T CreateInstance<T>(this T original)
      where T : UnityObject 
      => UnityObject.Instantiate(original);

    // Clone an existing unity object with a specific position and rotation
    public static T CreateInstance<T>(this T original, PositionAndRotation position)
      where T : UnityObject
      => UnityObject.Instantiate(original, position.Position, position.Rotation);

    // Clone an existing unity object with a specific position and rotation and owner
    public static T CreateInstance<T>(this T original, Transform owner, PositionAndRotation position)
      where T : UnityObject
      => UnityObject.Instantiate(original, position.Position, position.Rotation, owner);

    // Destroy an existing unity object
    public static void Destroy(GameObject gameObject) 
      => UnityObject.Destroy(gameObject);
  }
}