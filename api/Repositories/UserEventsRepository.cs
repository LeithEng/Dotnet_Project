using Microsoft.EntityFrameworkCore;
using api.Consts;
using api.Interfaces;
using api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using api.Models;

namespace Repositories
{
    public class UserEventsRepository : BaseRepository<UserEvent>  ,IUserEventsRepository
    {
        protected ApplicationDbContext _context;

        public UserEventsRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<UserEvent> GetByIdAsync(string userId)
        {
            return FindAsync(b => b.UserId == userId);
        }

        public Task<IEnumerable<UserEvent>> getUsers(string eventId, bool includes = false)
        {
            if (includes)
            {
                return FindAllAsync(b => b.EventId == eventId, new string[] { "User" });
            }
            return FindAllAsync(b => b.EventId == eventId);
        }

        // Sort events by a specified property
        public async Task<IEnumerable<UserEvent>> SortEvents(Expression<Func<UserEvent, object>> orderBy, string orderByDirection = OrderBy.Ascending)
        {
            return await FindAllAsync(b => true, null, null, orderBy, orderByDirection);
        }

        // Get upcoming events (events that have a start date in the future)
        public async Task<IEnumerable<UserEvent>> GetUpcomingEvents()
        {
            return await FindAllAsync(e => e.Event.StartDate > DateTime.UtcNow);
        }


        // Get paginated events
        public async Task<IEnumerable<UserEvent>> GetPagedEvents(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            return await FindAllAsync(b => true, pageSize, skip);
        }

        // Get all events for a specific user
        public async Task<IEnumerable<UserEvent>> GetEventsByUser(string userId)
        {
            return await FindAllAsync(e => e.UserId == userId);
        }
    }
}