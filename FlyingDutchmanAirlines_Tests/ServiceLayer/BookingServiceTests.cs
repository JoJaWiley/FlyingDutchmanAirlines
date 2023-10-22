using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class BookingServiceTests
{
    private FlyingDutchmanAirlinesContext _context;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new
                DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchman").Options;

        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        BookingRepository bookingRepository = new BookingRepository(_context);
        CustomerRepository customerRepository = new CustomerRepository(_context);
        BookingService service = new BookingService(bookingRepository, customerRepository);
        (bool result, Exception exception) =
            await service.CreateBooking("Leo Tolstoy", 0);
    }
}