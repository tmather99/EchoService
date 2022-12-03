using BenchmarkDotNet.Attributes;
using Dapr.Client;
using GloboTicket.Frontend.Models.Api;
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
        public async Task DaprStateStore()
        {
            using var daprClient = new DaprClientBuilder().Build();
            var data = Program.CreateExampleContract();
            await daprClient.SaveStateAsync<ExampleContract>(storeName: "shopstate", key: "example", value: data);
            var result = await daprClient.GetStateAsync<ExampleContract>(storeName: "shopstate", key: "example");
            Log.Information(Program.JsonSerialize(result));
        }

        [Benchmark]
        [TestMethod]
        public async Task DaprGetSecrets()
        {
            try
            {
                using var daprClient = new DaprClientBuilder().Build();
                var secretStoreName = Environment.GetEnvironmentVariable("SECRET_STORE_NAME") ?? "secretstore";
                var secrets = await daprClient.GetBulkSecretAsync(storeName: secretStoreName);
                secrets.OrderBy(o => o.Key).ToList().ForEach(secret => Log.Information($"{secret.Key} = {secret.Value.Values.First()}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Benchmark]
        [TestMethod]
        public async Task DaprStatePublishOrder()
        {
            var guid = Guid.NewGuid();
            var order = new OrderForCreation()
            {
                Date = DateTimeOffset.Now,
                OrderId = guid,
                Lines = new List<OrderLine>()
                {
                    new OrderLine()
                    {
                        EventId = guid,
                        EventName = "Event.Name",
                        ArtistName = "Event.Artist",
                        Price = 1,
                        TicketCount = 1
                    }
                },
                CustomerDetails = new CustomerDetails()
                {
                    Address = "Address",
                    CreditCardNumber = "CreditCard",
                    Email = "user@email.com",
                    Name = $"Name_{guid}",
                    PostalCode = "PostalCode",
                    Town = "Town",
                    CreditCardExpiryDate = "CreditCardDate"
                }
            };

            using var daprClient = new DaprClientBuilder().Build();
            await daprClient.PublishEventAsync<OrderForCreation>(pubsubName: "pubsub", topicName: "orders", data: order);
            Log.Information($"Publihsed orderId={order.OrderId}");
        }

        [Benchmark]
        [TestMethod]
        public async Task WebApiGetEvents()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}");
            var request = new RestRequest("event");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task WebApiGetEventById()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}/event");
            var request = new RestRequest("cfb88e29-4744-48c0-94fa-b25b92dea317");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }


        [Benchmark]
        [TestMethod]
        public async Task DaprSDKGetEvents()
        {
            using var daprClient = new DaprClientBuilder().Build();
            var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appId: "catalog", methodName: "event");
            var respone = await daprClient.InvokeMethodWithResponseAsync(request);
            var content = await respone.Content.ReadAsStringAsync();
            Log.Information(content);
        }

        [Benchmark]
        [TestMethod]
        public async Task RestClientGetEvents()
        {
            var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}");
            var request = new RestRequest("event");
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "catalog");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task DaprSDKGetEventById()
        {
            using var daprClient = new DaprClientBuilder().Build();
            var request = daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get, appId: "catalog", methodName: $"event/{Guid.NewGuid()}");
            var respone = await daprClient.InvokeMethodWithResponseAsync(request);
            var content = await respone.Content.ReadAsStringAsync();
            Log.Information(content);
        }

        [Benchmark]
        [TestMethod]
        public async Task RestClientGetEventById()
        {
            var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}/event");
            var request = new RestRequest(Guid.NewGuid().ToString());
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "catalog");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}