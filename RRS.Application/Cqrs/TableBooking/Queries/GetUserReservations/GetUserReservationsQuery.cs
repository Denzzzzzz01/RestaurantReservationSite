using MediatR;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetUserReservations;

public record GetUserReservationsQuery(Guid UserId, ReservationFilter Filter) : IRequest<List<Reservation>>;
