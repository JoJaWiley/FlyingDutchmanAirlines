using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;
using FlyingDutchmanAirlines.DatabaseLayer;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class CustomerRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new
                DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
            .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
    }
    
    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        CustomerRepository repository = new CustomerRepository(_context);
        Assert.IsNotNull(repository);

        bool result = await repository.CreateCustomer("Donald Knuth");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsNull()
    {
        CustomerRepository repository = new CustomerRepository(_context);
        Assert.IsNotNull(repository);

        bool result = await repository.CreateCustomer(null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsEmptyString()
    {
        CustomerRepository repository = new CustomerRepository(_context);
        Assert.IsNotNull(repository);

        bool result = await repository.CreateCustomer("");
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('!')]
    [DataRow('@')]
    [DataRow('#')]
    [DataRow('$')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public async Task CreateCustomer_Failure_NameContainsInvalidCharacters(char invalidCharacter)
    {
        CustomerRepository repository = new CustomerRepository(_context);
        Assert.IsNotNull(repository);

        bool result = await repository.CreateCustomer("Donald Knuth" + invalidCharacter);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_DatabaseAccessError()
    {
        CustomerRepository repository = new CustomerRepository(null);
        Assert.IsNotNull(repository);

        bool result = await repository.CreateCustomer("Donald Knuth");
        Assert.IsFalse(result);
    }
    
}