using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http;

namespace WebApplication1
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
            TimeStampedCpuAndRamLoadByServerName.GetOrAdd(serverName).Add(new TimeStamped<CpuAndRamLoad>(DateTimeOffset.Now, body));
        }

        [Route("{serverName}")]
        public IHttpActionResult Get(string serverName)
        {
            if (!TimeStampedCpuAndRamLoadByServerName.ContainsKey(serverName))
            {
                return NotFound();
            }

            var last60Minutes = LoadReportGenerator.SummarizeByTimeBin(TimeStampedCpuAndRamLoadByServerName[serverName],
                DateTimeOffset.Now - TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(1));

            var last24Hours = LoadReportGenerator.SummarizeByTimeBin(TimeStampedCpuAndRamLoadByServerName[serverName],
                DateTimeOffset.Now - TimeSpan.FromHours(24), TimeSpan.FromHours(1));

            return Ok(new
            {
                last60Minutes,
                last24Hours,
            });
        }

        private static readonly Dictionary<string, List<TimeStamped<CpuAndRamLoad>>> TimeStampedCpuAndRamLoadByServerName =
            new Dictionary<string, List<TimeStamped<CpuAndRamLoad>>>();

        public class TimeBinAndAverageCpuAndRamLoad
        {
            public DateTimeOffset TimeBin { get; set; }
            public double AverageCpuLoad { get; set; }
            public double AverageRamLoad { get; set; }
        }
    }
}