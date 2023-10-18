using FlyingDutchmanAirlines.DatabaseLayer;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public BookingRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }
}