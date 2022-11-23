using System;
using System.Threading;
using System.Threading.Tasks;
using Bechmark;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace TCP_client
{
    public class Program
    {
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;
        public const int NETTCP_PORT = 8090;

        public static string ECHO_SERVER = Environment.GetEnvironmentVariable("ECHO_SERVER") ?? "localhost";
        public static string TCP_SERVER = Environment.GetEnvironmentVariable("TCP_SERVER") ?? "localhost";

        private static readonly Random random = new Random();

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                await Wcf.Client.CallCalculator();
                return;
            }

            int minimumWorkerThreadCount, minimumIOCThreadCount;
            int maximumWorkerThreadCount, maximumIOCThreadCount;
            int availableWorkerThreadCount, availableIOCThreadCount;
            int logicalProcessorCount = Environment.ProcessorCount;
            ThreadPool.GetMinThreads(out minimumWorkerThreadCount, out minimumIOCThreadCount);
            ThreadPool.GetMaxThreads(out maximumWorkerThreadCount, out maximumIOCThreadCount);
            ThreadPool.GetAvailableThreads(out availableWorkerThreadCount, out availableIOCThreadCount);

            Console.WriteLine("              No. of processors: {0}", logicalProcessorCount);
            Console.WriteLine("  Minimum no. of Worker threads: {0}, IOCP threads: {1}", minimumWorkerThreadCount, minimumIOCThreadCount);
            Console.WriteLine("  Maximum no. of Worker threads: {0}, IOCP threads: {1}", maximumWorkerThreadCount, maximumIOCThreadCount);
            Console.WriteLine("Available no. of Worker threads: {0}, IOCP threads: {1}", availableWorkerThreadCount, availableIOCThreadCount);

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args,
                ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator));
        }
    }
}
