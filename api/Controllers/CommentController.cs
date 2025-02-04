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
    [Authorize]  
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

     
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
        {
            // Validate the model
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

            // Get the user ID from the current authenticated user (no need to pass it in the body)
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user exists (this is optional, as the user is already validated by the JWT token)
            var user = await _unitOfWork.UserManager.FindByIdAsync(currentUserId);
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

            // Create the new comment and associate it with the authenticated user
            var newComment = _mapper.Map<Comment>(commentDto);
            newComment.Id = Guid.NewGuid().ToString();
            newComment.UserId = currentUserId;  // Set the UserId from the authenticated user
            newComment.User = user;  
            newComment.Post = post;

            // Add the comment to the repository
            await _unitOfWork.comments.AddAsync(newComment);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetCommentById), new { id = newComment.Id }, newComment);
        }



        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            var comments = await _unitOfWork.comments.FindAllAsync(c => c.PostId == postId);

            if (!comments.Any())
            {
                return NotFound(new { message = "No comments found for the specified post." });
            }

            // Format the response to include UserId
            var response = comments.Select(comment => new
            {
                comment.Id,
                comment.Content,
                comment.CreatedAt,
                comment.UpdatedAt,
                UserId = comment.UserId, 
                PostId = comment.PostId
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var comment = await _unitOfWork.comments.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound(new { message = "Comment not found." });
            }

            // Return the comment with the UserId included in the response
            var response = new
            {
                comment.Id,
                comment.Content,
                comment.CreatedAt,
                comment.UpdatedAt,
                UserId = comment.UserId, 
                PostId = comment.PostId
            };

            return Ok(response);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, [FromBody] UpdateCommentDto commentDto)
        {
            // Retrieve the comment to be updated
            var commentToUpdate = await _unitOfWork.comments.GetByIdAsync(id);
            if (commentToUpdate == null)
            {
                return NotFound(new { message = "Comment not found." });
            }

            // Ensure the current user is authorized to update the comment
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (commentToUpdate.UserId != currentUserId)
            {
                return Unauthorized(new { message = "You are not authorized to update this comment." });
            }

            // Only update fields if provided (ignore empty or default values)
            if (!string.IsNullOrEmpty(commentDto.Content) && commentDto.Content != "string")
            {
                commentToUpdate.Content = commentDto.Content;
            }

            // Keep the `PostId` as is from the existing comment (no need to pass in request)
            commentToUpdate.PostId = commentToUpdate.PostId;  // This will keep the PostId the same
            commentToUpdate.UserId = currentUserId;  // Keep the current user as the owner of the comment

            // Update the comment in the repository
            _unitOfWork.comments.Update(commentToUpdate);
            _unitOfWork.Complete();

            return Ok(new { message = "Comment updated successfully." });
        }


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
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

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
