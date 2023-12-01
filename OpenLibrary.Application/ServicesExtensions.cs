using Microsoft.Extensions.DependencyInjection;
using OpenLibrary.Application.Interfaces;
using OpenLibrary.Application.Services;
using Rent2Park.Application.AutoMapper;

namespace OpenLibrary.Application;

public static class ServicesExtensions
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddTransient<IBookService, BookService>();
    }
}