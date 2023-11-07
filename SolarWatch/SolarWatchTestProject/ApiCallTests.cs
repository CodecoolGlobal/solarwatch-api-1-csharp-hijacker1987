using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;

namespace SunriseSunsetTest;

[TestFixture]
public class WeatherDataProviderTests
{
    private ILogger<OpenSolarMapApi> _logger;
    private ISolarDataProvider _weatherDataProvider;
    private IConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<OpenSolarMapApi>>().Object;
        _weatherDataProvider = new OpenSolarMapApi(_logger, _configuration);
    }

    [Test]
    public async Task GetCoordinates_ValidCityName_ReturnsNonEmptyString()
    {
        const string cityName = "New York";
        
        var result = await _weatherDataProvider.GetCurrentAsync(cityName);
        
        Assert.That(result, Is.Not.Empty, "Result should not be empty.");
    }
    
    [Test]
    public async Task GetSunTime_ValidCityName_ReturnsNonEmptyString()
    {
        const double lat = 12.1;
        const double lon = 12.1;
        
        var result = await _weatherDataProvider.GetCurrentAsync(lat, lon);
        
        Assert.That(result, Is.Not.Empty, "Result should not be empty.");
    }
}