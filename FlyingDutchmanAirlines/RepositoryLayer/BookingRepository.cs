using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public BookingRepository(FlyingDutchmanAirlinesContext _context)
    {
        this._context = _context;
    }

    public async Task CreateBooking(int customerID, int flightNumber)
    {
        if (customerID < 0 || flightNumber < 0)
        {
            Console.WriteLine($"Argument exception in CreateBooking! CustomerID" +
                              $" = {customerID}, flightNumber = {flightNumber}");
            throw new ArgumentException("Invalid Arguments Provided");
        }

        Booking newBooking = new Booking
        {
            CustomerId = customerID,
            FlightNumber = flightNumber
        };
    }
}