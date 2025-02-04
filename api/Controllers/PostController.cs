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
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace api.Controllers
{
    [Authorize]  
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

       
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
        {
            // Ensure the model state is valid
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

            // Get the authenticated user's ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            // Retrieve the user from the database based on the currentUserId
            var user = await _unitOfWork.UserManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                return BadRequest(new { message = "User does not exist." });
            }

            // Retrieve the hobby from the database
            var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == postDto.HobbyId);
            if (hobby == null)
            {
                return BadRequest(new { message = "Invalid Hobby ID provided." });
            }

            // Map the PostDto to a Post object
            var newPost = _mapper.Map<Post>(postDto);

            // Set additional properties
            newPost.Id = Guid.NewGuid().ToString();
            newPost.User = user;
            newPost.Hobby = hobby;
            newPost.CreatedAt = DateTime.UtcNow;

            // Add the post to the repository
            await _unitOfWork.posts.AddAsync(newPost);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id }, newPost);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            var post = await _unitOfWork.posts
                .Include(p => p.Comments)  // Eagerly load Comments
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound(new { message = "Post not found." });
            }

            return Ok(post);
        }


        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPosts([FromQuery] int count = 5)
        {


            var posts = await _unitOfWork.posts
                .Include(p => p.Comments)  // Eagerly load Comments
                .Take(count)
                .ToListAsync();

            if (!posts.Any())
            {
                return NotFound(new { message = "No posts found." });
            }

            return Ok(posts);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdatePost(string id, [FromBody] PostDto postDto)
        {
            // Get the authenticated user's ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the post by its ID
            var postToUpdate = await _unitOfWork.posts.GetByIdAsync(id);
            if (postToUpdate == null)
            {
                return NotFound(new { message = "Post not found." });
            }

            // Ensure the authenticated user is the owner of the post
            if (postToUpdate.UserId != currentUserId)
            {
                return Unauthorized(new { message = "You are not authorized to update this post." });
            }

            // Validate HobbyId (ignore default placeholder values)
            if (!string.IsNullOrEmpty(postDto.HobbyId) && postDto.HobbyId != "string")
            {
                var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == postDto.HobbyId);
                if (hobby == null)
                {
                    return BadRequest(new { message = "Invalid Hobby ID provided." });
                }
                postToUpdate.HobbyId = postDto.HobbyId;
                postToUpdate.Hobby = hobby;
            }

            // Only update fields if provided (ignore empty or default values)
            if (!string.IsNullOrEmpty(postDto.Content) && postDto.Content != "string")
            {
                postToUpdate.Content = postDto.Content;
            }

            if (!string.IsNullOrEmpty(postDto.ImageUrl) && postDto.ImageUrl != "string")
            {
                postToUpdate.ImageUrl = postDto.ImageUrl;
            }

            // Keep fixed values
            postToUpdate.Id = id;
            postToUpdate.UserId = currentUserId; 

            // Update the post in the repository
            _unitOfWork.posts.Update(postToUpdate);
            _unitOfWork.Complete();

            return Ok(new { message = "Post updated successfully." });
        }


        // DELETE: api/Post/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            // Retrieve the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the post from the database
            var postToDelete = await _unitOfWork.posts.GetByIdAsync(id);
            if (postToDelete == null)
            {
                return NotFound(new { message = "Post not found." });
            }

            // Check if the current user is the creator of the post
            if (postToDelete.UserId != currentUserId)
            {
                return Unauthorized(new { message = "You are not authorized to delete this post." });
            }

            // Proceed with deleting the post
            _unitOfWork.posts.SoftDelete(postToDelete);
            _unitOfWork.Complete();

            return Ok(new { message = "Post deleted successfully." });
        }

        // Search posts by Hobby ID
        [HttpGet("searchByHobby")]
        public async Task<IActionResult> SearchPostsByHobby([FromQuery] string hobbyId)
        {
            if (string.IsNullOrEmpty(hobbyId))
            {
                return BadRequest(new { message = "Hobby ID is required." });
            }

            var posts = await _unitOfWork.posts.FindAllAsync(p => p.HobbyId == hobbyId);

            if (!posts.Any())
            {
                return NotFound(new { message = "No posts found for the specified hobby." });
            }

            return Ok(posts);
        }

        // Search posts by Content (case-insensitive search)
        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest(new { message = "Search term is required." });
            }

            // Get the IQueryable to allow LINQ filtering
            var postsQuery = _unitOfWork.posts.GetQueryable()
                .Where(p => p.DeletedAt == null)
                .Where(p => EF.Functions.Like(p.Content, $"%{searchTerm}%"));

            var posts = await postsQuery.ToListAsync();

            if (!posts.Any())
            {
                return NotFound(new { message = "No posts found matching the search criteria." });
            }

            return Ok(posts);
        }

        // Paginated retrieval of posts
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { message = "Page and pageSize must be greater than zero." });
            }

            var allPosts = await _unitOfWork.posts.GetAllAsync();
            var pagedPosts = allPosts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                currentPage = page,
                totalPages = (int)Math.Ceiling((double)allPosts.Count() / pageSize),
                posts = pagedPosts
            });
        }
    }
}

