using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
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