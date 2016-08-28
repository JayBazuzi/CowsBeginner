using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication1
{
    public class TimeLimitedList<T> : IEnumerable<TimeStamped<T>>
    {
        readonly List<TimeStamped<T>> data = new List<TimeStamped<T>>();

        public IEnumerator<TimeStamped<T>> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddAndRemoveOld(DateTimeOffset timeStamp, T item, DateTimeOffset olderThan)
        {
            data.Add(new TimeStamped<T>(timeStamp, item));
            data.RemoveAll(_ => _.TimeStamp < olderThan);
        }
    }
}