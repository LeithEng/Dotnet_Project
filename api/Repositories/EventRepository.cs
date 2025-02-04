using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Data;
using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.DTOs;
using AutoMapper;


namespace Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly IMapper _mapper;  // Add this line

        // Inject IMapper into the constructor
        public EventRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;  // Set the injected mapper
        }




        public async Task<EventDto> GetByIdAsync(string eventId)
        {
            var eventEntity = await _context.Events
                                            .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventEntity == null)
            {
                return null; // Handle case when event is not found
            }

            // Map to EventDto
            var eventDto = _mapper.Map<EventDto>(eventEntity);

            return eventDto;
        }


        //public async Task<EventDto> GetByIdAsync(string eventId)
        //{
        //    var eventEntity = await _context.Events
        //                                     .Include(e => e.UserEvents)  // Include UserEvents
        //                                         .ThenInclude(ue => ue.User)  // Include the related User (if necessary)
        //                                     .FirstOrDefaultAsync(e => e.Id == eventId);

        //    if (eventEntity == null)
        //    {
        //        return null; // Handle case when event is not found
        //    }

        //    // Map to EventDto
        //    var eventDto = _mapper.Map<EventDto>(eventEntity);

        //    return eventDto;
        //}



        //public async Task<IEnumerable<Event>> GetAllEventsAsync(bool includesUserEvents = false)
        //{
        //    IQueryable<Event> query = _context.Events;

        //    if (includesUserEvents)
        //    {
        //        query = query.Include(e => e.UserEvents)  // Include the UserEvent relation
        //                     .ThenInclude(ue => ue.User);  // Include User details within the UserEvent relation
        //    }

        //    return await query.ToListAsync();
        //}

    }
}

