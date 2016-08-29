using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;

namespace ServerTrack.Web
{
    public class ServerName : IEquatable<ServerName>
    {
        private readonly string _value;

        public ServerName([NotNull]string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _value = value;
        }

        public bool Equals(ServerName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringComparer.OrdinalIgnoreCase.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ServerName) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_value);
        }

        public static bool operator ==(ServerName left, ServerName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ServerName left, ServerName right)
        {
            return !Equals(left, right);
        }
    }
}