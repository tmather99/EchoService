using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Contract;

namespace NetCoreClient
{
    public static class Program
    {
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;
        public const int NETTCP_PORT = 8090;

        public static string ECHO_SERVER = Environment.GetEnvironmentVariable("ECHO_SERVER") ?? "localhost";
        
        static async Task Main(string[] args)
        {
            Console.Title = "WCF .Net Core Client";

            await CallBasicHttpBinding($"http://{ECHO_SERVER}:{HTTP_PORT}");
            //await CallBasicHttpBinding($"https://{ECHO_SERVER}:{HTTPS_PORT}");
            
            await CallWsHttpBinding($"http://{ECHO_SERVER}:{HTTP_PORT}");
            //await CallWsHttpBinding($"https://{ECHO_SERVER}:{HTTPS_PORT}");
            
            await CallNetTcpBinding($"net.tcp://{ECHO_SERVER}:{NETTCP_PORT}");
        }

        public static async Task CallBasicHttpBinding(string hostAddr)
        {
            var binding = new BasicHttpBinding(IsHttps(hostAddr) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/basicHttp"));
            factory.Open();
            
            try
            {
                IEchoService client = factory.CreateChannel();
                var channel = client as IClientChannel;
                channel.Open();

                string clientId = Guid.NewGuid().ToString();
                string msg = $"Hello World...BasicHttpBinding from {clientId}";
                Console.WriteLine("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        public static async Task CallWsHttpBinding(string hostAddr)
        {
            var binding = new WSHttpBinding(IsHttps(hostAddr) ? SecurityMode.Transport : SecurityMode.None);
            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/wsHttp"));
            factory.Open();
            
            try
            {
                IEchoService client = factory.CreateChannel();
                var channel = client as IClientChannel;
                channel.Open();

                string clientId = Guid.NewGuid().ToString();
                string msg = $"Hello World...WsHttpBinding from {clientId}";
                Console.WriteLine("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        public static async Task CallNetTcpBinding(string hostAddr)
        {
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            binding.TransferMode = TransferMode.Streamed;

            var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/netTcp"));
            factory.Open();
            
            try
            {
                IEchoService client = factory.CreateChannel();
                var channel = client as IClientChannel;
                channel.Open();

                string clientId = Guid.NewGuid().ToString();
                string msg = $"Hello World...NetTcpBinding from {clientId}";
                Console.WriteLine("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Console.WriteLine(result);
            }
            finally
            {
                factory.Close();
            }
        }

        private static bool IsHttps(string url)
        {
            return url.ToLower().StartsWith("https://");
        }
    }
}
