using System;
using CoreWCF.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            Console.WriteLine("Starting up ------");

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options =>
            { })
            .UseNetTcp(8089)
            .UseStartup<Startup>();
    }
}