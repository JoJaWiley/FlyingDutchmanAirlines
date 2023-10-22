using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }

    public async Task<Airport> GetAirportByID(int airportID)
    {
        if (airportID < 0)
        {
            Console.WriteLine($"Argument Exception in GetAirportByID! AirportID = {airportID}");
            throw new ArgumentException("invalid argument provided");
        }
        return new Airport();
    }
}