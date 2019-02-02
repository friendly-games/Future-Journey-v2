using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   General purpose identifier for a specific type of statistic.
  /// </summary>
  public abstract class StatisticIdentifier
  {
    private static int _currentId = 1;

    protected StatisticIdentifier()
    {
      LocalId = Interlocked.Increment(ref _currentId);
    }
    
    /// <summary>
    ///  The global id for the statistic.  This CANNOT change between instance of the game. 
    /// </summary>
    public Guid Guid { get; set; }
    
    /// <summary>
    ///   THe local id for the statistic.  This CAN change between instance of the game.
    /// </summary>
    public int LocalId { get; set; }
    
    /// <summary>
    ///  Human-readable description of what the statistic represents.
    /// </summary>
    public string Description { get; set; }
  }

  /// <summary>
  ///   Associates a <see cref="StatisticIdentifier"/> with a specific Type of statistic
  /// </summary>
  public class StatisticIdentifier<T> : StatisticIdentifier
    where T : IStatistic, new()
  {
    public T Create(IStatisticContainer container)
    {
      var stat = new T();
      stat.Owner = container;
      stat.Id = this;
      return stat;
    }
  }
}