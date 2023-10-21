using FlyingDutchmanAirlines.DatabaseLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.Stubs;

public class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
{
    public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options)
        : base(options)
    { }
    
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.Bookings.First().CustomerId switch
        {
            1 => await base.SaveChangesAsync(cancellationToken),
            _ => throw new Exception("Database Error!"),
        };
    }
}