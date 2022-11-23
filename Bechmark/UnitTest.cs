using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bechmark
{
    [MemoryDiagnoser]
    [TestClass]
    public class UnitTest
    {
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
    }
}