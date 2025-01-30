using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api.Data;
namespace api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly AuthenticationController _authController;
    private readonly IMapper _mapper;
    public UserController(UserManager<User> userManager, IMapper mapper, AuthenticationController authenticationController, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _mapper = mapper;
        _authController = authenticationController;
        _dbContext = dbContext;
    }
    [Authorize]
    [HttpPost("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return NotFound();
        }
        if (await checkUserNameUnique(user.UserName, currentUser.Id))
        {
            return BadRequest("the username is already taken");
        }
        if (await CheckEmailUnique(user.Email, currentUser.Id))
        {
            return BadRequest("the email is already taken");
        }
        var removePasswordResult = await _userManager.RemovePasswordAsync(currentUser);
        if (!removePasswordResult.Succeeded)
        {
            return BadRequest(removePasswordResult.Errors);
        }

        var addPasswordResult = await _userManager.AddPasswordAsync(currentUser, user.Password);
        if (!addPasswordResult.Succeeded)
        {
            return BadRequest(addPasswordResult.Errors);
        }
        currentUser.UserName = user.UserName;
        currentUser.FirstName = user.FirstName;
        currentUser.LastName = user.LastName;
        currentUser.Email = user.Email;
        currentUser.UpdatedAt = DateTime.Now;
        await _userManager.UpdateAsync(currentUser);
        return Ok("User updated successfully");
    }
    private async Task<bool> checkUserNameUnique(string userName, string id)
    {
        return _userManager.Users
    .Any(u => u.UserName == userName && u.Id != id);

    }
    private async Task<bool> CheckEmailUnique(string email, string id)
    {
        return _userManager.Users
            .Any(u => u.Email == email && u.Id != id);
    }
    [Authorize]
    [HttpGet("GetProfile")]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        var userProfile = _mapper.Map<GetProfileDto>(user);
        userProfile.roles = (await _userManager.GetRolesAsync(user)).ToArray();
        return Ok(userProfile);
    }
    [Authorize]
    [HttpDelete("DeleteProfile")]
    public async Task<IActionResult> DeleteProfileByUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok("User deleted successfully");
        }
        return BadRequest(result.Errors);

    }
    [Authorize]
    [HttpGet("GetFriendsOfUser")]
    public async Task<IActionResult> GetFriendsOfUser()
    {
        var user = await _userManager.GetUserAsync(User);
        var userWithFriends = await _userManager.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == user.Id); ;
        if (userWithFriends == null)
        {
            return NotFound();
        }
        List<User> friends = userWithFriends.Friends.ToList();
        List<GetProfileDto> friendsProfiles = _mapper.Map<List<GetProfileDto>>(friends);
        return Ok(friendsProfiles);
    }
    [Authorize]
    [HttpGet("SerchUser")]
    public async Task<IActionResult> SearchUser(string searchTerm)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        var friends = await _userManager.Users.Where(u => u.UserName.Contains(searchTerm)).ToListAsync();
        var friendsProfiles = _mapper.Map<List<GetProfileDto>>(friends);
        return Ok(friendsProfiles);
    }
    //test if already freind
    [Authorize]
    [HttpPost("AddFriend")]
    public async Task<IActionResult> AddFriend(string username)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("you are authenticated :)");
        }
        if (username == user.UserName)
        {
            return BadRequest("You can't add yourself as a friend");
        }
        User friend = await _userManager.FindByNameAsync(username);
        if (friend != null)
        {
            user.Friends.Add(friend);
            await _userManager.UpdateAsync(user);
            friend.Friends.Add(user);
            await _userManager.UpdateAsync(friend);
            return Ok("Friend added successfully");
        }
        return NotFound("friend not found !!");
    }
    [Authorize]
    [HttpPost("removeFriend")]
    public async Task<IActionResult> RemoveFriend(string username)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        User friend = await _userManager.Users
       .Include(u => u.Friends)
       .FirstOrDefaultAsync(u => u.UserName == username);
        var userWithFriends = await _userManager.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == user.Id); ;
        if (userWithFriends == null)
        {
            return NotFound();
        }
        List<User> friends = userWithFriends.Friends.ToList();
        bool isfriend = friends.Any(f => f.UserName == friend.UserName);
        if (!isfriend)
        {
            return BadRequest("You are not friend with this user");
        }
        if (friend != null)
        {

            userWithFriends.Friends.Remove(friend);
            await _userManager.UpdateAsync(userWithFriends);
            friend.Friends.Remove(user);
            await _userManager.UpdateAsync(friend);
            return Ok("Friend removed successfully");
        }
        return NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var userProfiles = _mapper.Map<List<GetProfileDto>>(users);
        return Ok(userProfiles);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteUserByAdmin")]
    public async Task<IActionResult> DeleteUserByAdmin(string username)
    {
        User user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return NotFound("user not found");
        }
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok("User deleted successfully");
        }
        return BadRequest(result.Errors);
    }
    //[Authorize(Roles = "Admin")]
    [HttpPost("AddAdmin")]
    public async Task<IActionResult> AddAdmin(RegisterDto registerDto)
    {
        registerDto.isAdmin = true;
        return await _authController.Register(registerDto);
    }

}

