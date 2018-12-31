using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Handles the input for the player. </summary>
  public class PlayerInputHandler
  {
    /// <summary> How fast the player would like to be moving. </summary>
    public Vector2 DesiredVelocity;

    /// <summary> The location at which the mouse/player is pointing/looking. </summary>
    public Vector3 DesiredTrackerLocation;

    /// <summary>
    ///  The desired rotation of the player (to look at <see cref="DesiredTrackerLocation"/>.
    /// </summary>
    public Quaternion DesiredRotation;

    /// <summary> Re-sample all input and recalculate the above variables. </summary>
    public void Recalculate()
    {
      DesiredVelocity = CalculateMovementVector();
      DesiredTrackerLocation = CalculateRotation();
      DesiredRotation = Quaternion.FromToRotation(Vector2.up, DesiredTrackerLocation);
    }

    private Vector2 CalculateMovementVector()
    {
      var movement = new Vector2();

      if (Input.GetKey(KeyCode.A))
      {
        movement.x -= 1;
      }

      if (Input.GetKey(KeyCode.D))
      {
        movement.x += 1;
      }

      if (Input.GetKey(KeyCode.W))
      {
        movement.y += 1;
      }

      if (Input.GetKey(KeyCode.S))
      {
        movement.y -= 1;
      }

      if (movement.sqrMagnitude > 0)
      {
        movement.Normalize();
      }

      return movement * 3;
    }

    private Vector3 CalculateRotation()
    {
      var maxDiff = 3;
      float xDiff = Mathf.Clamp(Input.GetAxisRaw("Mouse X") / maxDiff, -maxDiff, maxDiff);
      float yDiff = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") / maxDiff, -maxDiff, maxDiff);

      var diff = new Vector3(xDiff, yDiff);

      var newPosition = DesiredTrackerLocation + diff;

      var maxRange = 3;
      newPosition.x = Mathf.Clamp(newPosition.x, -maxRange, maxRange);
      newPosition.y = Mathf.Clamp(newPosition.y, -maxRange, maxRange);

      return newPosition;
    }
  }
}