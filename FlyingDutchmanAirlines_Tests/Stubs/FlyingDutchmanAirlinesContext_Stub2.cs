using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlyingDutchmanAirlines_Tests.Stubs;

public class FlyingDutchmanAirlinesContext_Stub2 : FlyingDutchmanAirlinesContext
{
    public FlyingDutchmanAirlinesContext_Stub2(DbContextOptions<FlyingDutchmanAirlinesContext> options)
        : base(options)
    {
        base.Database.EnsureDeleted();
    }
    
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);

        IEnumerable<Airport> airports = pendingChanges
            .Select(e => e.Entity).OfType<Airport>();
        if (!airports.Any())
        {
            throw new Exception("Database Error!");
        }

        await base.SaveChangesAsync(cancellationToken);
        return 1;
    }
}