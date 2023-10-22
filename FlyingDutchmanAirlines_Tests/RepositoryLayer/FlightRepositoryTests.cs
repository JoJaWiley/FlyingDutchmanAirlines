using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class FlightRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private FlightRepository _repository;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;

        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }
}