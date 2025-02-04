using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;
using api.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Security.Claims;


namespace api.Controllers
{

    [Authorize]  // Restrict access to authenticated users
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(string id, [FromBody] EventDto eventDto)
        {
            // Retrieve the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the event to be updated
            var eventToUpdate = await _unitOfWork.events.GetByIdAsync(id);
            if (eventToUpdate == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Check if the current user is the creator of the event
            if (eventToUpdate.UserId != currentUserId)
            {
                return Unauthorized(new { message = "You are not authorized to update this event." });
            }

            // Check and set the HobbyId if provided
            if (!string.IsNullOrEmpty(eventDto.HobbyId))
            {
                var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == eventDto.HobbyId);
                if (hobby == null)
                {
                    return BadRequest(new { message = "Invalid Hobby ID provided." });
                }
                eventToUpdate.HobbyId = eventDto.HobbyId;
                eventToUpdate.Hobby = hobby;
            }

            // Map the updated data to the event
            _mapper.Map(eventDto, eventToUpdate);
            eventToUpdate.UpdatedAt = DateTime.UtcNow;

            // Save the changes
            _unitOfWork.events.Update(eventToUpdate);
            _unitOfWork.Complete();

            return Ok(new { message = "Event updated successfully." });
        }

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

            // Get the currently authenticated user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Creator user ID is missing or invalid." });
            }

            var creatorUser = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (creatorUser == null)
            {
                return BadRequest(new { message = "Creator user does not exist." });
            }

            // Create the new event
            var newEvent = _mapper.Map<Event>(eventDto);
            newEvent.Id = Guid.NewGuid().ToString();
            newEvent.UserId = userId; // Set UserId to the currently authenticated user

            // Add the event to the repository
            await _unitOfWork.events.AddAsync(newEvent);

            // Commit the event save and user event save
            _unitOfWork.Complete();

            // Return the created event
            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);

        }

    // POST: api/Event/Participate/{eventId}
    [HttpPost("Participate/{eventId}")]
        public async Task<IActionResult> Participate(string eventId)
        {
            // Get the currently authenticated user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User is not authenticated." });
            }

            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new { message = "User does not exist." });
            }

            // Find the event by its ID
            var eventEntity = await _unitOfWork.events.FindAsync(e => e.Id == eventId);
            if (eventEntity == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Check if the user is already participating in the event
            var existingUserEvent = await _unitOfWork.userEvents
                .FindAsync(ue => ue.UserId == userId && ue.EventId == eventId);

            if (existingUserEvent != null)
            {
                return BadRequest(new { message = "User is already a participant in this event." });
            }

            // Create the UserEvent for the participant
            var userEvent = new UserEvent
            {
                UserId = userId,
                EventId = eventId,
                Event = eventEntity,
                Rate = null // Optionally, set the initial rating if applicable
            };

            // Add the user event to the repository
            await _unitOfWork.userEvents.AddAsync(userEvent);

            // Commit the user event save
            _unitOfWork.Complete();

            // Return success message
            return Ok(new { message = "User successfully added to the event." });
        }

        // GET: api/Event
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _unitOfWork.events.GetAllAsync();
            return Ok(events);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            // Retrieve the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the event from the database
            var eventToDelete = await _unitOfWork.events.GetByIdAsync(id);
            if (eventToDelete == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Check if the current user is the creator of the event
            if (eventToDelete.UserId != currentUserId)
            {
                return Unauthorized(new { message = "You are not authorized to delete this event." });
            }

            // Perform soft delete
            _unitOfWork.events.SoftDelete(eventToDelete);
            _unitOfWork.Complete();

            return Ok(new { message = "Event deleted successfully." });
        }


        [HttpGet("searchByHobby")]
        public async Task<IActionResult> SearchEventsByHobby([FromQuery] string hobbyId)
        {
            if (string.IsNullOrEmpty(hobbyId))
            {
                return BadRequest(new { message = "Hobby ID is required." });
            }

            var events = await _unitOfWork.events.FindAllAsync(e => e.HobbyId == hobbyId);

            if (events == null || !events.Any())
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

       // POST: api/Event/RateEvent
        [HttpPost("rate/{eventId}")]
        public async Task<IActionResult> RateEvent(string eventId, [FromBody] RateDto rateDto)
        {
            // Ensure the user is authenticated and get the current userId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming the user ID is stored in the claims

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            // Check if the event exists
            var existingEvent = await _unitOfWork.events.FindAsync(e => e.Id == eventId);
            if (existingEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Check if the user is already a participant in the event
            var existingUserEvent = await _unitOfWork.userEvents
                    .FindAsync(ue => ue.UserId == userId && ue.EventId == eventId);

            if (existingUserEvent == null)
            {
                return BadRequest(new { message = "User is not a participant in this event." });
            }


            // Rating validation - ensure the rate is between 1 and 5
            if (rateDto.Rate < 1 || rateDto.Rate > 5)
            {
                return BadRequest(new { message = "Rating must be between 1 and 5." });
            }

            // Update the rating
            existingUserEvent.Rate = rateDto.Rate;

            // Save the changes
            _unitOfWork.Complete();

            return Ok(new { message = "Rating submitted successfully." });
}







}
}





