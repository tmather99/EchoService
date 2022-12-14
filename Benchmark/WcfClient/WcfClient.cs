using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Contracts;
using Serilog;

namespace Wcf
{
    public class Client
    {
        public static int HTTP_PORT = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8088");
        public static int HTTPS_PORT = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8443");
        public static int NETTCP_PORT = int.Parse(Environment.GetEnvironmentVariable("NETTCP_PORT") ?? "8090");
        public static int TCP_PORT = int.Parse(Environment.GetEnvironmentVariable("TCP_PORT") ?? "8089");

        public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";
        public static string ECHO_SERVER = Environment.GetEnvironmentVariable("ECHO_SERVER") ?? "localhost";
        public static string TCP_SERVER = Environment.GetEnvironmentVariable("TCP_SERVER") ?? "localhost";

        private static readonly Random random = new Random(DateTime.Now.Millisecond);

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
                string msg = $"Hello World...{(IsHttps(hostAddr) ? "SecureBasicHttpBinding" : "BasicHttpBinding")} from {clientId}";
                Log.Information("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Log.Information(result);
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
                string msg = $"Hello World...{(IsHttps(hostAddr) ? "SecureWsHttpBinding" : "WsHttpBinding")} from {clientId}";
                Log.Information("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Log.Information(result);
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
                Log.Information("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Log.Information(result);
            }
            finally
            {
                factory.Close();
            }
        }

        public static async Task CallNetTcpTransportBinding(string hostAddr)
        {
            var binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.TransferMode = TransferMode.Streamed;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            var factory = new ChannelFactory<IEchoService1>(binding, new EndpointAddress($"{hostAddr}/netTcp1"));
            //factory.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, "c6779716aea1546aef89ef03a720fb6a1330629f");
            factory.Credentials.ClientCertificate.Certificate = new X509Certificate2("WcfClient/echo-local.pfx", "Th@nhy99");
            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            factory.Open();

            try
            {
                IEchoService1 client = factory.CreateChannel();
                var channel = client as IClientChannel;
                channel.Open();

                string clientId = Guid.NewGuid().ToString();
                string msg = $"Hello World...CallNetTcpTransportBinding from {clientId}";
                Log.Information("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Log.Information(result);
            }
            finally
            {
                factory.Close();
            }
        }

        public static async Task CallNetTcpTransportWithMessageCredentialBinding(string hostAddr)
        {
            hostAddr = "net.tcp://echo.local.com:8090";

            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

            var factory = new ChannelFactory<IEchoService2>(binding, new EndpointAddress($"{hostAddr}/netTcp2"));
            //factory.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, "c6779716aea1546aef89ef03a720fb6a1330629f");
            factory.Credentials.ClientCertificate.Certificate = new X509Certificate2("WcfClient/echo-local.pfx", "Th@nhy99");
            factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            factory.Open();

            try
            {
                IEchoService2 client = factory.CreateChannel();
                var channel = client as IClientChannel;
                channel.Open();

                string clientId = Guid.NewGuid().ToString();
                string msg = $"Hello World...NetTcpTransportWithMessageCredentialBinding from {clientId}";
                Log.Information("Sending " + msg);
                var result = await client.Echo(msg);
                channel.Close();
                Log.Information(result);
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

        public static async Task CallCalculator()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.Security.Mode = SecurityMode.None;

            EndpointAddress endpointAddress = new EndpointAddress($"net.tcp://{TCP_SERVER}:{TCP_PORT}/nettcp");
            var factory = new ChannelFactory<ICalculate>(binding, endpointAddress);
            factory.Open();

            Log.Information("Invoking CalculatorService at {0}", endpointAddress);

            int value1 = random.Next(1, 10);
            int value2 = random.Next(1, 10);

            // Call the Add service operation.
            ICalculate client = factory.CreateChannel();
            int result = await client.Add(value1, value2);
            Log.Information("Add({0},{1}) = {2}", value1, value2, result);

            // Call the Subtract service operation.
            result = await client.Substract(value1, value2);
            Log.Information("Subtract({0},{1}) = {2}", value1, value2, result);

            // Call the Multiply service operation.
            result = await client.multiply(value1, value2);
            Log.Information("Multiply({0},{1}) = {2}", value1, value2, result);

            //Closing the client gracefully closes the connection and cleans up resources
            ((IClientChannel)client).Close();
            Log.Information("Closed Proxy");

            EndpointAddress endpointAddress2 = new EndpointAddress($"net.tcp://{TCP_SERVER}:{TCP_PORT}/nettcp2");
            var factory2 = new ChannelFactory<ICalculate2>(binding, endpointAddress2);
            factory2.Open();

            Log.Information("Invoking CalculatorService2 at {0}", endpointAddress2);

            int value11 = random.Next(1, 10);
            int value22 = random.Next(1, 10);

            ICalculate2 client2 = factory2.CreateChannel();

            // Call the Add service operation.
            int result2 = await client2.Add2(value11, value22);
            Log.Information("Add({0},{1}) = {2}", value11, value22, result2);

            //Closing the client gracefully closes the connection and cleans up resources
            ((IClientChannel)client2).Close();
            Log.Information("Closed Proxy2");
        }
    }
}
