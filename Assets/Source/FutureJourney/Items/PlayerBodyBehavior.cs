using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary>
  ///  The behavior that is actually on the player object that rotations and moves.
  /// </summary>
  public class PlayerBodyBehavior : BaseBehavior
  {
    [Tooltip("The location to which the weapon should be placed on the player")]
    public RelativeOffset WeaponOffset;

    private Quaternion _lastRotation
      = Quaternion.identity;

    private PlayerInputHandler _inputHandler;

    public void Initalize(PlayerInputHandler inputHandler)
    {
      _inputHandler = inputHandler;
    }

    public void FixedUpdate()
    {
      transform.rotation = Quaternion.Lerp(_lastRotation, _inputHandler.DesiredRotation, .8f);
      _lastRotation = transform.rotation;
    }
  }
}