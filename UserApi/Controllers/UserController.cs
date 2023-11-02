using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    readonly ILogger<UserController> _logger;
    private readonly ApplicationContext _context;

    public UserController(ApplicationContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetUserAsync(string id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var result = await db.GetUserAsync(id);
            if (result != null)
                return Results.Json(result);
            _logger.LogError("GetUserAsync => User Not Found {@result}", result);
            return Results.NotFound(new { message = "Not Found" });
        }
    }

    [HttpGet("users/{page}")]
    public async Task<IResult> GetUsersAsync(int page = 0, int pageSize = 10)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var result = await db.GetUsersAsync(page, pageSize);
            if (result != null)
                return Results.Json(result);
            _logger.LogError("GetUsersAsync => User Not Found {@result}", result);
            return Results.NotFound(new { message = "Not Found" });
        }
    }

    [HttpPost("create")]
    public async Task<IResult> PostUserAsync(User user)
    {
        if (ModelState.IsValid)
        {
            user.Id = Guid.NewGuid().ToString();
            using (ApplicationContext db = new ApplicationContext())
            {
                return Results.Json(await db.AddUserAsync(user));
            }
        }
        _logger.LogError("PostUserAsync => User Incorrect data {@result}", user);
        return Results.BadRequest(new { message = "Incorrect data" });
    }

    [HttpPut("update")]
    public async Task<IResult> ChangeUserAsync(User userData)
    {
        if (ModelState.IsValid)
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = await db.UpdateUserAsync(userData);
                if (result != null)
                    return Results.Json(result);
                _logger.LogError("ChangeUserAsync => User Not Found {@result}", result);
                return Results.NotFound(new { message = "Not Found" });
            }
        _logger.LogError("ChangeUserAsync => User Incorrect data {@result}", userData);
        return Results.BadRequest(new { message = "Incorrect data" });
    }

    [HttpDelete("{id}/delete")]
    public async Task<IResult> DeleteUserAsync(string id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var result = await db.DeleteUserAsync(id);
            if (result != null)
                return Results.Json(result);
            _logger.LogError("DeleteUserAsync => User Not Found {@result}", result);
            return Results.NotFound(new { message = "Not Found" });
        }
    }
}
