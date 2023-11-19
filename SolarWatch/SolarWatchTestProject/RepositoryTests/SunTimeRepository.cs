using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Repository;

namespace SolarWatchTestProject.RepositoryTests;

[TestFixture]
public class SunTimeRepositoryTest
{
    private UsersContext _context;
    private ISunTimeRepository _sunTimeRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<UsersContext>()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        _context = new UsersContext(options);
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
        _context.Times?.Add(new SunriseSunsetTimes(19.02.ToString(CultureInfo.InvariantCulture), 19.02.ToString(CultureInfo.InvariantCulture)));
        _context.Times?.Add(new SunriseSunsetTimes(19.03.ToString(CultureInfo.InvariantCulture), 19.03.ToString(CultureInfo.InvariantCulture)));
        _context.SaveChanges();
        
        var result = _sunTimeRepository.GetAll();
        
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetByName_ShouldReturnCity_WhenCityExists()
    {
        _context.Times?.Add(new SunriseSunsetTimes(19.02.ToString(CultureInfo.InvariantCulture), 19.02.ToString(CultureInfo.InvariantCulture)) {Id = 1});
        _context.SaveChanges();
        
        var result = _sunTimeRepository.GetByName(1);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(1));
    }

    [Test]
    public void Add_ShouldAddCity_WhenValidCityIsProvided()
    {
        var sunTime = new SunriseSunsetTimes(19.02.ToString(CultureInfo.InvariantCulture), 19.02.ToString(CultureInfo.InvariantCulture)) { Id = 1 };

        _sunTimeRepository.Add(sunTime);
        _context.SaveChanges();
        
        Assert.That( _context.Times?.Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void Update_ShouldAddCity_WhenValidCityIsProvided()
    {
        var sunTime = new SunriseSunsetTimes(19.02.ToString(CultureInfo.InvariantCulture), 19.02.ToString(CultureInfo.InvariantCulture)) { Id = 1 };
        _sunTimeRepository.Add(sunTime);
        _context.SaveChanges();
        var updatedSunTime = new SunriseSunsetTimes(19.08.ToString(CultureInfo.InvariantCulture), 19.07.ToString(CultureInfo.InvariantCulture));

        _sunTimeRepository.Update(updatedSunTime);
        _context.SaveChanges();

        Assert.False(!_context.Times?.Contains(sunTime));
    }

    [Test]
    public void Delete_ShouldRemoveCity_WhenValidCityIsProvided()
    {
        var sunTime = new SunriseSunsetTimes(19.02.ToString(CultureInfo.InvariantCulture), 19.02.ToString(CultureInfo.InvariantCulture)) { Id = 1 };
        _context.Times?.Add(sunTime);
        _context.SaveChanges();
        
        _sunTimeRepository.Delete(sunTime);
        
        Assert.That(_context.Cities?.Count(), Is.EqualTo(0));
    }
}