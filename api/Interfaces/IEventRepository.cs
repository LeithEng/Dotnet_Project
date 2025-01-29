using api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IEventRepository
    {
        // Get event by Id
        Task<Event> GetByIdAsync(string eventId);

        // Get all events, with an option to include related UserEvents and Users
        Task<IEnumerable<Event>> GetAllEventsAsync(bool includesUserEvents = false);

        // Get events by userId (if needed)
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId);
    }
}

