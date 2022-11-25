using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Contracts;

namespace Wcf
{
    public class Client
    {
        public static int HTTP_PORT = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8088");
        public static int HTTPS_PORT = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8443");
        public static int NETTCP_PORT = int.Parse(Environment.GetEnvironmentVariable("NETTCP_PORT") ?? "8090");
        public static int TCP_PORT = int.Parse(Environment.GetEnvironmentVariable("TCP_PORT") ?? "8089");

        public static string ECHO_SERVER = Environment.GetEnvironmentVariable("ECHO_SERVER") ?? "localhost";
        public static string TCP_SERVER = Environment.GetEnvironmentVariable("TCP_SERVER") ?? "localhost";

        private static readonly Random random = new Random();

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


        public static async Task CallCalculator()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.Security.Mode = SecurityMode.None;

            EndpointAddress endpointAddress = new EndpointAddress($"net.tcp://{TCP_SERVER}:{TCP_PORT}/nettcp");
            var factory = new ChannelFactory<ICalculate>(binding, endpointAddress);
            factory.Open();

            Console.WriteLine("Invoking CalculatorService at {0}", endpointAddress);

            double value1 = random.NextDouble();
            double value2 = random.NextDouble();

            // Call the Add service operation.
            ICalculate client = factory.CreateChannel();
            double result = await client.Add(value1, value2);
            Console.WriteLine("Add({0},{1}) = {2}", value1, value2, result);

            // Call the Subtract service operation.
            result = await client.Substract(value1, value2);
            Console.WriteLine("Subtract({0},{1}) = {2}", value1, value2, result);

            // Call the Multiply service operation.
            result = await client.multiply(value1, value2);
            Console.WriteLine("Multiply({0},{1}) = {2}", value1, value2, result);

            result = await client.Action(new Inputs() { A = 100, B = 30, Operation = Inputs.OperationEnum.Multiplication });
            Console.WriteLine("Action with DataModel {0}", result);

            //Closing the client gracefully closes the connection and cleans up resources
            ((IClientChannel)client).Close();
            Console.WriteLine("Closed Proxy");

            EndpointAddress endpointAddress2 = new EndpointAddress($"net.tcp://{TCP_SERVER}:{TCP_PORT}/nettcp2");
            var factory2 = new ChannelFactory<ICalculate2>(binding, endpointAddress2);
            factory2.Open();

            Console.WriteLine("Invoking CalculatorService2 at {0}", endpointAddress2);

            double value11 = random.NextDouble();
            double value22 = random.NextDouble();

            ICalculate2 client2 = factory2.CreateChannel();

            // Call the Add service operation.
            double result2 = await client2.Add2(value11, value22);
            Console.WriteLine("Add({0},{1}) = {2}", value11, value22, result2);

            //Closing the client gracefully closes the connection and cleans up resources
            ((IClientChannel)client2).Close();
            Console.WriteLine("Closed Proxy2");
        }
    }
}
