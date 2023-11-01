using Microsoft.AspNetCore.Mvc;
using System;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationContext _context;

    public UserController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetUserAync(string id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            await db.GetUserAsync(id);
        }
        return Results.NotFound(new { message = "Not Found" });
    }

    [HttpGet("users/{page}")]
    public List<User> GetUsers(int page)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.GetUsers(page);
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
                await db.AddUserAsync(user);
            }
            return Results.Json(user);
        }
        return Results.NotFound(new { message = "Incorrect data" });
    }

    [HttpPut("update")]
    public async Task<IResult> ChangeUserAsync(User userData)
    {
        if (ModelState.IsValid)
            using (ApplicationContext db = new ApplicationContext())
            {
                await db.UpdateUserAsync(userData);
            }
        return Results.NotFound(new { message = "Not Found" });
    }

    [HttpDelete("{id}/delete")]
    public async Task<IResult> DeleteUserAsync(string id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            await db.DeleteUserAsync(id);
        }
        return Results.NotFound(new { message = "Not Found" });
    }
}
