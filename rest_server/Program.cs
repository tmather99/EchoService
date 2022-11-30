using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WebHttpServer
{
    public class Program
    {
        public static int API_PORT = int.Parse(Environment.GetEnvironmentVariable("API_PORT") ?? "8080");
        public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";

        static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq(Program.SEQ_SERVER_URL)
                    .CreateLogger();

                Log.Information($"SEQ_SERVER_URL={Program.SEQ_SERVER_URL}");

                IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args)
                    .UseKestrel(options =>
                    {
                        options.AllowSynchronousIO = true;
                        options.ListenAnyIP(Program.API_PORT);
                    })
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
                                .WriteTo.Seq(Program.SEQ_SERVER_URL),
                        preserveStaticLogger: false,
                        writeToProviders: false);
                
                IWebHost app = builder.Build();
                app.Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
