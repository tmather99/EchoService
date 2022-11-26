using System;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace TCP_Server
{
    class Program
    {
        public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";

        static void Main(string[] args)
        {
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                Log.Information($"SEQ_SERVER_URL={SEQ_SERVER_URL}");
                host.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseNetTcp(8089)
            .UseStartup<Startup>()
            .UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration)
                  .Enrich.WithEnvironmentName()
                  .Enrich.WithMachineName()
                  .Enrich.WithProcessName()
                  .Enrich.WithProcessId()
                  .Enrich.WithThreadId()
                  .Enrich.WithMemoryUsage()
                  .WriteTo.Console()
                  .WriteTo.Seq(SEQ_SERVER_URL),
                preserveStaticLogger: false,
                writeToProviders: false);
    }
}