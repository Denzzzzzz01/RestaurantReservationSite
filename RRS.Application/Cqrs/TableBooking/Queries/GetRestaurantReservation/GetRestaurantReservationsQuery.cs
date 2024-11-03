using MediatR;
using RRS.Application.Contracts.Reservations;
using RRS.Core.Enums;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetRestaurantReservation;

public record GetRestaurantReservationsQuery(Guid RestaurantId, ReservationFilter Filter) : IRequest<List<RestaurantReservationDto>>;
