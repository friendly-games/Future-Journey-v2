using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Common.Statistics
{
  /// <summary>
  ///   All of the currently defined stats in the world.
  /// </summary>
  public static class KnownStats
  {
    public static readonly StatisticIdentifier<DoubleStatistic> DamageDone
      = new StatisticIdentifier<DoubleStatistic>
        {
          Guid = Guid.Parse("C2E7519D-997E-4898-B50E-EE9248263009"),
          Description = "Total done to enemies",
        };

    public static readonly StatisticIdentifier<DoubleStatistic> DamageReceived
      = new StatisticIdentifier<DoubleStatistic>
        {
          Guid = Guid.Parse("9B4EEE16-BBAD-4955-B515-AA472DBC54D4"),
          Description = "Total damage received from enemies",
        };
  }
}