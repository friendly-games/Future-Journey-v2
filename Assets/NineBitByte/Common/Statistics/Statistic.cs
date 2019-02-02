using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   Base class for a value to track to inform the player of their progress.
  /// </summary>
  public interface IStatistic
  {
    /// <summary>
    ///   Resets the value
    /// </summary>
    void Reset();
    
    /// <summary>
    ///   The container that owns the given statistic.
    /// </summary>
    IStatisticContainer Owner { get; set; }
  }
}