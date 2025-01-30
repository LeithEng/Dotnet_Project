using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;
using api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.UnitOfWork;
using api.Consts;
using System.Linq.Expressions;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/Event
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = errors
                });
            }

            var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == eventDto.HobbyId);
            if (hobby == null)
            {
                return BadRequest(new { message = "Invalid Hobby ID provided." });
            }

            var creatorUser = await _unitOfWork.UserManager.FindByIdAsync(eventDto.CreatedByUserId);
            if (creatorUser == null)
            {
                return BadRequest(new { message = "Creator user does not exist." });
            }

            // Create the new event
            var newEvent = new Event
            {
                Id = Guid.NewGuid().ToString(),
                Name = eventDto.Name,
                Description = eventDto.Description,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HobbyId = eventDto.HobbyId,
                Hobby = hobby,
                // Set the CreatedByUserId to associate the event with the creator user
                UserId = eventDto.CreatedByUserId 
            };

            // Add the event to the repository
            await _unitOfWork.events.AddAsync(newEvent);

            // Commit the event save before proceeding with UserEvent
            _unitOfWork.Complete(); 

            var userEvent = new UserEvent
            {
                UserId = eventDto.CreatedByUserId,
                EventId = newEvent.Id,
                Event = newEvent,
                Rate = null
            };

            // Add the user event (creator) to the repository
            await _unitOfWork.userEvents.AddAsync(userEvent);

            if (string.IsNullOrEmpty(eventDto.CreatedByUserId))
            {
                return BadRequest(new { message = "Creator user ID cannot be null or empty." });
            }


            _unitOfWork.Complete();

            // Return the created event
            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
        }


        // GET: api/Event/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(string id)
        {
            var eventItem = await _unitOfWork.events.GetByIdAsync(id);

            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            return Ok(eventItem);
        }

        // GET: api/Event
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _unitOfWork.events.GetAllAsync();
            return Ok(events);
        }

        // PUT: api/Event/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(string id, [FromBody] EventDto eventDto)
        {
            // Fetch the event to be updated
            var eventToUpdate = await _unitOfWork.events.GetByIdAsync(id);
            if (eventToUpdate == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            if (!string.IsNullOrEmpty(eventDto.HobbyId))
            {
                var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == eventDto.HobbyId);
                if (hobby == null)
                {
                    return BadRequest(new { message = "Invalid Hobby ID provided." });
                }
                // Set the HobbyId and Hobby navigation property to the new hobby
                eventToUpdate.HobbyId = eventDto.HobbyId;
                eventToUpdate.Hobby = hobby;
            }

            // Update other properties
            eventToUpdate.Name = eventDto.Name;
            eventToUpdate.Description = eventDto.Description;
            eventToUpdate.StartDate = eventDto.StartDate;
            eventToUpdate.EndDate = eventDto.EndDate;
            eventToUpdate.UpdatedAt = DateTime.UtcNow;

            // Save the changes
            _unitOfWork.events.Update(eventToUpdate);
            _unitOfWork.Complete();

            return Ok(new { message = "Event updated successfully." });
        }

        // DELETE: api/Event/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var eventToDelete = await _unitOfWork.events.GetByIdAsync(id);
            if (eventToDelete == null)
            {
                return NotFound(new { message = "Event not found." });
            }

             _unitOfWork.events.Delete(eventToDelete);
             _unitOfWork.Complete();

            return Ok(new { message = "Event deleted successfully." });
        }

        // Search events by Hobby ID
        [HttpGet("searchByHobby")]
        public async Task<IActionResult> SearchEventsByHobby([FromQuery] string hobbyId)
        {
            if (string.IsNullOrEmpty(hobbyId))
            {
                return BadRequest(new { message = "Hobby ID is required." });
            }

            var events = await _unitOfWork.events.FindAllAsync(e => e.HobbyId == hobbyId);

            if (!events.Any())
            {
                return NotFound(new { message = "No events found for the specified hobby." });
            }

            return Ok(events);
        }


        // Search events by Name (case-insensitive search)
        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest(new { message = "Search term is required." });
            }

            var events = await _unitOfWork.events.FindAllAsync(e => e.Name.Contains(searchTerm));

            if (!events.Any())
            {
                return NotFound(new { message = "No events found matching the search criteria." });
            }

            return Ok(events);
        }

        // Search events by Created User ID
        [HttpGet("searchByUser")]
        public async Task<IActionResult> SearchEventsByUser([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User ID is required." });
            }

            var events = await _unitOfWork.events.FindAllAsync(e => e.UserId == userId);

            if (!events.Any())
            {
                return NotFound(new { message = "No events found for the specified user." });
            }

            return Ok(events);
        }

        // Get upcoming events (events starting from today)
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var today = DateTime.UtcNow;
            var events = await _unitOfWork.events.FindAllAsync(e => e.StartDate >= today);

            if (!events.Any())
            {
                return NotFound(new { message = "No upcoming events found." });
            }

            return Ok(events.OrderBy(e => e.StartDate));
        }

        // Get events sorted by Start Date (ascending/descending)
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedEvents([FromQuery] string sortOrder = "asc")
        {
            var events = await _unitOfWork.events.GetAllAsync();

            if (!events.Any())
            {
                return NotFound(new { message = "No events found." });
            }

            events = sortOrder.ToLower() == "desc"
                ? events.OrderByDescending(e => e.StartDate).ToList()
                : events.OrderBy(e => e.StartDate).ToList();

            return Ok(events);
        }

        // Get events within a date range
        [HttpGet("dateRange")]
        public async Task<IActionResult> GetEventsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new { message = "Invalid date range. Start date should be before end date." });
            }

            var events = await _unitOfWork.events.FindAllAsync(e => e.StartDate >= startDate && e.StartDate <= endDate);

            if (!events.Any())
            {
                return NotFound(new { message = "No events found within the given date range." });
            }

            return Ok(events.OrderBy(e => e.StartDate));
        }

        // Paginated retrieval of events
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { message = "Page and pageSize must be greater than zero." });
            }

            var allEvents = await _unitOfWork.events.GetAllAsync();
            var pagedEvents = allEvents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                currentPage = page,
                totalPages = (int)Math.Ceiling((double)allEvents.Count() / pageSize),
                events = pagedEvents
            });
        }

    }
}



