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

    [HttpGet("GetUsers/{testValue}")]
    public string[] GetUsers(string testValue)
    {
        string[] responseArr = new string[]{
            "test1",
            "test2",
            testValue
        };

        return responseArr;
    }
}
