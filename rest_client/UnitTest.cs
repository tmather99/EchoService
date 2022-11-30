using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            string result = await client.PathAsync($"Testing Path Endpoint {Guid.NewGuid()}");
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

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}