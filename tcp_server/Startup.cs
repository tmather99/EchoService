using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace TCP_Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices()
                    .AddServiceModelMetadata()
                    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.Security.Mode = SecurityMode.None;

            app.UseServiceModel(builder =>
            {
                builder
                    .AddService<CalculateService>()
                    .AddServiceEndpoint<CalculateService>(typeof(Contract.ICalculate), binding, "nettcp")
                    .AddServiceEndpoint<CalculateService>(typeof(Contract.ICalculate2), binding, "nettcp2");
            });

            app.UseServiceModel();
        }
    }
}