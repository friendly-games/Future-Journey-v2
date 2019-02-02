using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   Statistic that holds a double/floating value.
  /// </summary>
  public class DoubleStatistic : IStatistic
  {
    public double Value { get; private set; }

    public void Reset()
      => Value = 0;

    public IStatisticContainer Owner { get; set; }

    public void Increment(double amount)
    {
      Value += amount;
      Owner.NotifyChanged(this);
    }
  }
}