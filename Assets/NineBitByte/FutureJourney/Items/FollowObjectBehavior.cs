using System;
using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;
using NineBitByte.Common;
using NineBitByte.Common.Structures;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  public class FollowObjectBehavior : BaseBehavior
  {
    [Tooltip("The object to follow")]
    public Transform Target;

    [Tooltip("The location relative to the camera where the object should appear")]
    public RelativeOffset TargetLocationRelativeToCamera
      = new RelativeOffset(new Vector3(0f, 7.5f, 0f));

    [UsedImplicitly]
    // LateUpdate is called after all Update functions have been called. This is useful to order
    // script execution. For example a follow camera should always be implemented in LateUpdate
    // because it tracks objects that might have moved inside Update. 
    public void LateUpdate()
    {
      transform.position = Target.position - TargetLocationRelativeToCamera.Offset;
    }
  }
}