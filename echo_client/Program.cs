using System;
using System.Threading.Tasks;

namespace NetCoreClient
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "WCF .Net Core Client";

            await Wcf.Client.CallBasicHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
            //await CallBasicHttpBinding($"https://{ECHO_SERVER}:{HTTPS_PORT}");
            
            await Wcf.Client.CallWsHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
            //await CallWsHttpBinding($"https://{ECHO_SERVER}:{HTTPS_PORT}");
            
            await Wcf.Client.CallNetTcpBinding($"net.tcp://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.NETTCP_PORT}");
        }
    }
}
