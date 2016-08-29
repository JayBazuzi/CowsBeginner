using System;

namespace ServerTrack.Web
{
    public class TimeStamped<T>
    {
        public readonly DateTimeOffset TimeStamp;
        public readonly T Data;

        public TimeStamped(DateTimeOffset timeStamp, T data)
        {
            Data = data;
            TimeStamp = timeStamp;
        }
    }
}