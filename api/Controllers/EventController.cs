using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;
using api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.UnitOfWork;

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

            var newEvent = new Event
            {
                Id = Guid.NewGuid().ToString(),
                Name = eventDto.Name,
                Description = eventDto.Description,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.events.AddAsync(newEvent);
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
            var eventToUpdate = await _unitOfWork.events.GetByIdAsync(id);
            if (eventToUpdate == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            eventToUpdate.Name = eventDto.Name;
            eventToUpdate.Description = eventDto.Description;
            eventToUpdate.StartDate = eventDto.StartDate;
            eventToUpdate.EndDate = eventDto.EndDate;
            eventToUpdate.UpdatedAt = DateTime.UtcNow;

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
    }
}



