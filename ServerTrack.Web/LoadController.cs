﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;

namespace ServerTrack.Web
{
    [CreateErrorResponseIfInvalidModelState]
    [CreateErrorResponseIfUnsetActionArguments]
    [RoutePrefix("api/Load")]
    public class LoadController : ApiController
    {
        public class CpuAndRamLoad
        {
            [Required]
            public double? CpuLoad { get; set; }

            [Required]
            public double? RamLoad { get; set; }
        }

        [Route("{serverName}")]
        public void Post(string serverName, [FromBody] CpuAndRamLoad body)
        {
            TimeStampedCpuAndRamLoadByServerName.GetOrAdd(
                    serverName,
                    _ => new TimeLimitedList<CpuAndRamLoad>()
                )
                .AddAndRemoveOld(
                    DateTimeOffset.Now,
                    body,
                    DateTimeOffset.Now - maxDataAge);
        }

        public class LoadSummary
        {
            public IEnumerable<LoadController.TimeBinAndAverageCpuAndRamLoad> last60Minutes { get; set; }
            public IEnumerable<LoadController.TimeBinAndAverageCpuAndRamLoad> last24Hours { get; set; }
        }

        [Route("{serverName}")]
        public IHttpActionResult Get(string serverName)
        {
            if (!TimeStampedCpuAndRamLoadByServerName.ContainsKey(serverName))
            {
                return NotFound();
            }

            var last60Minutes = LoadReportGenerator.SummarizeByTimeBin(
                TimeStampedCpuAndRamLoadByServerName[serverName],
                DateTimeOffset.Now - TimeSpan.FromMinutes(60),
                TimeSpan.FromMinutes(1));

            var last24Hours = LoadReportGenerator.SummarizeByTimeBin(
                TimeStampedCpuAndRamLoadByServerName[serverName],
                DateTimeOffset.Now - TimeSpan.FromHours(24),
                TimeSpan.FromHours(1));

            return Ok(
                new LoadSummary
                {
                    last60Minutes = last60Minutes,
                    last24Hours = last24Hours,
                });
        }

        private static readonly ConcurrentDictionary<string, TimeLimitedList<CpuAndRamLoad>> TimeStampedCpuAndRamLoadByServerName
            =
            new ConcurrentDictionary<string, TimeLimitedList<CpuAndRamLoad>>();

        private static readonly TimeSpan maxDataAge = TimeSpan.FromHours(25);

        public class TimeBinAndAverageCpuAndRamLoad
        {
            public DateTimeOffset TimeBin { get; set; }
            public double AverageCpuLoad { get; set; }
            public double AverageRamLoad { get; set; }
        }
    }
}