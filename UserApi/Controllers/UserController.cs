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
    private readonly ILogger<UserController> _logger;
    private readonly ApplicationContext _context;

    public UserController(ApplicationContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetUserAsync(string id)
    {
        try
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = await db.GetUserAsync(id);
                if (result != null)
                    return Results.Json(result);
                _logger.LogWarning("{@MethodName} => User Not Found {@id}", nameof(GetUserAsync), id);
                return Results.NotFound(new { message = "Not Found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "an error occurred in method {@MethodName}", nameof(GetUserAsync));
            return Results.StatusCode(500);
        }
    }

    [HttpGet("users/{page}")]
    public async Task<IResult> GetUsersAsync(int page = 0, int pageSize = 10)
    {
        try
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = await db.GetUsersAsync(page, pageSize);
                if (result != null)
                    return Results.Json(result);
                _logger.LogWarning("{@MethodName} => Users Not Found {@result}", nameof(GetUsersAsync), result);
                return Results.NotFound(new { message = "Not Found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "an error occurred in method {@MethodName}", nameof(GetUsersAsync));
            return Results.StatusCode(500);
        }
    }

    [HttpPost("create")]
    public async Task<IResult> PostUserAsync(User user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("{@MethodName} => User Incorrect data {@result}", nameof(PostUserAsync), user);
                return Results.BadRequest(new { message = "Incorrect data" });
            }
            user.Id = Guid.NewGuid().ToString();
            using (ApplicationContext db = new ApplicationContext())
            {
                return Results.Json(await db.AddUserAsync(user));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "an error occurred in method {@MethodName}", nameof(PostUserAsync));
            return Results.StatusCode(500);
        }
    }

    [HttpPut("update")]
    public async Task<IResult> ChangeUserAsync(User userData)
    {
        try
        {
            if (ModelState.IsValid)
                using (ApplicationContext db = new ApplicationContext())
                {
                    var result = await db.UpdateUserAsync(userData);
                    if (result != null)
                        return Results.Json(result);
                    _logger.LogError("ChangeUserAsync => User Not Found {@id}", userData.Id);
                    return Results.NotFound(new { message = "Not Found" });
                }
            _logger.LogWarning("{@MethodName} => User Incorrect data {@result}", nameof(ChangeUserAsync), userData);
            return Results.BadRequest(new { message = "Incorrect data" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "an error occurred in method {@MethodName}", nameof(ChangeUserAsync));
            return Results.StatusCode(500);
        }
    }

    [HttpDelete("{id}/delete")]
    public async Task<IResult> DeleteUserAsync(string id)
    {
        try
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = await db.DeleteUserAsync(id);
                if (result != null)
                    return Results.Json(result);
                _logger.LogWarning("{@MethodName} => User Not Found {@id}", nameof(DeleteUserAsync), id);
                return Results.NotFound(new { message = "Not Found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "an error occurred in method {@MethodName}", nameof(DeleteUserAsync));
            return Results.StatusCode(500);
        }
    }
}
