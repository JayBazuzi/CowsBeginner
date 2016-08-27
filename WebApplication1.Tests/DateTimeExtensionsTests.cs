using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApplication1.Tests
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void Floor()
        {
            DateTimeOffset.Parse("2016-02-03 11:22").FloorBy(TimeSpan.FromDays(1))
                .Should().Be(DateTimeOffset.Parse("2016-02-03"));
        }

        [TestMethod]
        public void Floor_with_offset()
        {
            DateTimeOffset.Parse("2016-02-03 +10:00").FloorBy(TimeSpan.FromDays(1))
                .Should().Be(DateTimeOffset.Parse("2016-02-03 +10:00"));
        }
    }
}