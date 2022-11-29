using Contract;
using CoreWCF;
using Serilog;

namespace NetCoreServer
{
    public class EchoService : IEchoService, IEchoService1, IEchoService2
    {
        public string Echo(string text)
        {
            Log.Information($"Received {text} client!");
            return $"Response for {text} client!";
        }

        public string ComplexEcho(EchoMessage text)
        {
            Log.Information($"Received {text.Text} client!");
            return $"Response for {text.Text} client!";
        }

        public string FailEcho(string text)
            => throw new FaultException<EchoFault>(new EchoFault() { Text = "WCF Fault OK" }, new FaultReason("FailReason"));

        [AuthorizeRole("CoreWCFGroupAdmin")]
        public string EchoForPermission(string echo)
        {
            return echo;
        }
    }
}
