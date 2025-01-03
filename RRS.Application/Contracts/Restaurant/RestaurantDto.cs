﻿using RRS.Application.Contracts.RestaurantManagerData;
using RRS.Core.Models;

namespace RRS.Application.Contracts.Restaurant;

public class RestaurantDto
{
    public Guid Id { get; set; }
    public List<RestaurantManagerDataDto> Manageres { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Address Address { get; set; }
    public string LogoUrl { get; set; }
    public TimeSpan OpeningHour { get; set; }
    public TimeSpan ClosingHour { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Website { get; set; }
    
}
