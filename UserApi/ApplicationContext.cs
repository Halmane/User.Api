using Microsoft.EntityFrameworkCore;

namespace UserApi;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Users.db");
    }

    public async Task AddUserAsync(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
    }

    public async Task<IResult> UpdateUserAsync(User userData)
    {
        var user = await Users.FindAsync(userData.Id);
        if (user != null)
        {
            user.Age = userData.Age;
            user.Name = userData.Name;
            user.Surname = userData.Surname;
            await SaveChangesAsync();
            return Results.Json(user);
        }
        return Results.NotFound(new { message = "Not Found" });
    }

    public async Task<IResult> DeleteUserAsync(string id)
    {
        var user = await Users.FindAsync(id);
        if (user != null)
        {
            Users.Remove(user);
            SaveChanges();
            return Results.Json(user);
        }
        return Results.NotFound(new { message = "Not Found" });
    }

    public async Task<IResult> GetUserAsync(string id)
    {
        var user = await Users.FindAsync(id);
        if (user != null)
        {
            return Results.Json(user);
        }
        return Results.NotFound(new { message = "Not Found" });
    }

    public List<User> GetUsers(int page)
    { 
        if (page <= 0)
            page = 1;
        var usersCount = Users.Count();
        var userCountOnPage = usersCount - (10 * (page-1));

        if (userCountOnPage > 10)
            userCountOnPage = 10;


        List<User> users;
        if (userCountOnPage < 0)
        {
            var skipedSures = (usersCount / 10) * 10;
            users = Users.Skip(skipedSures).Take(Users.Count() - skipedSures).ToList();
            return users;
        }
        users = Users.Skip(10 * (page - 1)).Take(userCountOnPage).ToList();
        return users;
    }
}
