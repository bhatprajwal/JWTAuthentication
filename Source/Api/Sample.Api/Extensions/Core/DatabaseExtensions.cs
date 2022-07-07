using Dao.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Sample.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        return services;
    }
}