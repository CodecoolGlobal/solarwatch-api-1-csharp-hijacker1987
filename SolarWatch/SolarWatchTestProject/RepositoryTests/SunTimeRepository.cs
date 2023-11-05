using Microsoft.EntityFrameworkCore;

namespace SunriseSunsetTest.RepositoryTests;
/*
[TestFixture]
public class SunTimeRepositoryTest
{
    private WeatherApiContext _context;
    private ISunTimeRepository _sunTimeRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<WeatherApiContext>()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        _context = new WeatherApiContext(options);
        _context.Database.EnsureCreated();
        _sunTimeRepository = new SunTimeRepository(_context);
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
        _context.SunTimes?.Add(new SunTime(19.02.ToString(), 19.02.ToString()));
        _context.SunTimes?.Add(new SunTime(19.03.ToString(), 19.03.ToString()));
        _context.SaveChanges();
        
        var result = _sunTimeRepository.GetAll();
        
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetByName_ShouldReturnCity_WhenCityExists()
    {
        _context.SunTimes?.Add(new SunTime(19.02.ToString(), 19.02.ToString()) {Id = 1});
        _context.SaveChanges();
        
        var result = _sunTimeRepository.GetByName(1);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(1));
    }

    [Test]
    public void Add_ShouldAddCity_WhenValidCityIsProvided()
    {
        var sunTime = new SunTime(19.02.ToString(), 19.02.ToString()) { Id = 1 };

        _sunTimeRepository.Add(sunTime);
        _context.SaveChanges();
        
        Assert.That( _context.SunTimes?.Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void Update_ShouldAddCity_WhenValidCityIsProvided()
    {
        var sunTime = new SunTime(19.02.ToString(), 19.02.ToString()) { Id = 1 };
        _sunTimeRepository.Add(sunTime);
        _context.SaveChanges();
        var updatedSunTime = new SunTime(19.08.ToString(), 19.07.ToString());

        _sunTimeRepository.Update(updatedSunTime);
        _context.SaveChanges();

        Assert.False(!_context.SunTimes?.Contains(sunTime));
    }

    [Test]
    public void Delete_ShouldRemoveCity_WhenValidCityIsProvided()
    {
        var sunTime = new SunTime(19.02.ToString(), 19.02.ToString()) { Id = 1 };
        _context.SunTimes?.Add(sunTime);
        _context.SaveChanges();
        
        _sunTimeRepository.Delete(sunTime);
        
        Assert.That(_context.Cities?.Count(), Is.EqualTo(0));
    }
}*/