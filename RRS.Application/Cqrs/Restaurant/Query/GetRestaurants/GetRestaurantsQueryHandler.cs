using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.AppUser;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Contracts.RestaurantManagerData;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurant.Query.GetRestaurants
{
    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, List<RestaurantDto>>
    {
        private readonly IAppDbContext _dbContext;

        public GetRestaurantsQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RestaurantDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var restaurants = await _dbContext.Restaurants
                .Include(r => r.Manageres)
                    .ThenInclude(m => m.AppUser)
                .Include(r => r.Address)
                .ToListAsync(cancellationToken);

            return restaurants.Select(r => new RestaurantDto
            {
                Id = r.Id,
                Manageres = r.Manageres.Select(m => new RestaurantManagerDataDto
                {
                    AppUser = m.AppUser.Adapt<AppUserDto>(),
                }).ToList(),
                Name = r.Name,
                Address = r.Address,
                SeatingCapacity = r.SeatingCapacity,
                OpeningHour = r.OpeningHour,
                ClosingHour = r.ClosingHour,
                PhoneNumber = r.PhoneNumber,
                Website = r.Website
            }).ToList();
        }
    }
}

