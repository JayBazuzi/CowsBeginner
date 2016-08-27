using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;

namespace WebApplication1.Tests
{
    [TestClass]
    public class Record_load_for_a_given_server
    {
        [TestMethod]
        public async Task Post_()
        {
            var client = TestUtilities.CreateTestHttpClient();

            (await client.PostAsJsonAsync("/api/Load/server1",
                new LoadController.CpuAndRamLoad
                {
                    CpuLoad = 0.10,
                    RamLoad = 0.20,
                }
                )).EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task Post_without_body_should_return_BadRequest()
        {
            var client = TestUtilities.CreateTestHttpClient();

            (await client.PostAsync("/api/Load/server1", null))
                .StatusCode
                .Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Post_missing_body_members_should_return_BadRequest()
        {
            var client = TestUtilities.CreateTestHttpClient();

            (await client.PostAsJsonAsync("/api/Load/server1", new LoadController.CpuAndRamLoad()))
                .StatusCode
                .Should().Be(HttpStatusCode.BadRequest);
        }
    }
}