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
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Authorize]
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

        // GET: FavoriteHobby/{userId} trajaa lista ids mtaa l fvhobbies mtee l user 

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavoriteHobbies(string userId)
        {
            var favoriteHobbies = await _unitOfWork.favoriteHobbies.GetByUserIdAsync(userId);

            if (favoriteHobbies == null || !favoriteHobbies.Any())
            {
                return NotFound(new { message = $"No favorite hobbies found for user {userId}." });
            }

            var hobbyIds = favoriteHobbies.Select(fh => fh.HobbyId).ToList(); // Extract Hobby IDs

            return Ok(hobbyIds);
        }


        //ta3tiha id mte3 hobby traja3 l hobby info
        [HttpGet("hobby/{hobbyId}")]
        public async Task<IActionResult> GetHobbyById(string hobbyId)
        {
            Console.WriteLine($"🔍 Received Hobby ID: '{hobbyId}'");

            var hobby = await _unitOfWork.hobbies.GetByIdAsync(hobbyId);

            if (hobby == null)
            {
                Console.WriteLine($"❌ Hobby Not Found: '{hobbyId}'");
                return NotFound(new { message = "Hobby not found." });
            }

            var hobbyDto = _mapper.Map<HobbyDto>(hobby);
            return Ok(hobbyDto);
        }



        // GET: FavoriteHobby/users/{hobbyId}  trajaa l ids mtaa l users li andhom l hobby heki as a fv hobby
        [HttpGet("users/{hobbyId}")]
        public async Task<IActionResult> GetUsersByHobby(string hobbyId)
        {
            var favoriteHobbies = await _unitOfWork.favoriteHobbies.GetUsersByHobbyAsync(hobbyId, true);

            if (favoriteHobbies == null || !favoriteHobbies.Any())
            {
                return NotFound(new { message = "No users found for this hobby." });
            }

            var favoriteHobbyDtos = _mapper.Map<IEnumerable<FavoriteHobbyDto>>(favoriteHobbies);

            var usersIds = favoriteHobbies.Select(fh => fh.UserId).ToList();

            return Ok(usersIds);
        }



        // creating fvhobby 
        [HttpPost]
        public async Task<IActionResult> CreateFavoriteHobby([FromBody] FavoriteHobbyDto favoriteHobbyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(favoriteHobbyDto.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            // Check if the hobby exists and is NOT soft deleted
            var hobby = await _unitOfWork.hobbies.GetByIdAsync(favoriteHobbyDto.HobbyId);
            if (hobby == null || hobby.DeletedAt != null)  // Ensure hobby is not deleted
            {
                return BadRequest(new { message = "Hobby is not available or has been deleted." });
            }

            var favoriteHobby = _mapper.Map<FavoriteHobby>(favoriteHobbyDto);

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