using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> The controller for overall game options. </summary>
  public class OverallGameBehavior : BaseBehavior
  {
    public bool HideCursorOnStartup
      = true;

    public void Start()
    {
      if (HideCursorOnStartup)
      {
        HideCursor = true;
      }
    }

    public bool HideCursor
    {
      get { return !Cursor.visible; }
      set
      {
        if (value)
        {
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
        }
        else
        {
          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;
        }
      }
    }
  }
}