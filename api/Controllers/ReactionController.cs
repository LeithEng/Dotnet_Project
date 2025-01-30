using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.DTOs;
using api.UnitOfWork;
using api.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using System.Security.Claims;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("posts/{postId}/reactions")]
    public class ReactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReactionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: posts/{postId}/reactions ta3ti reactions li 3la post
        [HttpGet]
        public async Task<IActionResult> GetReactionsForPost(string postId)
        {
            var reactions = await _unitOfWork.reactions.FindAllAsync(r => r.PostId == postId && r.DeletedAt == null);
            var reactionDTOs = _mapper.Map<IEnumerable<ReactionDto>>(reactions);
            return Ok(reactionDTOs);
        }

        // POST: posts/{postId}/reactions  ta3mal reaction 3la post 
        [HttpPost]
        public async Task<IActionResult> MakeReaction(string postId, [FromBody] ReactionDto reactionDto)
        {
            // Validate the model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the current authenticated user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            // Prevent duplicate reactions from the same user on the same post
            var existingReactions = await _unitOfWork.reactions.FindAllAsync(r => r.UserId == userId && r.PostId == postId && r.DeletedAt == null);
            if (existingReactions.Any())
                return BadRequest(new { message = "User has already reacted to this post." });

            // Map ReactionDto to Reaction entity
            var reaction = _mapper.Map<Reaction>(reactionDto);

            // Set additional properties
            reaction.PostId = postId;
            reaction.UserId = userId;

            // Automatically generate a unique Id for the reaction
            reaction.Id = Guid.NewGuid().ToString();  // Generate a new GUID as a string for the Id

            // Add the reaction to the repository
            await _unitOfWork.reactions.AddAsync(reaction);

            // Complete the unit of work
            _unitOfWork.Complete();  // Ensure the changes are persisted

            return Ok(new { message = "Reaction added successfully" });
        }


        // DELETE: posts/{postId}/reactions/{reactionId} tfasa5 react 3la post
        [HttpDelete("{reactionId}")]
        public async Task<IActionResult> DeleteReaction(string postId, string reactionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            var reaction = await _unitOfWork.reactions.GetByIdAsync(reactionId);
            if (reaction == null || reaction.PostId != postId || reaction.DeletedAt != null)
                return NotFound(new { message = "Reaction not found for this post." });

            if (reaction.UserId != userId)
                return Forbid();

            _unitOfWork.reactions.SoftDelete(reaction);
 
            _unitOfWork.Complete();

            return Ok(new {message = "reaction deleted" });
        }
    }
}