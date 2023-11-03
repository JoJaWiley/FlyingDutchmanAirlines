using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class FlightService
{
    private readonly FlightRepository _flightRepository;
    private readonly AirportRepository _airportRepository;

    public FlightService(FlightRepository flightRepository, AirportRepository airportRepository)
    {
        _flightRepository = flightRepository;
        _airportRepository = airportRepository;
    }

    public async IAsyncEnumerable<FlightView> GetFlights()
    {
        Queue<Flight> flights = _flightRepository.GetFlights();
        foreach (Flight flight in flights)
        {
            Airport originAirport =
                await _airportRepository.GetAirportByID(flight.Origin);
            Airport destinationAirport =
                await _airportRepository.GetAirportByID(flight.Destination);
            
            yield return new FlightView(flight.FlightNumber.ToString(),
                    (originAirport.City, originAirport.Iata),
                    (destinationAirport.City, destinationAirport.Iata));
        }
    }
}