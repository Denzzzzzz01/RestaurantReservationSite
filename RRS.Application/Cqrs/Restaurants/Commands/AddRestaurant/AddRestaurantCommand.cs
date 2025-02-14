﻿using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Restaurant.Commands.AddRestaurant;

public record AddRestaurantCommand
(
    AppUser User,
    string Name,
    string Description,
    Address Address,
    TimeSpan OpeningHour,
    TimeSpan ClosingHour,
    string? PhoneNumber,
    string? Website
) : IRequest<Guid>;
