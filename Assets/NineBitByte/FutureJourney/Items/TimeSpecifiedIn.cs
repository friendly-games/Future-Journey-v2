using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.Items
{
  public enum TimeSpecifiedIn
  {
    Milliseconds,
    Seconds,
    Minutes,
  }

  public static class TimeSpecifiedInExtensions
  {
    public static void GetInfo(
      this TimeSpecifiedIn value,
      out float multiplier,
      out string unitLabel)
    {
      switch (value)
      {
        case TimeSpecifiedIn.Milliseconds:
          multiplier = 1;
          unitLabel = "ms";
          break;
        case TimeSpecifiedIn.Seconds:
          multiplier = 1000;
          unitLabel = "sec";
          break;
        case TimeSpecifiedIn.Minutes:
          multiplier = 1000 * 60;
          unitLabel = "min";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}