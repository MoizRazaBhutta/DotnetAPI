using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    IUserRepository _userRepository;

    public UserEFController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
    }
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    // Put/Post request can expect a response body of any model in its parameter
    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        // If we find a matching object from db to the one we have
        User? userDb = _userRepository.GetSingleUser(user.UserId);
        if(userDb != null)
        {
            // Update the db value with the one user we have
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            // Save Changes if no of row effected is one
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }
            // Else throw error
            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to Get User");
    }
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        User userDb = new User(); 
        // Update the db value with the one user we have
        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        _userRepository.AddEntity<User>(userDb);
        // Save Changes if no of row effected is one
        if(_userRepository.SaveChanges())
        {
            return Ok();
        }
        // Else throw error
        throw new Exception("Failed to add user");
    }
    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);

        if(userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            // Save Changes if no of row effected is one
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }
            // Else throw error
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to get user");
    }
}
