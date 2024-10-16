using MediatR;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Commands.ChangeReservationStatus
{
    public record ChangeReservationStatusCommand(Guid ReservationId, ReservationStatus NewStatus, Guid UserId) : IRequest<Guid>;

}
