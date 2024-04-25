using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    // Create dapper private var
    DataContextDapper _dapper;
    public UserController(IConfiguration configuration)
    {
        // Access Dapper and pass the config to access the connection string into that class
        _dapper = new DataContextDapper(configuration);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        // Run dapper with SQL Query to return the result from the database
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users WHERE UserId=" + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    // Put/Post request can expect a response body of any model in its parameter
    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users 
            SET   [FirstName]= '" + user.FirstName +
            "', [LastName]= '" + user.LastName +
            "', [Email]= '" + user.Email +
            "', [Gender]= '" + user.Gender +
            "', [Active]= '" + user.Active +
            "' WHERE UserId=" + user.UserId;
        if (_dapper.ExecuteSql(sql))
        {
            // This is normally Http Response of type IActionResult coming from Controller base class
            return Ok();
        }
        throw new Exception("Failed to update user");
    }
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users (
                    [FirstName],
                    [LastName],
                    [Email],
                    [Gender],
                    [Active] )
                    VALUES (" +
                    "'" + user.FirstName +
                        "','" + user.LastName +
                        "','" + user.Email +
                        "','" + user.Gender +
                        "','" + user.Active +
                        "')";
        if (_dapper.ExecuteSql(sql))
        {
            // This is normally Http Response of type IActionResult coming from Controller base class
            return Ok();
        }
        throw new Exception("Failed to add user");
    }
}
