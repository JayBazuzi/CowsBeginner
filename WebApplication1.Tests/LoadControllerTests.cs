using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApprovalTests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Tests
{
    [TestClass]
    public class LoadControllerTests
    {
        [TestMethod]
        public async Task Post_and_retrieve_data()
        {
            var client = TestUtilities.CreateTestHttpClient();

            (await client.PostAsJsonAsync("/api/Load/server1",
                new LoadController.CpuAndRamLoad
                {
                    CpuLoad = 0.10,
                    RamLoad = 0.20,
                })).EnsureSuccessStatusCode();

            var response = (await client.GetAsync("/api/Load/server1")).EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            content.Should().StartWith("{\"last60Minutes\":[{\"TimeBin\":\"2016");
        }

        [TestMethod]
        public async Task Get_should_return_NotFound_for_no_data()
        {
            var client = TestUtilities.CreateTestHttpClient();

            (await client.GetAsync("/api/Load/server99"))
                .StatusCode
                .Should().Be(HttpStatusCode.NotFound);
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