//using Microsoft.AspNetCore.Mvc;
//using api.Models;
//using api.Interfaces;
//using api.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using api.UnitOfWork;
//using api.Consts;
//using System.Linq.Expressions;
//using Microsoft.AspNetCore.Authorization;
//using AutoMapper;
//using System.Security.Claims;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;


//namespace api.Controllers
//{

//    [Authorize]  
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EventController : ControllerBase
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public EventController(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }


//        [HttpPost]
//        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values
//                    .SelectMany(v => v.Errors)
//                    .Select(e => e.ErrorMessage)
//                    .ToList();

//                return BadRequest(new
//                {
//                    message = "Validation failed.",
//                    errors = errors
//                });
//            }

//            var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == eventDto.HobbyId);
//            if (hobby == null)
//            {
//                return BadRequest(new { message = "Invalid Hobby ID provided." });
//            }

//            // Get the currently authenticated user's ID
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

//            if (string.IsNullOrEmpty(userId))
//            {
//                return BadRequest(new { message = "Creator user ID is missing or invalid." });
//            }

//            var creatorUser = await _unitOfWork.UserManager.FindByIdAsync(userId);
//            if (creatorUser == null)
//            {
//                return BadRequest(new { message = "Creator user does not exist." });
//            }

//            // Create the new event
//            var newEvent = _mapper.Map<Event>(eventDto);
//            newEvent.Id = Guid.NewGuid().ToString();
//            newEvent.UserId = userId; // Set UserId to the currently authenticated user

//            // Add the event to the repository
//            await _unitOfWork.events.AddAsync(newEvent);

//            // Commit the event save and user event save
//            _unitOfWork.Complete();

//            // Return the created event
//            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);

//        }

//    }


//        [HttpPost("Participate/{eventId}")]
//        public async Task<IActionResult> Participate(string eventId)
//        {
//            // Get the currently authenticated user's ID
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

//            if (string.IsNullOrEmpty(userId))
//            {
//                return BadRequest(new { message = "User is not authenticated." });
//            }

//            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
//            if (user == null)
//            {
//                return BadRequest(new { message = "User does not exist." });
//            }

//            // Find the event by its ID
//            var eventEntity = await _unitOfWork.events.FindAsync(e => e.Id == eventId);
//            if (eventEntity == null)
//            {
//                return NotFound(new { message = "Event not found." });
//            }

//            // Check if the user is already participating in the event
//            var existingUserEvent = await _unitOfWork.userEvents
//                .FindAsync(ue => ue.UserId == userId && ue.EventId == eventId);

//            if (existingUserEvent != null)
//            {
//                return BadRequest(new { message = "User is already a participant in this event." });
//            }

//            // Create the UserEvent for the participant
//            var userEvent = new UserEvent
//            {
//                UserId = userId,
//                EventId = eventId,
//                Event = eventEntity,
//                Rate = null // Optionally, set the initial rating if applicable
//            };

//            // Add the user event to the repository
//            await _unitOfWork.userEvents.AddAsync(userEvent);

//            // Commit the user event save
//            _unitOfWork.Complete();

//            // Return success message
//            return Ok(new { message = "User successfully added to the event." });
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateEvent(string id, [FromBody] EventDto eventDto)
//        {
//            // Retrieve the current user ID from the JWT token
//            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            // Fetch the event to be updated
//            var eventToUpdate = await _unitOfWork.events.GetByIdAsync(id);
//            if (eventToUpdate == null)
//            {
//                return NotFound(new { message = "Event not found." });
//            }

