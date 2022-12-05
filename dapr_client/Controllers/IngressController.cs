using Dapr.Client;
using GloboTicket.Catalog;
using GloboTicket.Frontend.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebHttpClient;

namespace dapr_client.Controllers;

[ApiController]
[Route("ingress")]
public class IngressController : ControllerBase
{
    private readonly DaprClient daprClient;

    private readonly ILogger<IngressController> logger;

    public IngressController(DaprClient daprClient, ILogger<IngressController> logger)
    {
        this.daprClient = daprClient;
        this.logger = logger;
    }

    [HttpGet("event", Name = "GetEvents")]
    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        var request = this.daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appId: "catalog", methodName: "event");
        var respone = await daprClient.InvokeMethodWithResponseAsync(request);
        var content = await respone.Content.ReadAsStringAsync();
        var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(content);
        this.logger.LogInformation(this.JsonSerialize(events));
        return events;
    }

    [HttpGet("event/{id}", Name = "GetEventById")]
    public async Task<Event> GetEventByIdAsync(Guid id)
    {
        var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Get, appId: "catalog", methodName: $"event/{id}");
        var respone = await daprClient.InvokeMethodWithResponseAsync(request);
        var content = await respone.Content.ReadAsStringAsync();
        var @event = JsonConvert.DeserializeObject<Event>(content);
        this.logger.LogInformation(this.JsonSerialize(@event));
        return @event;
    }

    [HttpGet("statestore", Name = "GetStateStore")]
    public async Task<ExampleContract> GetStateStoreAsync()
    {
        var data = this.CreateExampleContract();
        await daprClient.SaveStateAsync<ExampleContract>(storeName: "shopstate", key: "example", value: data);
        var result = await this.daprClient.GetStateAsync<ExampleContract>(storeName: "shopstate", key: "example");
        this.logger.LogInformation(this.JsonSerialize(result));
        return result;
    }

    [HttpGet("secrets", Name = "GetSecrets")]
    public async Task<Dictionary<string, Dictionary<string, string>>> GetSecretsAsync()
    {
        var secretStoreName = Environment.GetEnvironmentVariable("SECRET_STORE_NAME") ?? "secretstore";
        var secrets = await this.daprClient.GetBulkSecretAsync(storeName: secretStoreName);
        secrets.Keys.Order().ToList().ForEach(key =>
        {
            var secret = secrets[key].First();
            this.logger.LogInformation($"{secret.Key} => {secret.Value}");
        });

        return secrets;
    }

    [HttpGet("publish", Name = "PublishOrder")]
    public async Task<Guid> DaprPublishOrder()
    {
        var guid = Guid.NewGuid();
        var order = new OrderForCreation()
        {
            Date = DateTimeOffset.Now,
            OrderId = guid,
            Lines = new List<OrderLine>()
            {
                new OrderLine()
                {
                    EventId = guid,
                    EventName = "Event.Name",
                    ArtistName = "Event.Artist",
                    Price = 1,
                    TicketCount = 1
                }
            },
            CustomerDetails = new CustomerDetails()
            {
                Address = "Address",
                CreditCardNumber = "CreditCard",
                Email = "user@email.com",
                Name = $"Name_{guid}",
                PostalCode = "PostalCode",
                Town = "Town",
                CreditCardExpiryDate = "CreditCardDate"
            }
        };

        await this.daprClient.PublishEventAsync<OrderForCreation>(pubsubName: "pubsub", topicName: "orders", data: order);
        this.logger.LogInformation($"Publihsed orderId={order.OrderId}");
        return order.OrderId;
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
