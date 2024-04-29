using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    // Create dapper private var
    DataContextEF _entityFramework;

    public UserEFController(IConfiguration configuration)
    {
        // Access Dapper and pass the config to access the connection string into that class
        _entityFramework = new DataContextEF(configuration);

    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
        if(user != null)
        {
            return user;
        }
        throw new Exception("Failed to Get User");
    }

    // Put/Post request can expect a response body of any model in its parameter
    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        // If we find a matching object from db to the one we have
        User? userDb = _entityFramework.Users.Where(u => u.UserId == user.UserId).FirstOrDefault<User>();
        if(userDb != null)
        {
            // Update the db value with the one user we have
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            // Save Changes if no of row effected is one
            if(_entityFramework.SaveChanges() > 0 )
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
        _entityFramework.Add(userDb);
        // Save Changes if no of row effected is one
        if(_entityFramework.SaveChanges() > 0 )
        {
            return Ok();
        }
        // Else throw error
        throw new Exception("Failed to add user");
    }
    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();

        if(userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            // Save Changes if no of row effected is one
            if(_entityFramework.SaveChanges() > 0 )
            {
                return Ok();
            }
            // Else throw error
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to get user");
    }
}
