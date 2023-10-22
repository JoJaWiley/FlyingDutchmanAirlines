using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber)
    {
        return new Flight();
    }
}