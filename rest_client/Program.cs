using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Serilog;

namespace WebHttpClient
{
    public class Program
    {
        public static int API_PORT = int.Parse(Environment.GetEnvironmentVariable("API_PORT") ?? "8080");
        public static string API_SERVER = Environment.GetEnvironmentVariable("API_SERVER") ?? "localhost";
        public static string SEQ_SERVER_URL = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "http://localhost:5341";

        static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq(Program.SEQ_SERVER_URL)
                    .CreateLogger();

                Log.Information($"SEQ_SERVER_URL={Program.SEQ_SERVER_URL}");

                if (args.Length == 0)
                {
                    // Use a single HttpClient instance for multiple requests as it will pool connections 
                    var httpClient = new HttpClient();

                    // Using a wrapper generated based on the service OpenAPI definition
                    var client = new WebAPIGeneratedWrapper($"http://{Program.API_SERVER}:{Program.API_PORT}/", httpClient);

                    // Calls /api/path/{param}
                    string result = await client.PathAsync("Testing_the_path_endpoint");
                    Log.Information(result);

                    // Calls /api/query?param=value
                    result = await client.QueryAsync("Testing the query endpoint");
                    Log.Information(result);

                    // Calls /api/body with a complex data structure
                    var data = CreateExampleContract();
                    var result2 = await client.BodyAsync(data);
                    Log.Information(JsonSerialize(result2));
                }
                else
                {
                    BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args,
                        ManualConfig
                            .Create(DefaultConfig.Instance)
                            .WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile)
                            .WithOptions(ConfigOptions.DisableOptimizationsValidator));
                }

            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static ExampleContract CreateExampleContract()
        {
            return new ExampleContract()
            {
                SimpleProperty = "House Stark",
                ComplexProperty = new() { Name = "Jon Snow" },
                SimpleCollection = "Winter is coming".Split(" "),
                ComplexCollection = new ExampleContractArrayInnerContract[]
                {
                    new() { Name = Guid.NewGuid().ToString() },
                    new() { Name = "Arya Stark" },
                    new() { Name = "Sansa Stark" }
                }
            };
        }

        public static string JsonSerialize<T>(T thing)
        {
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            var sw = new StringWriter();
            var writer = new Newtonsoft.Json.JsonTextWriter(sw);
            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            jsonSerializer.Serialize(writer, thing);
            return sw.ToString();
        }
    }
}
