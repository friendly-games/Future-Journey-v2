using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> Limits the rate at which something can occur. </summary>
  [Serializable]
  public class RateLimiter
  {
    [SerializeField]
    private float _rechargeRate;

    [NonSerialized]
    private float _lastTime;

    public RateLimiter()
      : this(TimeSpan.FromSeconds(0))
    {
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
    public bool CanTrigger
    {
      get { return (Time.time - _lastTime) > _rechargeRate; }
    }

    /// <summary> Attempts to trigger the item. </summary>
    public void Restart()
    {
      _lastTime = Time.time;
    }
  }
}