using System;
using System.Net;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task CallCalculator()
        {
            await Wcf.Client.CallCalculator();
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}