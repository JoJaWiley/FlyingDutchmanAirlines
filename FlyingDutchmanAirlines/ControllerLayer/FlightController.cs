using System.Net;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

public class FlightController : Controller
{
    private readonly FlightService _service;

    public FlightController(FlightService service)
    {
        _service = service;
    }
    public async Task<IActionResult> GetFlights()
    {
        try
        {
            Queue<FlightView> flights = new Queue<FlightView>();
            await foreach (FlightView flight in _service.GetFlights())
            {
                flights.Enqueue(flight);
            }

            return StatusCode((int)HttpStatusCode.OK, flights);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int)HttpStatusCode.NotFound,
                "No flights were found in the database");
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "An error occurred");
        }
    }

    public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            FlightView flight = await _service.GetFlightByFlightNumber(flightNumber);
            return StatusCode((int)HttpStatusCode.OK, flight);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int)HttpStatusCode.NotFound,
                "The flight was not found in the database");
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
        }
    }
}