using System;
using System.Net;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Serilog;

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


        // Ingress controller requests

        [Benchmark]
        [TestMethod]
        public async Task GetEvents()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("event");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task GetEventById()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}/ingress/event");
            var request = new RestRequest(Guid.NewGuid().ToString());
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task StateStore()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("statestore");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task Secrets()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}/ingress");
            var request = new RestRequest("secrets");
            request.AddHeader("user-agent", "vscode-restclient");
            var response = await restClient.GetAsync(request);
            Log.Information(response.Content);
        }

        [Benchmark]
        [TestMethod]
        public async Task Publish()
        {
            var restClient = new RestClient($"http://{Program.API_SERVER}:{Program.API_PORT}/ingress");
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