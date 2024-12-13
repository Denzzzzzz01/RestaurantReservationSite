using MediatR;
using RRS.Core.Enums;

namespace RRS.Application.Cqrs.TableBooking.Commands.ChangeReservationStatus;

public record ChangeReservationStatusCommand(Guid ReservationId, ReservationStatus NewStatus, Guid UserId) : IRequest<Guid>;
