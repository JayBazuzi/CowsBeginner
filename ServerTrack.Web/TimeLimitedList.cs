using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ServerTrack.Web
{
    public class TimeLimitedList<T> : IEnumerable<TimeStamped<T>>
    {
        readonly ConcurrentDictionary<TimeStamped<T>, object> data = new ConcurrentDictionary<TimeStamped<T>, object>();

        public IEnumerator<TimeStamped<T>> GetEnumerator()
        {
            return this.data.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddAndRemoveOld(DateTimeOffset timeStamp, T item, DateTimeOffset olderThan)
        {
            data.TryAdd(new TimeStamped<T>(timeStamp, item), null);

            var oldItems = data.Keys.Where(_ => _.TimeStamp < olderThan).ToList();
            foreach (var oldItem in oldItems)
            {
                object dummy;
                data.TryRemove(oldItem, out dummy);
            }
        }
    }
}