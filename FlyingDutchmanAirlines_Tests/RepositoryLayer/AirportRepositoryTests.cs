﻿using System.Collections;
using FlyingDutchmanAirlines_Tests.Stubs;
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
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;

        _context = new FlyingDutchmanAirlinesContext_Stub2(dbContextOptions);

        SortedList<string, Airport> airports = new SortedList<string, Airport>
        {
            {
                "GOH",
                new Airport
                {
                    AirportId = 0,
                    City = "Nuuk",
                    Iata = "GOH"
                }
            },
            {
                "PHX",
                new Airport
                {
                    AirportId = 1,
                    City = "Phoenix",
                    Iata = "PHX"
                }
            },
            {
                "DDH",
                new Airport
                {
                    AirportId = 2,
                    City = "Bennington",
                    Iata = "DDH"
                }
            },
            { 
                "RDU",
                new Airport
                {
                    AirportId = 3,
                    City = "Raleigh-Durham",
                    Iata = "RDU"
                }
            }
        };

        _context.Airports.AddRange(airports.Values);
        await _context.SaveChangesAsync();
        
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
        Assert.AreEqual(0, airport.AirportId);
        Assert.AreEqual("Nuuk", airport.City);
        Assert.AreEqual("GOH", airport.Iata);
    }
}