using FlyingDutchmanAirlines.DatabaseLayer;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }
}