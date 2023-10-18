using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class BookingRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private BookingRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = 
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        _repository = new BookingRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public void CreateBooking_Success()
    {
        
    }
}