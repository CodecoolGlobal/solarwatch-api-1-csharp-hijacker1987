using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly CityApiContext _context;
    private readonly ISolarDataProvider _solar;

    public SolarWatchController(ILogger<SolarWatchController> logger, CityApiContext context, IJsonProcessor jsonProcessor, ISolarDataProvider solar)
    {
        _logger = logger;
        _context = context;
        _jsonProcessor = jsonProcessor;
        _solar = solar;
    }


    [HttpGet("GetCurrent")]
    public async Task<ActionResult<SolarWatch>> GetCurrent([Required] string name)
    {
        try
        {
            var weatherData = await _solar.GetCurrentAsync(name);
            var city = _jsonProcessor.Process(weatherData);

            var sunsetSunrise = await _solar.GetCurrentAsync(city.Latitude, city.Longitude);
            var time = _jsonProcessor.SunTimeProcess(sunsetSunrise);

            var newCity = new City(city.Name, city.Longitude, city.Latitude, city.State, city.Country);
            
            _context.Cities.Add(newCity);
            await _context.SaveChangesAsync();
            
            var cityId = newCity.Id;

            var newSunTime = new SunriseSunsetTimes(time.SunRiseTime, time.SunSetTime) { CityId = cityId };
            
            _context.Times!.Add(newSunTime);
            await _context.SaveChangesAsync();

            return Ok($"Name: {city.Name} SunRiseTime: {time.SunRiseTime}, SunSetTime: {time.SunSetTime}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sun data");
            return NotFound("Error getting sun data");
        }
    }

/*
[HttpGet("GetCurrent")]
public ActionResult<SolarWatch> GetCurrent([Required]string city)
{
    try
    {
        var cityData = _solarDataProvider.GetCurrent(city);
        var availableCityData = _jsonProcessor.Process(cityData, true);

        var solarData = _solarDataProvider.GetCurrent(availableCityData.Latitude, availableCityData.Longitude);
        var availableSolarData = _jsonProcessor.Process(solarData, false);

        return new SolarWatch
        {
            City = availableCityData.City,
            Latitude = availableCityData.Latitude,
            Longitude = availableCityData.Longitude,
            Sunrise = availableSolarData.Sunrise,
            Sunset = availableSolarData.Sunset
        };
    }
    catch (Exception e)
    {
        _logger.LogError(e, "Error getting solar data");
        return NotFound("Error getting solar data");
    }
}
*/
}