﻿using MediatR;
using RRS.Application.Contracts.Reservations;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetUserReservations;

public record GetUserReservationsQuery(AppUser User, ReservationFilter Filter) : IRequest<List<UserReservationDto>>;
