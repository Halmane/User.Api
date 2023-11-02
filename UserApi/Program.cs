using Microsoft.EntityFrameworkCore;
using Serilog;
using UserApi;

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Error()
        .WriteTo.Console()
        .WriteTo.File("logs/errorLog-", rollingInterval: RollingInterval.Day)
        .CreateLogger();

try {
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ApplicationDBContext>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Host.UseSerilog(Log.Logger);

    var app = builder.Build();

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

    using (ApplicationDBContext db = new ApplicationDBContext())
    {
        db.Database.EnsureCreated();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

