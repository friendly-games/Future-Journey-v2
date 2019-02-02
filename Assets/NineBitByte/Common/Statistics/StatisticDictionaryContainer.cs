using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   Contains statistics backed by a dictionary.
  /// </summary>
  public class StatisticDictionaryContainer : IStatisticContainer,
                                              IEnumerable
  {
    private readonly Dictionary<int, IStatistic> _knownStatistics;

    /// <summary />
    public StatisticDictionaryContainer()
    {
      _knownStatistics = new Dictionary<int, IStatistic>();
    }

    /// <summary>
    ///   Reserves room for the given statistic.
    /// </summary>
    public T Add<T>(StatisticIdentifier<T> descriptor)
      where T : IStatistic, new()
    {
      var stat = descriptor.Create(this);
      _knownStatistics[descriptor.LocalId] = stat;
      return stat;
    }
    
    /// <summary>
    ///   Attempts to retrieve the given statistic, returning null if it was not found.
    /// </summary>
    public T TryGetStatistic<T>(StatisticIdentifier<T> identifier)
      where T : IStatistic, new()
    {
      if (!_knownStatistics.TryGetValue(identifier.LocalId, out var stat))
      {
        return default(T);
      }

      return (T)stat;
    }

    /// <summary>
    ///  Resets all statistics in this container.
    /// </summary>
    public void Reset()
    {
      foreach (var stats in _knownStatistics.Values)
      {
        stats.Reset();
      }
    }

    /// <summary>
    ///   Notifies all listeners that the given statistic has changed.
    /// </summary>
    public void NotifyChanged(IStatistic statistic)
      => StatisticChanged?.Invoke(this, statistic);

    /// <inheritdoc />
    public event Action<IStatisticContainer, IStatistic> StatisticChanged;

    /// <inheritdoc />
    public IEnumerator GetEnumerator()
      => _knownStatistics.GetEnumerator();
  }
}