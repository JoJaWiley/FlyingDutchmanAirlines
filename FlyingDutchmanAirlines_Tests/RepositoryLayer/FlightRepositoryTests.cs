using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        Flight flight = new Flight
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 2
        };

        Flight flight2 = new Flight
        {
            FlightNumber = 10,
            Origin = 3,
            Destination = 4
        };

        _context.Flights.Add(flight);
        _context.Flights.Add(flight2);
        await _context.SaveChangesAsync();
        
        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        Flight flight = await _repository.GetFlightByFlightNumber(1);
        Assert.IsNotNull(flight);

        Flight dbFlight = _context.Flights.First(f => f.FlightNumber == 1);
        Assert.IsNotNull(dbFlight);
        
        Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
        Assert.AreEqual(dbFlight.Origin, flight.Origin);
        Assert.AreEqual(dbFlight.Destination, flight.Destination);
    }
    
    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber()
    {
        await _repository.GetFlightByFlightNumber(-1);
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_DatabaseException()
    {
        await _repository.GetFlightByFlightNumber(2);
    }

    [TestMethod]
    public void GetFlights_Success()
    {
        Queue<Flight> flights = _repository.GetFlights();
        Assert.IsNotNull(flights);

        Flight flight1 = flights.Dequeue();
        Flight flight2 = flights.Dequeue();
        Assert.IsNotNull(flight1);
        Assert.IsNotNull(flight2);

        Flight dbFlight1 = _context.Flights.First(f => f.FlightNumber == 1);
        Flight dbFlight2 = _context.Flights.First(f => f.FlightNumber == 10);
        Assert.IsNotNull(dbFlight1);
        Assert.IsNotNull(dbFlight2);
        
        Assert.AreEqual(dbFlight1.FlightNumber, flight1.FlightNumber);
        Assert.AreEqual(dbFlight1.Origin, flight1.Origin);
        Assert.AreEqual(dbFlight1.Destination, flight1.Destination);
        
        Assert.AreEqual(dbFlight2.FlightNumber, flight2.FlightNumber);
        Assert.AreEqual(dbFlight2.Origin, flight2.Origin);
        Assert.AreEqual(dbFlight2.Destination, flight2.Destination);
    }
    
    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public void GetFlights_Failure_FlightsNotFound()
    {
        _context.Database.EnsureDeleted();
        _repository.GetFlights();
    }
}