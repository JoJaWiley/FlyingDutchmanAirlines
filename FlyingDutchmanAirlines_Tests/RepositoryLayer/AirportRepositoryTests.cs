using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class AirportRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private AirportRepository _repository;
    
    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;

        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetAirportByID_Failure_InvalidInput()
    {
        StringWriter outputStream = new StringWriter();
        try
        {
            Console.SetOut(outputStream);
            await _repository.GetAirportByID(-1);
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(outputStream.ToString().Contains("Argument Exception in GetAirportByID!" +
                                                           " AirportID = -1"));
            throw new ArgumentException();
        }
        finally
        {
            outputStream.Dispose();
        }
    }

    [TestMethod]
    public async Task GetAirportByID_Success()
    {
        Airport airport = await _repository.GetAirportByID(0);
        Assert.IsNotNull(airport);
    }
}