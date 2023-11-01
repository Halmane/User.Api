using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace UserApi;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Users.db");
    }

    public async Task<User> AddUserAsync(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User userData)
    {
        var user = await Users.FindAsync(userData.Id);
        if (user != null)
        {
            user.Age = userData.Age;
            user.Name = userData.Name;
            user.Surname = userData.Surname;
            await SaveChangesAsync();
        }
        return user;
    }

    public async Task<User> DeleteUserAsync(string id)
    {
        var user = await Users.FindAsync(id);
        if (user != null)
        {
            Users.Remove(user);
            SaveChanges();
        }
        return user;
    }

    public async Task<User> GetUserAsync(string id)
    {
        var user = await Users.FindAsync(id);
        return user;
    }

    public async Task<List<User>> GetUsersAsync(int page = 0, int pageSize = 10)
    {
        return await Users.Skip(page * pageSize).Take(pageSize).ToListAsync();
    }
}
