using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Limits the rate at which something can occur. </summary>
  public class RateLimiter
  {
    private float _rechargeRate;

    [NonSerialized]
    private float _lastTime;

    public RateLimiter(bool allowFirst = false)
      : this(TimeSpan.FromSeconds(0))
    {
      _lastTime = allowFirst ? float.MinValue : 0;
    }

    /// <summary> Constructor. </summary>
    /// <param name="rechargeRate"> The time it takes for an action to "recharge". </param>
    public RateLimiter(TimeSpan rechargeRate)
    {
      RechargeRate = rechargeRate;
      _lastTime = 0;
    }

    /// <summary> The rate at which the limiter expires and can be used again. </summary>
    public TimeSpan RechargeRate
    {
      get { return TimeSpan.FromSeconds(_rechargeRate); }
      set { _rechargeRate = (float)value.TotalSeconds; }
    }

    /// <summary> True if the item can be re-triggered. </summary>
    public bool CanRestart 
      => (Time.time - _lastTime) > _rechargeRate;

    /// <summary>
    ///  A value between 0 and 1 determining how close we are to being able to Restart.  1 is ready.
    /// </summary>
    public float PercentComplete
    {
      get
      {
        if (RechargeRate.TotalMilliseconds == 0)
          return 1;
        
        return Mathf.Clamp(Time.time - _lastTime, 0, _rechargeRate) / _rechargeRate;
      }
    }

    /// <summary> Attempts to trigger the item. </summary>
    public void Restart()
    {
      _lastTime = Time.time;
    }

    /// <summary> Invokes <see cref="Restart"/> if <see cref="CanRestart"/> is true. </summary>
    public bool TryRestart()
    {
      if (!CanRestart)
        return false;

      Restart();
      return true;
    }
  }
}