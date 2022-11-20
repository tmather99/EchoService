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
            await NetCoreClient.Program.CallBasicHttpBinding($"http://{NetCoreClient.Program.ECHO_SERVER}:{NetCoreClient.Program.HTTP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallWsHttpBinding()
        {
            await NetCoreClient.Program.CallWsHttpBinding($"http://{NetCoreClient.Program.ECHO_SERVER}:{NetCoreClient.Program.HTTP_PORT}");
        }

        [Benchmark]
        [TestMethod]
        public async Task CallNetTcpBinding()
        {
            await NetCoreClient.Program.CallNetTcpBinding($"net.tcp://{NetCoreClient.Program.ECHO_SERVER}:{NetCoreClient.Program.NETTCP_PORT}");
        }
    }
}