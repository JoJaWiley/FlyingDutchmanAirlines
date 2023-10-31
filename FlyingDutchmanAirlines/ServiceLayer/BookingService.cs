using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly FlightRepository _flightRepository;
    private readonly CustomerRepository _customerRepository;

    public BookingService(BookingRepository bookingRepository,
        FlightRepository flightRepository, CustomerRepository customerRepository)
    {
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
        _customerRepository = customerRepository;
    }

    public async Task<(bool, Exception?)> CreateBooking(string customerName, int flightNumber)
    {
        if (string.IsNullOrEmpty(customerName) || !flightNumber.IsPositive())
        {
            return (false, new ArgumentException());
        }
        try
        {
            Customer customer;
            try
            {
                if (!await FlightExistsInDatabase(flightNumber))
                {
                    throw new CouldNotAddBookingToDatabaseException();
                }
                customer =
                    await _customerRepository.GetCustomerByName(customerName);
            }
            catch (CustomerNotFoundException)
            {
                await _customerRepository.CreateCustomer(customerName);
                return await CreateBooking(customerName, flightNumber);
            }

            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null);
        }
        catch(Exception exception)
        {
            return (false, exception);
        }
    }

    private async Task<bool> FlightExistsInDatabase(int flightNumber)
    {
        try
        {
            return await
                _flightRepository.GetFlightByFlightNumber(flightNumber) != null;
        }
        catch (FlightNotFoundException)
        {
            return false;
        }
    }
}