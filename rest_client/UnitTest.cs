using BenchmarkDotNet.Attributes;
using Dapr.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Serilog;
using WebHttpClient;

namespace Bechmark
{
    [MemoryDiagnoser]
    [TestClass]
    public class UnitTest : IDisposable
    {
        // Use a single HttpClient instance for multiple requests as it will pool connections 
        private static HttpClient httpClient = new HttpClient();

        private WebAPIGeneratedWrapper client;

        public UnitTest()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(Program.SEQ_SERVER_URL)
                .CreateLogger();

            Log.Information($"SEQ_SERVER_URL={Program.SEQ_SERVER_URL}");

            // Using a wrapper generated based on the service OpenAPI definition
            this.client = new WebAPIGeneratedWrapper($"http://{Program.API_SERVER}:{Program.API_PORT}/", UnitTest.httpClient);
        }

        [Benchmark]
        [TestMethod]
        public async Task PathEndpoint()
        {
            // Calls /api/path/{param}
            string result = await client.PathAsync($"Testing_Path_Endpoint_{Guid.NewGuid()}");
            Log.Information(result);
        }

        [Benchmark]
        [TestMethod]
        public async Task QueryEndpoint()
        {
            // Calls /api/path/{param}
            string result = await client.QueryAsync($"Testing Query Endpoint {Guid.NewGuid()}");
            Log.Information(result);
        }

        [Benchmark]
        [TestMethod]
        public async Task BodyContract()
        {
            // Calls /api/body with a complex data structure
            var data = Program.CreateExampleContract();
            var result2 = await client.BodyAsync(data);
            Log.Information(Program.JsonSerialize(result2));
        }

        [Benchmark]
        [TestMethod]
        public async Task DaprStateStoreBodyContract()
        {
            using var client = new DaprClientBuilder().Build();
            var data = Program.CreateExampleContract();
            await client.SaveStateAsync<ExampleContract>(storeName: "shopstate", key: "example", value: data);
            var result = await client.GetStateAsync<ExampleContract>(storeName: "shopstate", key: "example");
            Log.Information(Program.JsonSerialize(result));
        }

        [Benchmark]
        [TestMethod]
        public async Task WebApiGetEvents()
        {
            var client = new RestClient($"http://localhost:{Program.API_PORT}");
            var request = new RestRequest("event");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await client.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task WebApiGetEventById()
        {
            var client = new RestClient($"http://localhost:{Program.API_PORT}/event");
            var request = new RestRequest("cfb88e29-4744-48c0-94fa-b25b92dea317");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await client.GetAsync(request);
            Log.Information(response.Content);
        }


        [Benchmark]
        [TestMethod]
        public async Task DaprGetEvents()
        {
            var client = new RestClient($"http://localhost:{Program.DAPR_PORT}");
            var request = new RestRequest("event");
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "catalog");
            var response = await client.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task DaprGetEventById()
        {
            var client = new RestClient($"http://localhost:{Program.DAPR_PORT}/event");
            var request = new RestRequest("cfb88e29-4744-48c0-94fa-b25b92dea317");
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "catalog");
            var response = await client.GetAsync(request);
            Log.Information(response.Content);
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}