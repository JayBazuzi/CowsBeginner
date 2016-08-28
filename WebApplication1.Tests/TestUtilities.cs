using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication1.Tests
{
    public class TestUtilities
    {
        public static HttpClient CreateTestHttpClient()
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            var httpClient = new HttpClient(new DebugLogginMessageHandler(new HttpServer(configuration)))
            {
                BaseAddress = new Uri("http://example.com/")
            };
            return httpClient;
        }
    }

    public class DebugLogginMessageHandler : DelegatingHandler
    {
        public DebugLogginMessageHandler(DelegatingHandler httpServer)
            : base(httpServer)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Debug.WriteLine($"REQUEST URI: {request.RequestUri}");
            if (request.Content != null)
            {
                Debug.WriteLine($"REQUEST CONTENT: {await request.Content.ReadAsStringAsync()}");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.Content != null)
            {
                Debug.WriteLine($"RESPONSE CONTENT: {await response.Content.ReadAsStringAsync()}");
            }

            return response;
        }
    }
}