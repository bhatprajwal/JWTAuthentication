using Auth;

namespace Sample.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services, IConfiguration config)
    {
        // Auth
        services.AddTransient<IUsers, Users>();
        services.AddTransient<IRoles, Roles>();
        services.AddTransient<IClaims, Claims>();
        services.AddTransient<ITokens, Tokens>();

        return services;
    }
}