//            // Check if the current user is the creator of the event
//            if (eventToUpdate.UserId != currentUserId)
//            {
//                return Unauthorized(new { message = "You are not authorized to update this event." });
//            }

//            // Check and set the HobbyId if provided
//            if (!string.IsNullOrEmpty(eventDto.HobbyId))
//            {
//                var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == eventDto.HobbyId);
//                if (hobby == null)
//                {
//                    return BadRequest(new { message = "Invalid Hobby ID provided." });
//                }
//                eventToUpdate.HobbyId = eventDto.HobbyId;
//                eventToUpdate.Hobby = hobby;
//            }

//            // Map the updated data to the event
//            _mapper.Map(eventDto, eventToUpdate);
//            eventToUpdate.UpdatedAt = DateTime.UtcNow;

//            // Save the changes
//            _unitOfWork.events.Update(eventToUpdate);
//            _unitOfWork.Complete();

//            return Ok(new { message = "Event updated successfully." });
//        }


//        // POST: api/Event/RateEvent
//        [HttpPost("rate/{eventId}")]
//        public async Task<IActionResult> RateEvent(string eventId, [FromBody] RateDto rateDto)
//        {
//            // Ensure the user is authenticated and get the current userId
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming the user ID is stored in the claims

//            if (string.IsNullOrEmpty(userId))
//            {
//                return Unauthorized(new { message = "User is not authenticated." });
//            }

//            // Check if the event exists
//            var existingEvent = await _unitOfWork.events.FindAsync(e => e.Id == eventId);
//            if (existingEvent == null)
//            {
//                return NotFound(new { message = "Event not found." });
//            }

//            // Check if the user is already a participant in the event
//            var existingUserEvent = await _unitOfWork.userEvents
//                  .FindAsync(ue => ue.UserId == userId && ue.EventId == eventId);

//            if (existingUserEvent == null)
//            {
//                return BadRequest(new { message = "User is not a participant in this event." });
//            }


//            // Rating validation - ensure the rate is between 1 and 5
//            if (rateDto.Rate < 1 || rateDto.Rate > 5)
//            {
//                return BadRequest(new { message = "Rating must be between 1 and 5." });
//            }

//            // Update the rating
//            existingUserEvent.Rate = rateDto.Rate;

//            // Save the changes
//            _unitOfWork.Complete();

//            return Ok(new { message = "Rating submitted successfully." });
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEvent(string id)
//        {
//            // Retrieve the current user ID from the JWT token
//            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            // Fetch the event from the database
//            var eventToDelete = await _unitOfWork.events.GetByIdAsync(id);
//            if (eventToDelete == null)
//            {
//                return NotFound(new { message = "Event not found." });
//            }

//            // Check if the current user is the creator of the event
//            if (eventToDelete.UserId != currentUserId)
//            {
//                return Unauthorized(new { message = "You are not authorized to delete this event." });
//            }

//            // Perform soft delete
//            _unitOfWork.events.SoftDelete(eventToDelete);
//            _unitOfWork.Complete();

//            return Ok(new { message = "Event deleted successfully." });
//        }


//        //[HttpGet("{id}")]
//        //public async Task<IActionResult> GetEventById(string id)
//        //{
//        //    var eventItem = await ((IEventRepository)_unitOfWork.events).GetEventWithParticipantsAsync(id);

//        //    if (eventItem == null)
//        //    {
//        //        return NotFound(new { message = "Event not found." });
//        //    }

//        //    return Ok(eventItem);
//        //}


//        //[HttpGet]
//        //public async Task<IActionResult> GetAllEvents()
//        //{
//        //    // Eagerly load the UserEvents and the associated User for each UserEvent
//        //    var events = await _unitOfWork.events
//        //        .Include(e => e.UserEvents)   // Eagerly load UserEvents collection
//        //        .ThenInclude(ue => ue.User)  // Eagerly load the User for each UserEvent
//        //        .Take(100)                   // Optionally limit the number of events returned
//        //        .ToListAsync();              // Execute the query and fetch all events

//        //    return Ok(events);
//        //}

