using AutoMapper;
using OpenLibrary.Application.Dtos;
using OpenLibrary.Domain.Entities;

namespace Rent2Park.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BookDto, Book>();
    }
}


