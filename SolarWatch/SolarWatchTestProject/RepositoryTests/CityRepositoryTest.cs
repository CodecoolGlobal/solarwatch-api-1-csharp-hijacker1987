using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Repository;
using SolarWatchMvp.Repository;

namespace SunriseSunsetTest.RepositoryTests;

[TestFixture]
public class CityRepositoryTests
{
    private CityApiContext _context;
    private ICityRepository _cityRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CityApiContext>()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        _context = new CityApiContext(options);
        _context.Database.EnsureCreated();
        _cityRepository = new CityRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void GetAll_ShouldReturnAllCities()
    {
        _context.Cities?.Add(new City("Budapest", 19.02, 19.02, "-", "HU"));
        _context.Cities?.Add(new City("Budapest2", 19.022, 19.022, "-", "HU"));
        _context.SaveChanges();
        
        var result = _cityRepository.GetAll();
        
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetByName_ShouldReturnCity_WhenCityExists()
    {
        _context.Cities?.Add(new City("Budapest", 19.02, 19.02, "-", "HU"));
        _context.SaveChanges();
        
        var result = _cityRepository.GetByName("Budapest");
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Name, Is.EqualTo("Budapest"));
    }

    [Test]
    public void Add_ShouldAddCity_WhenValidCityIsProvided()
    {
        var city = new City("Budapest", 19.02, 19.02, "-", "HU");

        _cityRepository.Add(city);
        _context.SaveChanges();
        
        Assert.That( _context.Cities?.Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void Update_ShouldAddCity_WhenValidCityIsProvided()
    {
        var city = new City("Budapest", 19.02, 19.02, "-", "HU");
        _cityRepository.Add(city);
        _context.SaveChanges();
        var updatedCity = new City("Test", 19.02, 19.02, "-", "HU");

        _cityRepository.Update(updatedCity);
        _context.SaveChanges();

        Assert.False(!_context.Cities?.Contains(city));
    }

    [Test]
    public void Delete_ShouldRemoveCity_WhenValidCityIsProvided()
    {
        var city = new City("Budapest", 19.02, 19.02, "-", "HU");
        _context.Cities?.Add(city);
        _context.SaveChanges();
        
        _cityRepository.Delete(city);
        
        Assert.That(_context.Cities?.Count(), Is.EqualTo(0));
    }
}