//        //[HttpGet]
//        //public async Task<IActionResult> GetAllEvents()
//        //{
//        //    // Fetch all events with their participants eagerly loaded
//        //    var events = await ((IEventRepository)_unitOfWork.events).GetAllEventsWithParticipantsAsync();

//        //    if (events == null || !events.Any())
//        //    {
//        //        return NotFound(new { message = "No events found." });
//        //    }

//        //    return Ok(events);
//        //}


//        [HttpGet("searchByHobby")]
//        public async Task<IActionResult> SearchEventsByHobby([FromQuery] string hobbyId)
//        {
//            if (string.IsNullOrEmpty(hobbyId))
//            {
//                return BadRequest(new { message = "Hobby ID is required." });
//            }

//            var events = await _unitOfWork.events.FindAllAsync(e => e.HobbyId == hobbyId);

//            if (events == null || !events.Any())
//            {
//                return NotFound(new { message = "No events found for the specified hobby." });
//            }
//                return Ok(events);
//        }

//        // Search events by Name (case-insensitive search)
//        [HttpGet("search")]
//        public async Task<IActionResult> SearchEvents([FromQuery] string searchTerm)
//        {
//            if (string.IsNullOrEmpty(searchTerm))
//            {
//                return BadRequest(new { message = "Search term is required." });
//            }

//            var events = await _unitOfWork.events.FindAllAsync(e => e.Name.Contains(searchTerm));

//            if (!events.Any())
//            {
//                return NotFound(new { message = "No events found matching the search criteria." });
//            }

//            return Ok(events);
//        }

//        // Search events by Created User ID
//        [HttpGet("searchByUser")]

//        public async Task<IActionResult> SearchEventsByUser([FromQuery] string userId)
//        {
//            if (string.IsNullOrEmpty(userId))
//            {
//                return BadRequest(new { message = "User ID is required." });
//            }

//            var events = await _unitOfWork.events.FindAllAsync(e => e.UserId == userId);

//            if (!events.Any())
//            {
//                return NotFound(new { message = "No events found for the specified user." });
//            }

//            return Ok(events);
//        }

//        // Get upcoming events (events starting from today)
//        [HttpGet("upcoming")]
//        public async Task<IActionResult> GetUpcomingEvents()
//        {
//            var today = DateTime.UtcNow;
//            var events = await _unitOfWork.events.FindAllAsync(e => e.StartDate >= today);

//            if (!events.Any())
//            {
//                return NotFound(new { message = "No upcoming events found." });
//            }

//            return Ok(events.OrderBy(e => e.StartDate));
//        }

//        // Get events sorted by Start Date (ascending/descending)
//        [HttpGet("sorted")]
//        public async Task<IActionResult> GetSortedEvents([FromQuery] string sortOrder = "asc")
//        {
//            var events = await _unitOfWork.events.GetAllAsync();

//            if (!events.Any())
//            {
//                return NotFound(new { message = "No events found." });
//            }

//            events = sortOrder.ToLower() == "desc"
//                ? events.OrderByDescending(e => e.StartDate).ToList()
//                : events.OrderBy(e => e.StartDate).ToList();

//            return Ok(events);
//        }

//        // Get events within a date range
//        [HttpGet("dateRange")]
//        public async Task<IActionResult> GetEventsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
//        {
//            if (startDate > endDate)
//            {
//                return BadRequest(new { message = "Invalid date range. Start date should be before end date." });
//            }

//            var events = await _unitOfWork.events.FindAllAsync(e => e.StartDate >= startDate && e.StartDate <= endDate);

//            if (!events.Any())
//            {
//                return NotFound(new { message = "No events found within the given date range." });
//            }

//            return Ok(events.OrderBy(e => e.StartDate));
//        }

//        // Paginated retrieval of events
//        [HttpGet("paginated")]
//        public async Task<IActionResult> GetPaginatedEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
//        {
//            if (page < 1 || pageSize < 1)
//            {
//                return BadRequest(new { message = "Page and pageSize must be greater than zero." });
//            }

//            var allEvents = await _unitOfWork.events.GetAllAsync();
//            var pagedEvents = allEvents
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            return Ok(new
//            {
//                currentPage = page,
//                totalPages = (int)Math.Ceiling((double)allEvents.Count() / pageSize),
//                events = pagedEvents
//            });
//        }




//    }
//}



