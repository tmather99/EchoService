using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Serilog;

namespace TCP_client
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq(Wcf.Client.SEQ_SERVER_URL)
                    .CreateLogger();

                Log.Information($"SEQ_SERVER_URL={Wcf.Client.SEQ_SERVER_URL}");

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
