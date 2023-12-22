using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SW_MVC.Models;
using SW_MVC.Utility;

namespace SW_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISolarDataPro _solar;

    public HomeController(ISolarDataPro solar, IJsonProcessor jsonProcessor, ILogger<HomeController> logger)
    {
        _solar = solar;
        _jsonProcessor = jsonProcessor;
        _logger = logger;
    }

    [HttpGet("/{name}")]
    public async Task<IActionResult> Index(string name)
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
            
                return View(city);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting sun data");
                return StatusCode(500, "Internal Server Error");
            }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}