using System.Net;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

[TestClass]
public class FlightControllerTests
{
    [TestMethod]
    public async Task GetFlights_Success()
    {
        Mock<FlightService> service = new Mock<FlightService>();

        List<FlightView> returnFlightViews = new List<FlightView>(2)
        {
            new FlightView("1932",
                ("Groningen", "GRQ"), ("Phoenix", "PHX")),
            new FlightView("841",
                ("New York City", "JFK"), ("London", "LHR"))
        };

        service.Setup(s =>
            s.GetFlights()).Returns(FlightViewAsyncGenerator(returnFlightViews));

        FlightController controller = new FlightController(service.Object);
        ObjectResult? response =  
            await controller.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

        Queue<FlightView>? content = response.Value as Queue<FlightView>;
        Assert.IsNotNull(content);
        
        Assert.IsTrue(returnFlightViews.All(flight =>  
            content.Contains(flight)));
    }

    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(IEnumerable<FlightView> views)
    {
        foreach (FlightView flightView in views)
        {
            yield return flightView;
        }
    }
}