using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Tests
{
    public class TestUtilities
    {
        public static HttpClient CreateTestHttpClient()
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            var httpClient = new HttpClient(new HttpServer(configuration));
            return httpClient;
        }
    }
}