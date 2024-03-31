using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController()
    {
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