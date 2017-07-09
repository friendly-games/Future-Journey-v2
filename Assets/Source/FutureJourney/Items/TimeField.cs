using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Wrapper around timespan. </summary>
  [Serializable]
  public struct TimeField
  {
    private const int TicksPerMillesecond = 10000;

    public const string NameOfSerializedField = nameof(_timeValue);

    /// <summary> Field represents an amount of time in milliseconds </summary>
    [SerializeField]
    private long _timeValue;

    /// <summary> Constructor. </summary>
    /// <param name="timespan"> The length of time that the field represents. </param>
    public TimeField(TimeSpan timespan)
    {
      _timeValue = (long)timespan.TotalMilliseconds;
    }

    /// <summary> The rate at which the limiter expires and can be used again. </summary>
    public TimeSpan Time
    {
      get { return new TimeSpan(_timeValue * TicksPerMillesecond); }
      set { _timeValue = (long)value.TotalMilliseconds; }
    }

    /// <summary> Create a RateLimiter based on the amount of time in this object. </summary>
    /// <returns> A rate limiter that lasts as long as this time field. </returns>
    public RateLimiter ToRateLimiter()
    {
      return new RateLimiter(Time);
    }
  }

  /// <summary> Attribute for time field. </summary>
  public class TimeFieldAttribute : Attribute
  {
    public static readonly TimeFieldAttribute Default
      = new TimeFieldAttribute(TimeSpecifiedIn.Milliseconds);

    public TimeFieldAttribute(TimeSpecifiedIn specifiedIn)
    {
      SpecifiedIn = specifiedIn;
    }

    public int Minimum { get; set; }
      = 0;

    public int Maximum { get; set; }
      = 10000;

    public TimeSpecifiedIn SpecifiedIn { get; }
  }
}