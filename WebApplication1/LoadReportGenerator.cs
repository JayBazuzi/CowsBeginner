using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public static class LoadReportGenerator
    {
        public static IEnumerable<LoadController.TimeBinAndAverageCpuAndRamLoad> SummarizeByTimeBin(
            IEnumerable<TimeStamped<LoadController.CpuAndRamLoad>> cpuAndRamLoads,
            DateTimeOffset start,
            TimeSpan binSize)
        {
            return cpuAndRamLoads
                .Where(_ => _.TimeStamp >= start)
                .GroupByTimeBin(binSize)
                .Select(
                    grouping => new LoadController.TimeBinAndAverageCpuAndRamLoad
                    {
                        TimeBin = grouping.Key,
                        AverageCpuLoad = grouping.Select(_ => _.Data.CpuLoad.Value).Average(),
                        AverageRamLoad = grouping.Select(_ => _.Data.RamLoad.Value).Average(),
                    });
        }
    }
}