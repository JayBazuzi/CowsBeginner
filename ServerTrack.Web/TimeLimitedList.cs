using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ServerTrack.Web
{
    public class TimeLimitedList<T> : IEnumerable<TimeStamped<T>>
    {
        private readonly ConcurrentDictionary<TimeStamped<T>, object> _data = new ConcurrentDictionary<TimeStamped<T>, object>();

        public IEnumerator<TimeStamped<T>> GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddAndRemoveOld(DateTimeOffset timeStamp, T item, DateTimeOffset olderThan)
        {
            _data.TryAdd(new TimeStamped<T>(timeStamp, item), null);

            var oldItems = _data.Keys.Where(_ => _.TimeStamp < olderThan).ToList();
            foreach (var oldItem in oldItems)
            {
                object dummy;
                _data.TryRemove(oldItem, out dummy);
            }
        }
    }
}