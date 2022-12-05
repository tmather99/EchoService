using GloboTicket.Catalog.Model;

namespace GloboTicket.Catalog.Repositories;

public interface IEventRepository
{
  Task<IEnumerable<Event>> GetEvents();
  Task<Event> GetEventById(Guid eventId);
  Event UpdateSpecialOffer();
}
