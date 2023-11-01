using Microsoft.AspNetCore.Mvc;

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
    public IResult GetUser(string id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                return Results.Json(user);
            }
        }
        return Results.NotFound(new { message = "Not Found" });
    }

    [HttpGet("users")]
    public List<User> GetUsers()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var users = db.Users.ToList();
            return users;
        }
    }

    [HttpPost("user")]
    public User PostUser(string name, string surname, int age)
    {
        var user = new User(name, surname, age);
        user.Id = Guid.NewGuid().ToString();
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        return user;
    }

    [HttpPut("user")]
    public IResult ChangeUser(string id, string name, string surname, int age)
    {
        var userData = new User(name, surname, age);
        userData.Id = id;
        using (ApplicationContext db = new ApplicationContext())
        {
            var user = db.Users.Find(userData.Id);
            if (user != null)
            {
                user.Age = userData.Age;
                user.Name = userData.Name;
                user.Surname = userData.Surname;
                db.SaveChanges();
                return Results.Json(user);
            }
        }
        return Results.NotFound(new { message = "Not Found" });
    }

    [HttpDelete("{id}")]
    public IResult DeleteUser(string id)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return Results.Json(user);
            }
        }
        return Results.NotFound(new { message = "Not Found" });
    }
}
