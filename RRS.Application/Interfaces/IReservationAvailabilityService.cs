﻿using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface IReservationAvailabilityService
{
    Task<bool> IsReservationTimeValidAsync(Guid restaurantId, TimeSpan startTime, CancellationToken cancellationToken);
    Task<RestaurantTable> GetAvailableTableAsync(Guid restaurantId, DateTime reservationDate, TimeSpan startTime, TimeSpan endTime, int requestedSeats, CancellationToken cancellationToken);
}
