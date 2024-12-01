using MediatR;

namespace RRS.Application.Cqrs.TableBooking.Commands.CancelReservation;

public record CancelReservationCommand(Guid ReservationId, Guid UserId) : IRequest<Unit>;
