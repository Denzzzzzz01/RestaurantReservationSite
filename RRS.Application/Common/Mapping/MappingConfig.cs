using Mapster;
using RRS.Application.Contracts.AppUser;
using RRS.Application.Contracts.Reservations;
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

        TypeAdapterConfig<Reservation, UserReservationDto>.NewConfig()
            .Map(dest => dest.RestaurantName, src => src.Restaurant.Name)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);

        TypeAdapterConfig<Reservation, RestaurantReservationDto>.NewConfig()
            .Map(dest => dest.UserName, src => src.User.UserName)    
            .Map(dest => dest.UserContact, src => src.User.Email)
            .Map(dest => dest.UserId, src => src.UserId);

    }
}
