using Microsoft.AspNetCore.Mvc;
using GloboTicket.Catalog.Repositories;
using GloboTicket.Catalog.Model;

namespace GloboTicket.Catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventRepository eventRepository;

    private readonly ILogger<EventController> logger;

    public EventController(IEventRepository eventRepository, ILogger<EventController> logger)
    {
        this.eventRepository = eventRepository;
        this.logger = logger;
    }

    [HttpGet(Name = "GetEvents")]
    public async Task<IEnumerable<Event>> GetAll()
    {
        this.logger.LogInformation($"Received get request.");
        return await this.eventRepository.GetEvents();
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<Event> GetById(Guid id)
    {
        this.logger.LogInformation($"Received request {id}");
        return await this.eventRepository.GetEventById(id);
    }
}
