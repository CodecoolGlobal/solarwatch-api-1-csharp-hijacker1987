using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ISolarDataProvider _solarDataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarWatchController(ILogger<SolarWatchController> logger, ISolarDataProvider solarDataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _solarDataProvider = solarDataProvider;
        _jsonProcessor = jsonProcessor;
    }
    
    [HttpGet("GetCurrent")]
    public async Task<ActionResult<SolarWatch>> GetCurrent([Required]string city)
    {
        try
        {
            var cityData = await _solarDataProvider.GetCurrentAsync(city);
            var availableCityData = _jsonProcessor.Process(cityData, true);

            var solarData = await _solarDataProvider.GetCurrentAsync(availableCityData.Latitude, availableCityData.Longitude);
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