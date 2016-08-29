using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTrack.Web.Tests
{
    [TestClass]
    public class TimeLimitedListTests
    {
        [TestMethod]
        public void Add_an_item_with_timestamp()
        {
            var dateTimeOffset = DateTimeOffset.Now;
            var subject = new TimeLimitedList<int>();
            subject.AddAndRemoveOld(dateTimeOffset, 42, olderThan: DateTimeOffset.Parse("2016-01-01"));

            subject.Should().ContainSingle()
                .Which.ShouldBeEquivalentTo(new TimeStamped<int>(dateTimeOffset, 42));
        }

        [TestMethod]
        public void Adding_an_item_makes_old_items_go_away()
        {
            var subject = new TimeLimitedList<int>();
            subject.AddAndRemoveOld(
                DateTimeOffset.Now - TimeSpan.FromHours(1),
                7,
                olderThan: DateTimeOffset.Now - TimeSpan.FromHours(2));
            subject.AddAndRemoveOld(
                DateTimeOffset.Now - TimeSpan.FromHours(0.5),
                8,
                olderThan: DateTimeOffset.Now - TimeSpan.FromHours(2));
            subject.Should().HaveCount(2);

            subject.AddAndRemoveOld(DateTimeOffset.Now, 42, DateTimeOffset.Now - TimeSpan.FromMinutes(1));

            subject.Single().Data.Should().Be(42);
        }
    }
}