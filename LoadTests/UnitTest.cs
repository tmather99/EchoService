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
            await Wcf.Client.CallBasicHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
        }

        [TestMethod]
        public async Task CallWsHttpBinding()
        {
            await Wcf.Client.CallWsHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
        }

        [TestMethod]
        public async Task CallNetTcpBinding()
        {
            await Wcf.Client.CallNetTcpBinding($"net.tcp://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.NETTCP_PORT}");
        }

        [TestMethod]
        public async Task CallCalculator()
        {
            await Wcf.Client.CallCalculator();
        }
    }
}