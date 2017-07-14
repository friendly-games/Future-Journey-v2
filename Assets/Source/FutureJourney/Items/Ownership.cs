using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Holds a behavior instance and logic for the given behavior. </summary>
  public struct Ownership<TProgramming, TBehavior>
    where TBehavior : class
    where TProgramming : class
  {
    private GameObject _instance;
    private TBehavior _behavior;
    private TProgramming _programming;

    public Ownership(GameObject owner)
    {
      Owner = owner;
      _instance = null;
      _behavior = null;
      _programming = null;
    }

    public bool IsValid
      => _instance != null;

    
    /// <summary> The game object the <see cref="Instance"/> should be attached to. </summary>
    public GameObject Owner { get; }

    /// <summary>
    ///  The game object which represents an instance of the template created by the given
    ///  <see cref="Programming"/>
    /// </summary>
    public GameObject Instance 
      => _instance;

    /// <summary> The behavior which provides the logic for the <see cref="Behavior"/>. </summary>
    public TProgramming Programming
      => _programming;

    /// <summary> The behavior queried from <see cref="Instance"/>. </summary>
    public TBehavior Behavior
      => _behavior;

    /// <summary> Assigns the given values to this instance.. </summary>
    public void Assign(TProgramming programming, GameObject instance) 
      => Assign(programming, instance, instance.GetComponent<TBehavior>());

    /// <summary> Assigns the given values to this instance.. </summary>
    public void Assign(TProgramming programming, GameObject instance, TBehavior behavior)
    {
      _instance = instance;
      _programming = programming;
      _behavior = behavior;
    }

    /// <summary>
    ///  Removes the <see cref="Instance"/> from <see cref="Owner"/> and cleans up all other fields.
    /// </summary>
    public void Destroy()
    {
      if (!IsValid)
        return;

      UnityExtensions.Destroy(_instance);
      _instance = null;
      _programming = null;
      _behavior = null;
    }
  }
}