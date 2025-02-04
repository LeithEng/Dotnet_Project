using api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IEventRepository
    {
        // Get event by Id
        Task<Event> GetByIdAsync(string eventId);



    }
}

