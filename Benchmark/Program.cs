namespace Bechmark
{
    using System.Threading;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// Singleton instance Command Processor.
    /// </summary>
    public class Program
    {
        public static int DAPR_HTTP_PORT = int.Parse(Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");
        public static int API_PORT = int.Parse(Environment.GetEnvironmentVariable("API_PORT") ?? "8083");
        public static string API_PROTOCOL = Environment.GetEnvironmentVariable("API_PROTOCOL") ?? "http";
        public static string API_SERVER = Environment.GetEnvironmentVariable("API_SERVER") ?? "localhost";
        public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">command args.</param>
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq(Wcf.Client.SEQ_SERVER_URL)
                    .CreateLogger();

                int minimumWorkerThreadCount, minimumIOCThreadCount;
                int maximumWorkerThreadCount, maximumIOCThreadCount;
                int availableWorkerThreadCount, availableIOCThreadCount;
                int logicalProcessorCount = System.Environment.ProcessorCount;
                ThreadPool.GetMinThreads(out minimumWorkerThreadCount, out minimumIOCThreadCount);
                ThreadPool.GetMaxThreads(out maximumWorkerThreadCount, out maximumIOCThreadCount);
                ThreadPool.GetAvailableThreads(out availableWorkerThreadCount, out availableIOCThreadCount);

                Log.Information("              No. of processors: {0}", logicalProcessorCount);
                Log.Information("  Minimum no. of Worker threads: {0}, IOCP threads: {1}", minimumWorkerThreadCount, minimumIOCThreadCount);
                Log.Information("  Maximum no. of Worker threads: {0}, IOCP threads: {1}", maximumWorkerThreadCount, maximumIOCThreadCount);
                Log.Information("Available no. of Worker threads: {0}, IOCP threads: {1}", availableWorkerThreadCount, availableIOCThreadCount);

                BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args,
                    ManualConfig
                        .Create(DefaultConfig.Instance)
                        .WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile)
                        .WithOptions(ConfigOptions.DisableOptimizationsValidator));
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
