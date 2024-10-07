using Mapster;
using RRS.Application.Contracts.AppUser;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Contracts.RestaurantManagerData;
using RRS.Core.Models;

namespace RRS.Application.Common.Mapping;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Restaurant, RestaurantDto>.NewConfig()
            .Map(dest => dest.Manageres, src => src.Manageres.Adapt<List<RestaurantManagerDataDto>>());

        TypeAdapterConfig<RestaurantManagerData, RestaurantManagerDataDto>.NewConfig()
            .Map(dest => dest.AppUser, src => src.AppUser.Adapt<AppUserDto>());
    }
}
