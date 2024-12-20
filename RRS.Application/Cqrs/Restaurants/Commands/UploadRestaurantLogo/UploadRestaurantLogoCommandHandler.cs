using MediatR;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurants.Commands.UploadRestaurantLogo;

public class UpdateRestaurantLogoCommandHandler : IRequestHandler<UpdateRestaurantLogoCommand, Unit>
{
    private readonly IAppDbContext _dbContext;

    public UpdateRestaurantLogoCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants.FindAsync(request.RestaurantId, cancellationToken);
            
        if (restaurant is null)
            throw new InvalidOperationException("Restaurant not found.");
        

        restaurant.LogoUrl = request.LogoUrl;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
