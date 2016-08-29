using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTrack.Web.Tests
{
    [TestClass]
    public class LoadControllerTests
    {
        [TestMethod]
        public async Task Post_and_retrieve_data()
        {
            var client = TestUtilities.CreateTestHttpClient();

            (await client.PostAsJsonAsync(
                "/api/Load/server1",
                new LoadController.CpuAndRamLoad
                {
                    CpuLoad = 0.10,
                    RamLoad = 0.20,
                })).EnsureSuccessStatusCode();

            var response = (await client.GetAsync("/api/Load/server1")).EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<LoadController.LoadSummary>();

            result.Last24Hours.Single().AverageCpuLoad.Should().Be(0.10);
            result.Last24Hours.Single().AverageRamLoad.Should().Be(0.20);
        }

        [TestMethod]
        public void Data_is_persisted_across_controller_instances()
        {
            new LoadController().Post("server1", new LoadController.CpuAndRamLoad {CpuLoad = 0.10, RamLoad = 0.20,});
            var result = ((OkNegotiatedContentResult<LoadController.LoadSummary>) new LoadController().Get("server1")).Content;
            result.Last24Hours.Should().ContainSingle();
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