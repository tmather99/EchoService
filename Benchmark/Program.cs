namespace Bechmark
{
    using System;
    using System.Threading;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;

    /// <summary>
    /// Singleton instance Command Processor.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">command args.</param>
        public static void Main(string[] args)
        {
            int minimumWorkerThreadCount, minimumIOCThreadCount;
            int maximumWorkerThreadCount, maximumIOCThreadCount;
            int availableWorkerThreadCount, availableIOCThreadCount;
            int logicalProcessorCount = System.Environment.ProcessorCount;
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
