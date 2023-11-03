using System.Reflection;
using System.Runtime.CompilerServices;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public AirportRepository()
    {
        if (Assembly.GetExecutingAssembly().FullName ==
            Assembly.GetCallingAssembly().FullName)
        {
            throw new Exception("This constructor should only be used for testing");
        }
    }

    public virtual async Task<Airport> GetAirportByID(int airportID)
    {
        if (!airportID.IsPositive())
        {
            Console.WriteLine($"Argument Exception in GetAirportByID! AirportID = {airportID}");
            throw new ArgumentException("invalid argument provided");
        }
        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportID)
        ?? throw new AirportNotFoundException();
    }
    
    
}