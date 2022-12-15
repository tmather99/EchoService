using Dapr.Client;
using dapr_server;
using GloboTicket.Catalog.Model;
using GloboTicket.Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Serilog;

namespace GloboTicket.Catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly DaprClient daprClient;
    private readonly IEventRepository eventRepository;
    private readonly ILogger<EventController> logger;

    public EventController(DaprClient daprClient, IEventRepository eventRepository, ILogger<EventController> logger)
    {
        this.daprClient = daprClient;
        this.eventRepository = eventRepository;
        this.logger = logger;
    }

    [HttpGet("version", Name = "GetVerion")]
    public async Task<string> GetVerionAsync()
    {
        try
        {
            var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}/api/version");
            var request = new RestRequest();
            request.AddHeader("dapr-app-id", "rest-server");
            var rest_server_version = await restClient.GetAsync(request);
            return $"dapr_server:v1 => {rest_server_version.Content.Trim('"')}";
        }
        catch (Exception e)
        {
            return "dapr_server:v1 -- " + e.Message;
        }
    }

    [HttpGet(Name = "GetEvents")]
    public async Task<IEnumerable<Event>> GetAll()
    {
        this.logger.LogInformation($"Received get request.");

        /*
        var dataContract = this.CreateExampleContract();
        this.logger.LogInformation(this.JsonSerialize<ExampleContract>(dataContract));
        var response = await this.daprClient.InvokeMethodAsync<ExampleContract, ExampleContract>(appId: "rest-server", methodName: "/api/body", data: dataContract);
        this.logger.LogInformation(this.JsonSerialize(response));
        */

        var restClient = new RestClient($"http://localhost:{Program.DAPR_HTTP_PORT}/api/body");
        var request = new RestRequest();
        request.AddHeader("dapr-app-id", "rest-server");
        var data = this.JsonSerialize<ExampleContract>(this.CreateExampleContract());
        Log.Information(data);
        request.AddJsonBody(data);
        var response = await restClient.PostAsync(request);
        this.logger.LogInformation(this.JsonSerialize(response.Content));

        return await this.eventRepository.GetEvents();
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<Event> GetById(Guid id)
    {
        this.logger.LogInformation($"Received request {id}");

        var data = $"Testing_Path_Endpoint_{id}";
        var request = this.daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appId: "rest-server", methodName: "/api/path/" + data);
        var respone = await daprClient.InvokeMethodWithResponseAsync(request);
        var content = await respone.Content.ReadAsStringAsync();
        this.logger.LogInformation(content);

        return await this.eventRepository.GetEventById(id);
    }


    private ExampleContract CreateExampleContract()
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

    private string JsonSerialize<T>(T thing)
    {
        var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
        var sw = new StringWriter();
        var writer = new Newtonsoft.Json.JsonTextWriter(sw);
        writer.Formatting = Newtonsoft.Json.Formatting.Indented;
        jsonSerializer.Serialize(writer, thing);
        return sw.ToString();
    }
}
