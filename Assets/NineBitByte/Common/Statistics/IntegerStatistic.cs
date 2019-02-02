using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   A statistic that holds an integer value.
  /// </summary>
  public class IntegerStatistic : IStatistic
  {
    public int Value { get; private set; }

    /// <inheritdoc />
    public void Reset()
      => Value = 0;

    /// <inheritdoc />
    public IStatisticContainer Owner { get; set; }

    public void Increment(int amount = 1)
    {
      Owner.NotifyChanged(this);
      Value += amount;
    }
  }
}