using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerTrack.Web
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset FloorBy(this DateTimeOffset @this, TimeSpan timeSpam)
        {
            return new DateTimeOffset(@this.Ticks - @this.Ticks%timeSpam.Ticks, @this.Offset);
        }

        public static IEnumerable<IGrouping<DateTimeOffset, TimeStamped<T>>> GroupByTimeBin<T>(
            this IEnumerable<TimeStamped<T>> source,
            TimeSpan binSize)
        {
            return source.GroupBy(_ => _.TimeStamp.FloorBy(binSize));
        }
    }
}