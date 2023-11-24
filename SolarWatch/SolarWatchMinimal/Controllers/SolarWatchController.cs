using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatchMinimal.Controllers.Interfaces;
using SolarWatchMinimal.Model;

namespace SolarWatchMinimal.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISolarDataProvider _solar;

    public SolarWatchController(ILogger<SolarWatchController> logger, IJsonProcessor jsonProcessor, ISolarDataProvider solar)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _solar = solar;
    }

    [HttpGet("GetCurrent")]
    public async Task<ActionResult<SolarWatch>> GetCurrent([Required] string name)
    {
        Console.WriteLine(name);
        try
        {
            var weatherData = await _solar.GetCurrentAsync(name);
            var city = _jsonProcessor.Process(weatherData);

            var sunsetSunrise = await _solar.GetCurrentAsync(city.Latitude, city.Longitude);
            var time = _jsonProcessor.SunTimeProcess(sunsetSunrise);

            var newCity = new City(city.Name, city.Longitude, city.Latitude, city.State, city.Country);
            
            var cityId = newCity.Id;

            var newSunTime = new Times(time.SunRiseTime, time.SunSetTime) { CityId = cityId };

            var showCity = new
            {
                Name = city.Name,
                SunRiseTime = time.SunRiseTime,
                SunSetTime = time.SunSetTime
            };
            
            return Ok(showCity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sun data");
            return NotFound("Error getting sun data");
        }
    }
}