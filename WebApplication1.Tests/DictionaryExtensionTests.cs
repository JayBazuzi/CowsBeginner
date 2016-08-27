using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApplication1.Tests
{
    [TestClass]
    public class DictionaryExtensionTests
    {
        [TestMethod]
        public void GetOrAdd_adds_a_missing_value()
        {
            var result = new Dictionary<string, int>()
                .GetOrAdd("foo", () => 42);

            result.Should().Be(42);
        }

        [TestMethod]
        public void GetOrAdd_returns_an_existing_value()
        {
            var result = new Dictionary<string, int> {{"foo", 42}}
                .GetOrAdd("foo", () => 7);

            result.Should().Be(42);
        }
    }
}