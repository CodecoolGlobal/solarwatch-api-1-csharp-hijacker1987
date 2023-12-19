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

    [HttpGet("GetCityWithSunriseSunsetTimes/{name}")]
    public async Task<IActionResult> GetCityWithSunriseSunsetTimes(string name)
    {
        try
        {
            var weatherData = await _solar.GetCurrentAsync(name);
            var city = _jsonProcessor.Process(weatherData);
            _logger.LogInformation(city.ToString());

            var sunsetSunrise = await _solar.GetCurrentAsync(city.Latitude, city.Longitude);
            var time = _jsonProcessor.SunTimeProcess(sunsetSunrise);
            _logger.LogInformation(sunsetSunrise);

            ViewBag.Name = city.Name;
            ViewBag.SunRiseTime = time.SunRiseTime;
            ViewBag.SunSetTime = time.SunSetTime;

            return View();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sun data");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
