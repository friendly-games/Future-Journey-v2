using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;

namespace NineBitByte.FutureJourney.View
{
  /// <summary> Behavior for managing the various UI behaviors. </summary>
  public class UiManagerBehavior : BaseBehavior
  {
    [Tooltip("Prefab for health bar")]
    public GameObject HealthBarTemplate;

    public HealthBarBehavior CreateHealthBar()
    {
      var newInstance = HealthBarTemplate.CreateInstance(gameObject.transform);
      return newInstance.GetComponent<HealthBarBehavior>();
    }
  }
}