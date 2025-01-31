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
    //[Authorize]  // Restrict access to authenticated users
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

        // POST: api/Post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
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

            var user = await _unitOfWork.UserManager.FindByIdAsync(postDto.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "User does not exist." });
            }

            var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == postDto.HobbyId);
            if (hobby == null)
            {
                return BadRequest(new { message = "Invalid Hobby ID provided." });
            }

            // Create the new post
            //var newPost = new Post
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Content = postDto.Content,
            //    ImageUrl = postDto.ImageUrl,
            //    UserId = postDto.UserId,
            //    User = user,
            //    HobbyId = postDto.HobbyId,
            //    Hobby = hobby,
            //    CreatedAt = DateTime.UtcNow,
            //    UpdatedAt = DateTime.UtcNow
            //};

            var newPost = _mapper.Map<Post>(postDto);
            newPost.Id = Guid.NewGuid().ToString();
            newPost.User = user;
            newPost.Hobby = hobby;
            newPost.CreatedAt = DateTime.UtcNow;
            newPost.UpdatedAt = DateTime.UtcNow;


            // Add the post to the repository
            await _unitOfWork.posts.AddAsync(newPost);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id }, newPost);
        }

        // GET: api/Post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            var post = await _unitOfWork.posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { message = "Post not found." });
            }

            return Ok(post);
        }

        // GET: api/Post/random
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPosts([FromQuery] int count = 5)
        {
            var posts = await _unitOfWork.posts.GetAllAsync();
            var randomPosts = posts.OrderBy(x => Guid.NewGuid()).Take(count).ToList();

            if (!randomPosts.Any())
            {
                return NotFound(new { message = "No posts found." });
            }

            return Ok(randomPosts);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] PostDto postDto)
        {
            // Retrieve the current user ID from the JWT token
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the post from the database
            var postToUpdate = await _unitOfWork.posts.GetByIdAsync(id);
            if (postToUpdate == null)
            {
                return NotFound(new { message = "Post not found." });
            }

            // Check if the current user is the creator of the post
            if (postToUpdate.UserId != currentUserId)
            {
                return Unauthorized(new { message = "You are not authorized to update this post." });
            }

            // Check if the hobby ID provided is valid
            if (!string.IsNullOrEmpty(postDto.HobbyId))
            {
                var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == postDto.HobbyId);
                if (hobby == null)
                {
                    return BadRequest(new { message = "Invalid Hobby ID provided." });
                }
                postToUpdate.HobbyId = postDto.HobbyId;
                postToUpdate.Hobby = hobby;
            }

            // Map the updated postDto properties to the postToUpdate entity
            _mapper.Map(postDto, postToUpdate);
            postToUpdate.UpdatedAt = DateTime.UtcNow;

            // Update the post in the database
            _unitOfWork.posts.Update(postToUpdate);
            _unitOfWork.Complete();

            return Ok(new { message = "Post updated successfully." });
        }

        //// PUT: api/Post/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdatePost(string id, [FromBody] PostDto postDto)
        //{
        //    var postToUpdate = await _unitOfWork.posts.GetByIdAsync(id);
        //    if (postToUpdate == null)
        //    {
        //        return NotFound(new { message = "Post not found." });
        //    }

        //    if (!string.IsNullOrEmpty(postDto.HobbyId))
        //    {
        //        var hobby = await _unitOfWork.hobbies.FindAsync(h => h.Id == postDto.HobbyId);
        //        if (hobby == null)
        //        {
        //            return BadRequest(new { message = "Invalid Hobby ID provided." });
        //        }
        //        postToUpdate.HobbyId = postDto.HobbyId;
        //        postToUpdate.Hobby = hobby;
        //    }

        //    _mapper.Map(postDto, postToUpdate);
        //    postToUpdate.UpdatedAt = DateTime.UtcNow;


        //    _unitOfWork.posts.Update(postToUpdate);
        //    _unitOfWork.Complete();

        //    return Ok(new { message = "Post updated successfully." });
        //}

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



        //public async Task<IActionResult> DeletePost(string id)
        //{
        //    var postToDelete = await _unitOfWork.posts.GetByIdAsync(id);
        //    if (postToDelete == null)
        //    {
        //        return NotFound(new { message = "Post not found." });
        //    }

        //    _unitOfWork.posts.SoftDelete(postToDelete);
        //    _unitOfWork.Complete();

        //    return Ok(new { message = "Post deleted successfully." });
        //}

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

