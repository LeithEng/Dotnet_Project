using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Data;
using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        protected ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }
        // Method to get an Event by its Id
        public async Task<Event> GetByIdAsync(string eventId)
        {
            return await FindAsync(e => e.Id == eventId);
        }

        // Method to get all events, optionally including related UserEvents and Users
        public async Task<IEnumerable<Event>> GetAllEventsAsync(bool includesUserEvents = false)
        {
            if (includesUserEvents)
            {
                // If includesUserEvents is true, also include UserEvents and the related User data
                return await FindAllAsync(
                    e => true,  // Get all events
                    new string[] { "UserEvents", "UserEvents.User" } // Include related entities
                );
            }

            // If no includes, just return events
            return await FindAllAsync(e => true);
        }

        // Method to get a list of events by a userId (if UserEvents relationship is needed)
        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(string userId)
        {
            return await _context.UserEvents
                .Where(ue => ue.UserId == userId)
                .Select(ue => ue.Event)
                .ToListAsync();
        }
    }
}

