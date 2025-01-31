using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.DTOs;
using api.UnitOfWork;
using api.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using api.Interfaces;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HobbyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IWebHostEnvironment _environment;

        public HobbyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           
        }

        // GET: api/Hobby/first-level
        [HttpGet("first-level")]
        public async Task<IActionResult> GetFirstLevelHobbies()
        {
            var hobbies = await _unitOfWork.hobbies.FindAllAsync(h => h.Level == 1 && h.DeletedAt == null);
            var hobbyDtos = _mapper.Map<IEnumerable<HobbyDto>>(hobbies);
            return Ok(hobbyDtos);
        }

        // GET: api/Hobby/children/{id}
        [HttpGet("children/{id}")]
        public async Task<IActionResult> GetChildrenHobbies(string id)
        {
            var hobbies = await _unitOfWork.hobbies.FindAllAsync(h => h.ParentHobbyId == id && h.DeletedAt == null);
            var hobbyDtos = _mapper.Map<IEnumerable<HobbyDto>>(hobbies);
            return Ok(hobbyDtos);
        }

        // GET: api/Hobby/{id}
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetHobby(string id)
        {
            var hobby = await _unitOfWork.hobbies.GetByIdAsync(id);
            if (hobby == null || hobby.DeletedAt != null)
            {
                return NotFound(new { message = "Hobby not found." });
            }
            return Ok(_mapper.Map<HobbyDto>(hobby));
        }

        // POST: api/Hobby
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateHobby([FromForm] HobbyDto hobbyDto, [FromForm] IFormFile? iconPictureFile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var hobby = _mapper.Map<Hobby>(hobbyDto);
            hobby.Id = Guid.NewGuid().ToString(); // Auto-generate ID

            await _unitOfWork.hobbies.AddAsync(hobby);
             _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetHobby), new { id = hobby.Id }, _mapper.Map<HobbyDto>(hobby));
        }

        // PUT: api/Hobby/{id}
        [HttpPut("update/{id}")]
        [Authorize(Roles = "ADMIN")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateHobby(string id, [FromForm] HobbyDto hobbyDto, [FromForm] IFormFile? iconPictureFile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var hobby = await _unitOfWork.hobbies.GetByIdAsync(id);
            if (hobby == null || hobby.DeletedAt != null)
                return NotFound(new { message = "Hobby not found." });

            _mapper.Map(hobbyDto, hobby);

            _unitOfWork.hobbies.Update(hobby);
            _unitOfWork.Complete();

            return NoContent();
        }

        // DELETE: api/Hobby/{id}
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteHobby(string id)
        {
            var hobby = await _unitOfWork.hobbies.GetByIdAsync(id);
            if (hobby == null || hobby.DeletedAt != null)
                return NotFound(new { message = "Hobby not found." });

            _unitOfWork.hobbies.SoftDelete(hobby);
             _unitOfWork.Complete();

            return  NoContent();
        }

    }
}
