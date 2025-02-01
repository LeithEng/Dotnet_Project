using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    //[Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class FavoriteHobbyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public FavoriteHobbyController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: FavoriteHobby/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavoriteHobbies(string userId)
        {
            var favoriteHobbies = await _unitOfWork.favoriteHobbies.GetHobbiesByUserIdAsync(userId);

            if (favoriteHobbies == null || !favoriteHobbies.Any())
            {
                return NotFound(new { message = $"No favorite hobbies found for user {userId}." });
            }

            return Ok(favoriteHobbies);
        }



        // GET: FavoriteHobby/users/{hobbyId}
        [HttpGet("users/{hobbyId}")]
        public async Task<IActionResult> GetUsersByHobby(string hobbyId)
        {
            var favoriteHobbies = await _unitOfWork.favoriteHobbies.GetUsersByHobbyAsync(hobbyId, true);

            if (favoriteHobbies == null || !favoriteHobbies.Any())
            {
                return NotFound(new { message = "No users found for this hobby." });
            }

            var favoriteHobbyDtos = _mapper.Map<IEnumerable<FavoriteHobbyDto>>(favoriteHobbies);
            return Ok(favoriteHobbyDtos);
        }

        // POST: FavoriteHobby
        [HttpPost]
        public async Task<IActionResult> CreateFavoriteHobby([FromBody] FavoriteHobbyDto favoriteHobbyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the user exists using UserManager
            var user = await _userManager.FindByIdAsync(favoriteHobbyDto.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            // Map DTO to FavoriteHobby entity
            var favoriteHobby = _mapper.Map<FavoriteHobby>(favoriteHobbyDto);

            // Save favorite hobby
            await _unitOfWork.favoriteHobbies.AddAsync(favoriteHobby);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetFavoriteHobbies), new { userId = favoriteHobby.UserId }, _mapper.Map<FavoriteHobbyDto>(favoriteHobby));
        }


        // DELETE: FavoriteHobby/remove/{userId}/{hobbyId}
        [HttpDelete("remove/{userId}/{hobbyId}")]
        public async Task<IActionResult> RemoveFavoriteHobby(string userId, string hobbyId)
        {
            var favoriteHobby = await _unitOfWork.favoriteHobbies.GetByUserAndHobbyIdAsync(userId, hobbyId);

            if (favoriteHobby == null)
                return NotFound(new { message = "Favorite hobby not found." });

            _unitOfWork.favoriteHobbies.Delete(favoriteHobby);
             _unitOfWork.Complete();

            return NoContent();
        }

    }
}
 