using System;
using System.Security.Cryptography.X509Certificates;
using Contract;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreServer
{
    public class Startup
    {
        public const int HTTP_PORT = 8088;
        public const int HTTPS_PORT = 8443;
        public const int NETTCP_PORT = 8090;
        // Only used on case that UseRequestHeadersForMetadataAddressBehavior is not used
        public const string HOST_IN_WSDL = "localhost";

        public void ConfigureServices(IServiceCollection services)
        {
            // Enable CoreWCF Services, enable metadata
            // Use the Url used to fetch WSDL as that service endpoint address in generated WSDL 
            services.AddServiceModelServices()
                    .AddServiceModelMetadata()
                    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
        }

        public void Configure(IApplicationBuilder app)
        {
            //app.UseSerilogRequestLogging();

            var netTcpBinding = new NetTcpBinding();
            netTcpBinding.Security.Mode = SecurityMode.None;
            netTcpBinding.TransferMode = TransferMode.Streamed;

            var netTcpBinding1 = new NetTcpBinding();
            netTcpBinding1.Security.Mode = SecurityMode.Transport;
            netTcpBinding1.TransferMode = TransferMode.Streamed;
            netTcpBinding1.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            var netTcpBinding2 = new NetTcpBinding();
            netTcpBinding2.Security.Mode = SecurityMode.TransportWithMessageCredential;
            netTcpBinding2.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

            app.UseServiceModel(builder =>
            {
                // Add the Echo Service
                builder.AddService<EchoService>(serviceOptions =>
                {
                    // Set the default host name:port in generated WSDL and the base path for the address 
                    serviceOptions.BaseAddresses.Add(new Uri($"http://{HOST_IN_WSDL}/EchoService"));
                    //serviceOptions.BaseAddresses.Add(new Uri($"https://{HOST_IN_WSDL}/EchoService"));
                })

                // Add a BasicHttpBinding endpoint
                .AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(), "/basichttp")
                //.AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/basichttp")

                // Add WSHttpBinding endpoints
                .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.None), "/wsHttp")
                //.AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.Transport), "/wsHttp")

                // Add NetTcpBinding
                .AddServiceEndpoint<EchoService, IEchoService>(netTcpBinding,   $"net.tcp://localhost:{NETTCP_PORT}/netTcp")
                .AddServiceEndpoint<EchoService, IEchoService1>(netTcpBinding1, $"net.tcp://localhost:{NETTCP_PORT}/netTcp1")
                .AddServiceEndpoint<EchoService, IEchoService2>(netTcpBinding2, $"net.tcp://localhost:{NETTCP_PORT}/netTcp2")

                // Add server certificate
                .ConfigureServiceHostBase<EchoService>(h => ChangeHostBehavior(h));

                // Configure WSDL to be available over http & https
                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
            });
        }

        private static void ChangeHostBehavior(ServiceHostBase host)
        {
            var srvCredentials = new ServiceCredentials();
            srvCredentials.ClientCertificate.Authentication.CertificateValidationMode = CoreWCF.Security.X509CertificateValidationMode.None;
            srvCredentials.ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, "c6779716aea1546aef89ef03a720fb6a1330629f");
            host.Description.Behaviors.Add(srvCredentials);
        }
    }
}
