using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class BookingServiceTests
{
    private Mock<BookingRepository> _mockBookingRepository;
    private Mock<FlightRepository> _mockFlightRepository;
    private Mock<CustomerRepository> _mockCustomerRepository;
    
    [TestInitialize]
    public async Task TestInitialize()
    {
        _mockBookingRepository = new Mock<BookingRepository>();
        _mockFlightRepository = new Mock<FlightRepository>();
        _mockCustomerRepository = new Mock<CustomerRepository>();
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        _mockBookingRepository.Setup(repository =>
            repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);

        _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(0))
            .ReturnsAsync(new Flight());
        
        _mockCustomerRepository.Setup(repository =>
                repository.GetCustomerByName("Leo Tolstoy"))
                    .Returns(Task.FromResult(new Customer("Leo Tolstoy")));
        
        BookingService service = new BookingService(_mockBookingRepository.Object, 
            _mockFlightRepository.Object, _mockCustomerRepository.Object);
        (bool result, Exception? exception) =
            await service.CreateBooking("Leo Tolstoy", 0);
        
        Assert.IsTrue(result);
        Assert.IsNull(exception);
    }

    [TestMethod]
    [DataRow("", 0)]
    [DataRow(null, -1)]
    [DataRow("Galileo Galilei", -1)]
    public async Task CreateBooking_Failure_InvalidInputArguments(string name, int flightNumber)
    {
        BookingService service =
            new BookingService(_mockBookingRepository.Object, 
                _mockFlightRepository.Object, _mockCustomerRepository.Object);

        (bool result, Exception? exception) =
            await service.CreateBooking(name, flightNumber);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public async Task? CreateBooking_Failure_RepositoryException_ArgumentException()
    {
        _mockBookingRepository.Setup(repository =>
            repository.CreateBooking(0, 1)).Throws(new ArgumentException());

        _mockFlightRepository.Setup(repository =>
            repository.GetFlightByFlightNumber(1)).ReturnsAsync(new Flight());
        

        _mockCustomerRepository.Setup(repository =>
                repository.GetCustomerByName("Galileo Galilei"))
            .Returns(Task.FromResult(new Customer("Galileo Galilei") { CustomerId = 0 }));

        BookingService service = new BookingService(_mockBookingRepository.Object,
           _mockFlightRepository.Object, _mockCustomerRepository.Object);
        
        (bool result, Exception? exception) = await service.CreateBooking("Galileo Galilei", 1);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(ArgumentException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_RepositoryException_CouldNotAddBookingToDatabase()
    {
        _mockBookingRepository.Setup(repository =>
            repository.CreateBooking(1, 2)).Throws(new CouldNotAddBookingToDatabaseException());

        _mockFlightRepository.Setup(repository =>
            repository.GetFlightByFlightNumber(2)).ReturnsAsync(new Flight());
        
        _mockCustomerRepository.Setup(repository =>
                repository.GetCustomerByName("Eise Eisinga"))
            .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));
        
        BookingService service = new BookingService(_mockBookingRepository.Object,
           _mockFlightRepository.Object, _mockCustomerRepository.Object);
        
        (bool result, Exception? exception) = await service.CreateBooking("Eise Eisinga", 2);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_FlightNotInDatabase()
    {
        _mockFlightRepository.Setup(repository =>
                repository.GetFlightByFlightNumber(-1))
            .Throws(new FlightNotFoundException());
        
        BookingService service = new BookingService(_mockBookingRepository.Object,
            _mockFlightRepository.Object, _mockCustomerRepository.Object);
        (bool result, Exception? exception) =
            await service.CreateBooking("Maurits Escher", 19);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }
}