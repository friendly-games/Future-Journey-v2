using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Assets.Source.FutureJourney.Items;
using UnityEngine;

namespace NineBitByte.Assets.Source
{
  public static class UnityExtensions
  {
    public static T CreateInstance<T>(this T original)
      where T : UnityEngine.Object 
      => UnityEngine.Object.Instantiate(original);

    public static T CreateInstance<T>(this T original, Vector3 position, Quaternion rotation)
      where T : UnityEngine.Object
      => UnityEngine.Object.Instantiate(original, position, rotation);

    public static T CreateInstance<T>(this T original, PositionAndRotation position)
      where T : UnityEngine.Object
      => UnityEngine.Object.Instantiate(original, position.Position, position.Rotation);

    public static T CreateInstance<T>(this T original, Transform owner, PositionAndRotation position)
      where T : UnityEngine.Object
      => UnityEngine.Object.Instantiate(original, position.Position, position.Rotation, owner);

    public static T CreateInstance<T>(this T original, Transform owner, Vector3 position, Quaternion rotation)
      where T : UnityEngine.Object
    {
      return UnityEngine.Object.Instantiate(original, position, rotation, owner);
    }

    public static void Destroy(GameObject gameObject)
    {
      UnityEngine.Object.Destroy(gameObject);
    }
  }
}