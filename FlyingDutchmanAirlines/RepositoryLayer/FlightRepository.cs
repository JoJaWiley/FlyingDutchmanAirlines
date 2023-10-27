using System.Reflection;
using System.Runtime.CompilerServices;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public FlightRepository()
    {
        if (Assembly.GetExecutingAssembly().FullName ==
            Assembly.GetCallingAssembly().FullName)
        {
            throw new Exception("This constructor should only be used for testing");
        }
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAirportId, 
        int destinationAirportId)
    {
        if (!originAirportId.IsPositive() || !destinationAirportId.IsPositive())
        {
            Console.WriteLine($"Argument Exception in GetFlightByFlightNumber!" +
                              $"originAirportId = {originAirportId} : " +
                              $"destinationAirportId = {destinationAirportId}");
            throw new ArgumentException("invalid arguments provided");
        }

        if (!flightNumber.IsPositive())
        {
            Console.WriteLine($"Could not find flight in GetFlightByFlightNumber!" +
                              $" flightNumber = {flightNumber}");
            throw new FlightNotFoundException();
        }

        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber)
               ?? throw new FlightNotFoundException();
    }
}