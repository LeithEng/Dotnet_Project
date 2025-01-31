using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Interfaces;
using api.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using api.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
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

            // Check if the user exists
            var user = await _unitOfWork.UserManager.FindByIdAsync(commentDto.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User does not exist." });
            }

            // Check if the post exists
            var post = await _unitOfWork.posts.GetByIdAsync(commentDto.PostId);
            if (post == null)
            {
                return BadRequest(new { message = "Post does not exist." });
            }

            // Create the new comment
            var newComment = _mapper.Map<Comment>(commentDto);
            newComment.Id = Guid.NewGuid().ToString();
            newComment.User = user;
            newComment.Post = post;
            newComment.CreatedAt = DateTime.UtcNow;
            newComment.UpdatedAt = DateTime.UtcNow;

            // Add the comment to the repository
            await _unitOfWork.comments.AddAsync(newComment);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetCommentById), new { id = newComment.Id }, newComment);
        }

        // GET: api/Comment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var comment = await _unitOfWork.comments.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound(new { message = "Comment not found." });
            }

            return Ok(comment);
        }

        // GET: api/Comment/post/{postId}
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            var comments = await _unitOfWork.comments.FindAllAsync(c => c.PostId == postId);

            if (!comments.Any())
            {
                return NotFound(new { message = "No comments found for the specified post." });
            }

            return Ok(comments);
        }

        // PUT: api/Comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, [FromBody] CommentDto commentDto)
        {
            var commentToUpdate = await _unitOfWork.comments.GetByIdAsync(id);
            if (commentToUpdate == null)
            {
                return NotFound(new { message = "Comment not found." });
            }

            // Map the updated fields to the comment
            _mapper.Map(commentDto, commentToUpdate);
            commentToUpdate.UpdatedAt = DateTime.UtcNow;

            // Update the comment in the repository
            _unitOfWork.comments.Update(commentToUpdate);
            _unitOfWork.Complete();

            return Ok(new { message = "Comment updated successfully." });
        }

        // DELETE: api/Comment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            // Retrieve the comment to delete
            var commentToDelete = await _unitOfWork.comments.GetByIdAsync(id);
            if (commentToDelete == null)
            {
                return NotFound(new { message = "Comment not found." });
            }

            // Check if the logged-in user is the creator of the post or the comment
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming you're using JWT and the user ID is stored in the claims

            if (commentToDelete.UserId != currentUserId)
            {
                var post = await _unitOfWork.posts.GetByIdAsync(commentToDelete.PostId);
                if (post?.UserId != currentUserId)  // Check if the current user is the creator of the post
                {
                    return Unauthorized(new { message = "You are not authorized to delete this comment." });
                }
            }

            // Soft delete the comment
            _unitOfWork.comments.SoftDelete(commentToDelete);
            _unitOfWork.Complete();

            return Ok(new { message = "Comment deleted successfully." });
        }


    }
}
