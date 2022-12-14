using System;
using System.Net;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
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
        public UnitTest()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(Wcf.Client.SEQ_SERVER_URL)
                .CreateLogger();
        }

        [Benchmark]
        [TestMethod]
        public async Task CallBasicHttpBinding()
        {
            await Wcf.Client.CallBasicHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallWsHttpBinding()
        {
            await Wcf.Client.CallWsHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallSecureBasicHttpBinding()
        {
            await Wcf.Client.CallBasicHttpBinding($"https://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallSecureWsHttpBinding()
        {
            await Wcf.Client.CallWsHttpBinding($"https://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallNetTcpBinding()
        {
            await Wcf.Client.CallNetTcpBinding($"net.tcp://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.NETTCP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallNetTcpTransportBinding()
        {
            await Wcf.Client.CallNetTcpTransportBinding($"net.tcp://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.NETTCP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallNetTcpTransportWithMessageCredentialBinding()
        {
            await Wcf.Client.CallNetTcpTransportWithMessageCredentialBinding($"net.tcp://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.NETTCP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallCalculator()
        {
            await Wcf.Client.CallCalculator();
        }

        // WCF WebHttpBinding

        [Benchmark]
        [TestMethod]
        public async Task PathEndpoint()
        {
            // Calls /api/path/{param}
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/api/path");
            var request = new RestRequest($"Testing_Path_Endpoint_{Guid.NewGuid()}");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task QueryEndpoint()
        {
            // Calls /api/path/{param}
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/api/query");
            var request = new RestRequest($"?param=Testing Query Endpoint {Guid.NewGuid()}");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task BodyContract()
        {
            // Calls /api/body with a complex data structure
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/api/body");
            var request = new RestRequest();
            request.AddHeader("user-agent", "vscode-restclient");
            var data = Program.JsonSerialize<ExampleContract>(Program.CreateExampleContract());
            request.AddJsonBody(data);
            var response = await restClient.PostAsync(request);
            Log.Information(Program.JsonSerialize(response.Content));
        }


        // Dapr RestClient service invoke

        [Benchmark]
        [TestMethod]
        public async Task DaprPathEndpoint()
        {
            var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}/api/path");
            var request = new RestRequest($"Testing_Path_Endpoint_{Guid.NewGuid()}");
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "rest-server");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task DarpQueryEndpoint()
        {
            var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}/api/query");
            var request = new RestRequest($"?param=Testing Query Endpoint {Guid.NewGuid()}");
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "rest-server");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task DaprBodyContract()
        {
            var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}/api/body");
            var request = new RestRequest();
            request.AddHeader("user-agent", "vscode-restclient");
            request.AddHeader("dapr-app-id", "rest-server");
            var data = Program.JsonSerialize<ExampleContract>(Program.CreateExampleContract());
            request.AddJsonBody(data);
            var response = await restClient.PostAsync(request);
            Log.Information(Program.JsonSerialize(response.Content));
        }

        // Ingress controller requests

        [Benchmark]
        [TestMethod]
        public async Task GetVersion()
        {
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("version");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task GetEvents()
        {
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("event");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task GetEventById()
        {
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/ingress/event");
            var request = new RestRequest(Guid.NewGuid().ToString());
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task StateStore()
        {
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("statestore");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task Secrets()
        {
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("secrets");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task Publish()
        {
            var restClient = new RestClient($"{Program.API_PROTOCOL}://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("publish");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}