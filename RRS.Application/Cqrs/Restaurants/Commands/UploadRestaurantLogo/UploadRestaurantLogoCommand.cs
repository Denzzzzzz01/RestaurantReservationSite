using MediatR;

namespace RRS.Application.Cqrs.Restaurants.Commands.UploadRestaurantLogo;

public record UpdateRestaurantLogoCommand(Guid RestaurantId, string LogoUrl) : IRequest<Unit>;

