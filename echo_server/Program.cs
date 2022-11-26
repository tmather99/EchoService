using System;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace NetCoreServer
{
    class Program
    {
        public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";

        static void Main(string[] args)
        {
            try
            {
                IWebHost host = CreateWebHostBuilder(args).Build();
                Log.Information($"SEQ_SERVER_URL={SEQ_SERVER_URL}");
                host.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        // Listen on 8088 for http, and 8443 for https, 8089 for NetTcp.
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options =>
            {
                options.ListenAnyIP(Startup.HTTP_PORT);
            })
            .UseNetTcp(Startup.NETTCP_PORT)
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
