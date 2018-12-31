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

    public static T CreateInstance<T>(this T original, Transform owner)
      where T : UnityObject
      => UnityObject.Instantiate(original, owner);

    // Clone an existing unity object with a specific position and rotation
    public static T CreateInstance<T>(this T original, PositionAndRotation position)
      where T : UnityObject
      => UnityObject.Instantiate(original, position.Position, position.Rotation);

    // Clone an existing unity object with a specific position and rotation and owner
    public static T CreateInstance<T>(this T original, Transform owner, PositionAndRotation position)
      where T : UnityObject
      => UnityObject.Instantiate(original, position.Position, position.Rotation, owner);

    /// <summary> True if the given object has been destroyed. </summary>
    public static bool IsDestroyed(this GameObject gameObject)
      => gameObject == null;

    // Destroy an existing unity object
    public static void Destroy(UnityObject gameObject) 
      => UnityObject.Destroy(gameObject);

    /// <summary>
    ///   Get a lazy-enumerated collection of game objects that are ancestors of this game object.
    /// </summary>
    public static IEnumerable<GameObject> GetAncestorsAndSelf(this GameObject instance)
    {
      var it = instance.transform;

      while (it != null)
      {
        yield return it.gameObject;
        it = it.transform.parent;
      }
    }
  }
}