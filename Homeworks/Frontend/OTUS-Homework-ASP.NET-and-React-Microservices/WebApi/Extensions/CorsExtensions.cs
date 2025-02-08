namespace WebApi.Extensions;

using Microsoft.Extensions.DependencyInjection;

public static class ApiCorsPolicies
{
    public const string AllowAllOrigins = "AllowAllOrigins";
    public const string AllowSpecificOrigin = "AllowSpecificOrigin";
    public const string AllowMultipleOrigins = "AllowMultipleOrigins";
    public const string AllowSpecificRoute = "AllowSpecificRoute";
}

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApiCorsPolicies.AllowAllOrigins, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy(ApiCorsPolicies.AllowSpecificOrigin, policy =>
            {
                policy.WithOrigins("https://localhost:5272")
                    .WithMethods("GET", "POST")
                    .WithHeaders("Content-Type", "Authorization");
            });

            options.AddPolicy(ApiCorsPolicies.AllowMultipleOrigins, policy =>
            {
                policy.WithOrigins("https://example.com", "http://example.com", "http://localhost:5272")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy(ApiCorsPolicies.AllowSpecificRoute, policy =>
            {
                policy.WithOrigins("http://localhost:5683")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}