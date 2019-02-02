using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common.Statistics
{
  /// <summary> A container that contains no stats </summary>
  public class EmptyStatisticContainer : IStatisticContainer
  {
    public T TryGetStatistic<T>(StatisticIdentifier<T> identifier)
      where T : IStatistic, new()
    {
      return default(T);
    }

    /// <inheritdoc />
    public void Reset()
    {
      // no-op
    }

    /// <inheritdoc />
    public void NotifyChanged(IStatistic statistic)
    {
      StatisticChanged?.Invoke(this, statistic);
    }

    public event Action<IStatisticContainer, IStatistic> StatisticChanged;
  }
}