using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoadTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task CallBasicHttpBinding()
        {
            await NetCoreClient.Program.CallBasicHttpBinding($"http://{NetCoreClient.Program.ECHO_SERVER}:{NetCoreClient.Program.HTTP_PORT}");
        }

        [TestMethod]
        public async Task CallWsHttpBinding()
        {
            await NetCoreClient.Program.CallWsHttpBinding($"http://{NetCoreClient.Program.ECHO_SERVER}:{NetCoreClient.Program.HTTP_PORT}");
        }

        [TestMethod]
        public async Task CallNetTcpBinding()
        {
            await NetCoreClient.Program.CallNetTcpBinding($"net.tcp://{NetCoreClient.Program.ECHO_SERVER}:{NetCoreClient.Program.NETTCP_PORT}");
        }
    }
}
