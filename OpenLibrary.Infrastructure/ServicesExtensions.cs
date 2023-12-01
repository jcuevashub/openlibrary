using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenLibrary.Domain.Interfaces;
using OpenLibrary.Infrastructure.Repositories;

namespace OpenLibrary.Infrastructure;
public static class ServicesExtensions
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookRepository, BookRepository>();
    }
}