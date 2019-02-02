using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   Contains all of the statistics for a given object.
  /// </summary>
  public interface IStatisticContainer
  {
    /// <summary>
    ///   Attempts to retrieve the statistic associated with the given identifier, returning null if the statistic
    ///   is not present in the given container.
    /// </summary>
    [CanBeNull]
    T TryGetStatistic<T>(StatisticIdentifier<T> identifier)
      where T : IStatistic, new();

    /// <summary>
    ///   Resets all statistics in the given container. 
    /// </summary>
    void Reset();

    /// <summary>
    ///   Invoked when a given statistic has changed.
    /// </summary>
    void NotifyChanged(IStatistic statistic);

    /// <summary>
    ///   Event fired when a statistic within the container changes.
    /// </summary>
    event Action<IStatisticContainer, IStatistic> StatisticChanged;
  }
}