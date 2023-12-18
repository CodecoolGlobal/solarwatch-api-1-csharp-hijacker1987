using Microsoft.AspNetCore.Mvc;
using SW_MVC.Utility;

namespace SW_MVC.Controllers;

public class SWController : Controller
{
    private readonly ILogger<SWController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISolarDataPro _solar;

    public SWController(ISolarDataPro solar, IJsonProcessor jsonProcessor, ILogger<SWController> logger)
    {
        _solar = solar;
        _jsonProcessor = jsonProcessor;
        _logger = logger;
    }

    [HttpGet("GetCityWithSunriseSunsetTimes/{lat}-{lon}")]
    public async Task<IActionResult> GetCityWithSunriseSunsetTimes(double lat, double lon)
    {
        try
        {
            var existingCity = await _solar.GetCurrentAsync(lat, lon);
            var weatherData = await _solar.GetCurrentAsync(existingCity);
            var city = _jsonProcessor.Process(weatherData);

            var sunsetSunrise = await _solar.GetCurrentAsync(city.Latitude, city.Longitude);
            var time = _jsonProcessor.SunTimeProcess(sunsetSunrise);

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
            return StatusCode(500, "Internal Server Error");
        }
    }

}