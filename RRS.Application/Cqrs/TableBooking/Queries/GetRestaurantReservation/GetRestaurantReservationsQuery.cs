using MediatR;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetRestaurantReservation;

public record GetRestaurantReservationsQuery(Guid RestaurantId, ReservationFilter Filter) : IRequest<List<Reservation>>;
