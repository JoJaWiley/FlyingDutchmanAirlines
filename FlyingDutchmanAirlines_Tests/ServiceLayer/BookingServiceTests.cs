using FlyingDutchmanAirlines_Tests.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class BookingServiceTests
{
    [TestInitialize]
    public async Task TestInitialize()
    {
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        Mock<BookingRepository> mockBookingRepository = new Mock<BookingRepository>();
        Mock<CustomerRepository> mockCustomerRepository = new Mock<CustomerRepository>();
        
        mockBookingRepository.Setup(repository =>
            repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
        mockCustomerRepository.Setup(repository =>
                repository.GetCustomerByName("Leo Tolstoy"))
                    .Returns(Task.FromResult(new Customer("Leo Tolstoy")));
        
        BookingService service = new BookingService(mockBookingRepository.Object, 
            mockCustomerRepository.Object);
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
        Mock<BookingRepository> mockBookingRepository = new Mock<BookingRepository>();
        Mock<CustomerRepository> mockCustomerRepository = new Mock<CustomerRepository>();

        BookingService service =
            new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object);

        (bool result, Exception? exception) =
            await service.CreateBooking(name, flightNumber);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public async Task? CreateBooking_Failure_RepositoryException()
    {
        Mock<BookingRepository> mockBookingRepository = new Mock<BookingRepository>();
        Mock<CustomerRepository> mockCustomerRepository = new Mock<CustomerRepository>();

        mockBookingRepository.Setup(repository =>
            repository.CreateBooking(0, 1)).Throws(new ArgumentException());
        mockBookingRepository.Setup(repository =>
            repository.CreateBooking(1, 2)).Throws(new CouldNotAddBookingToDatabaseException());

        mockCustomerRepository.Setup(repository =>
                repository.GetCustomerByName("Galileo Galilei"))
            .Returns(Task.FromResult(new Customer("Galileo Galilei") { CustomerId = 0 }));
        mockCustomerRepository.Setup(repository =>
                repository.GetCustomerByName("Eise Eisinga"))
            .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));

        BookingService service = new BookingService(mockBookingRepository.Object,
            mockCustomerRepository.Object);
        (bool result, Exception? exception) = await service.CreateBooking("Galileo Galilei", 1);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(ArgumentException));

        (result, exception) = await service.CreateBooking("Eise Eisinga", 2);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));


    }
}