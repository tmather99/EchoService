using System;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog;

namespace NetCoreClient
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "WCF .Net Core Client";

            try
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(Wcf.Client.SEQ_SERVER_URL)
                .CreateLogger();

                Log.Information($"SEQ_SERVER_URL={Wcf.Client.SEQ_SERVER_URL}");

                await Wcf.Client.CallBasicHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
                //await CallBasicHttpBinding($"https://{ECHO_SERVER}:{HTTPS_PORT}");

                await Wcf.Client.CallWsHttpBinding($"http://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.HTTP_PORT}");
                //await CallWsHttpBinding($"https://{ECHO_SERVER}:{HTTPS_PORT}");

                await Wcf.Client.CallNetTcpBinding($"net.tcp://{Wcf.Client.ECHO_SERVER}:{Wcf.Client.NETTCP_PORT}");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
