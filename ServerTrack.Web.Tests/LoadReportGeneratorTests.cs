using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTrack.Web.Tests
{
    [TestClass]
    public class LoadReportGeneratorTests
    {
        [TestMethod]
        public void Two_items_in_the_same_bin_are_averaged_together()
        {
            var dateTimeOffset = DateTimeOffset.Parse("2016-01-01 11:00z");
            var result = LoadReportGenerator.SummarizeByTimeBin(
                new[]
                {
                    new TimeStamped<LoadController.CpuAndRamLoad>(
                        dateTimeOffset,
                        new LoadController.CpuAndRamLoad
                        {
                            CpuLoad = 0.3,
                            RamLoad = 0.5
                        }),
                    new TimeStamped<LoadController.CpuAndRamLoad>(
                        dateTimeOffset,
                        new LoadController.CpuAndRamLoad
                        {
                            CpuLoad = 0.2,
                            RamLoad = 0.3
                        }),
                },
                start: dateTimeOffset,
                binSize: TimeSpan.FromHours(1));

            result.ShouldBeEquivalentTo(
                new[]
                {
                    new LoadController.TimeBinAndAverageCpuAndRamLoad
                    {
                        TimeBin = dateTimeOffset,
                        AverageCpuLoad = 0.25,
                        AverageRamLoad = 0.4,
                    },
                });
        }

        [TestMethod]
        public void Time_bins_start_at_a_round_number()
        {
            var dateTimeOffset = DateTimeOffset.Parse("2016-01-01 11:00z");
            var result = LoadReportGenerator.SummarizeByTimeBin(
                new[]
                {
                    new TimeStamped<LoadController.CpuAndRamLoad>(
                        dateTimeOffset + TimeSpan.FromMinutes(1),
                        new LoadController.CpuAndRamLoad
                        {
                            CpuLoad = 0.2,
                            RamLoad = 0.3
                        }),
                },
                start: dateTimeOffset,
                binSize: TimeSpan.FromHours(1));

            result.ShouldBeEquivalentTo(
                new[]
                {
                    new LoadController.TimeBinAndAverageCpuAndRamLoad
                    {
                        TimeBin = dateTimeOffset,
                        AverageCpuLoad = 0.2,
                        AverageRamLoad = 0.3,
                    },
                });
        }

        [TestMethod]
        public void Time_bins_use_UTC()
        {
            var dateTimeOffset = DateTimeOffset.Parse("2016-01-01 11:00 +1:00");
            var result = LoadReportGenerator.SummarizeByTimeBin(
                new[]
                {
                    new TimeStamped<LoadController.CpuAndRamLoad>(
                        dateTimeOffset + TimeSpan.FromMinutes(1),
                        new LoadController.CpuAndRamLoad
                        {
                            CpuLoad = 0.2,
                            RamLoad = 0.3
                        }),
                },
                start: dateTimeOffset,
                binSize: TimeSpan.FromHours(1));

            result.Single().TimeBin.Offset.Should().Be(TimeSpan.Zero);
        }

        [TestMethod]
        public void Items_before_the_start_time_are_ignored()
        {
            var dateTimeOffset = DateTimeOffset.Parse("2016-01-01 11:00z");
            var result = LoadReportGenerator.SummarizeByTimeBin(
                new[]
                {
                    new TimeStamped<LoadController.CpuAndRamLoad>(
                        dateTimeOffset - TimeSpan.FromMinutes(1),
                        new LoadController.CpuAndRamLoad
                        {
                            CpuLoad = 0.2,
                            RamLoad = 0.3
                        }),
                },
                start: dateTimeOffset,
                binSize: TimeSpan.FromHours(1));

            result.Should().BeEmpty();
        }
    }
}