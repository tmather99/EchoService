using GloboTicket.Catalog.Repositories;
using Serilog;

namespace dapr_server;

public class Program
{
    public static int DAPR_HTTP_PORT = int.Parse(Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3501");
    public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";

    private static void Main(string[] args)
    {
        Log.Information($"SEQ_SERVER_URL={SEQ_SERVER_URL}");

        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithProcessName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithMemoryUsage()
            .WriteTo.Console()
            .WriteTo.Seq(SEQ_SERVER_URL)
            .CreateLogger();

        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            options.AllowSynchronousIO = true;
        });

        // Add services to the container.
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IEventRepository, EventRepository>();
        builder.Services.AddControllers().AddDapr();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCloudEvents();
        app.MapSubscribeHandler();

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}