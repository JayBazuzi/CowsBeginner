﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApplication1.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task SanityTest()
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            var httpClient = new HttpClient(new HttpServer(configuration));

            IEnumerable<string> results;
            (await httpClient.GetAsync("http://example.com/api/values")).EnsureSuccessStatusCode()
                .TryGetContentValue(out results)
                .Should().BeTrue();
            results.Should().BeEquivalentTo("value1", "value2");
        }
    }
}