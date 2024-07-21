using Microsoft.EntityFrameworkCore;
using WebApplication1.Libraries.Data;

namespace WebApplication1;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

        if (env.IsDevelopment())
        {
            builder.AddUserSecrets<Startup>();
        }

        builder.AddEnvironmentVariables();

        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<MyBooksDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
    }
}