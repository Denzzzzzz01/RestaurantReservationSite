﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurant.Queries.SearchRestaurants;

public class SearchRestaurantsQueryHandler : IRequestHandler<SearchRestaurantsQuery, List<RestaurantSummaryDto>>
{
    private readonly IAppDbContext _dbContext;

    public SearchRestaurantsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RestaurantSummaryDto>> Handle(SearchRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchCriteria.Query?.ToLower();

        var restaurants = string.IsNullOrWhiteSpace(searchTerm)
            ? new List<RestaurantSummaryDto>()
            : await _dbContext.Restaurants
                .Where(r => r.Name.ToLower().Contains(searchTerm) ||
                            r.Address.Street.ToLower().Contains(searchTerm) ||
                            r.Address.City.ToLower().Contains(searchTerm) ||
                            r.Address.Country.ToLower().Contains(searchTerm))
                .Select(r => new RestaurantSummaryDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address, 
                    OpeningHour = r.OpeningHour,
                    ClosingHour = r.ClosingHour
                })
                .ToListAsync(cancellationToken);

        return restaurants;
    }
}