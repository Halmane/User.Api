using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
var users = new List<User>();

app.UseDefaultFiles();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureCreated();
}

app.MapPost(
    "/api/users",
    (User user) =>
    {
        user.Id = Guid.NewGuid().ToString();
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        return user;
    }
);
app.MapPut(
    "/api/users",
    (User userData) =>
    {
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
);
app.MapDelete(
    "/api/users/{id}",
    (string id) =>
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
);
app.MapGet(
    "/api/users/{id}",
    (string id) =>
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
);
app.MapGet(
    "/api/users",
    () =>
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            users = db.Users.ToList();
        }
        return users;
    }
);

app.Run();

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    public User(string name, string surname, int age)
    {
        Name = name;
        Surname = surname;
        Age = age;
    }
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Users.db");
    }
}
