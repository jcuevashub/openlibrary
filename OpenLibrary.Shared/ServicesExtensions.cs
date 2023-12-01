using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenLibrary.Application.Interfaces;
using OpenLibrary.Shared.Services;

namespace OpenLibrary.Shared;
public static class ServicesExtensions
{
    public static void ConfigureShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IOpenLibraryService, OpenLibraryService>();
        services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
    }
}