using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly UsersContext _context;
    private readonly ISolarDataProvider _solar;

    public SolarWatchController(ILogger<SolarWatchController> logger, UsersContext context, IJsonProcessor jsonProcessor, ISolarDataProvider solar)
    {
        _logger = logger;
        _context = context;
        _jsonProcessor = jsonProcessor;
        _solar = solar;
    }

    [HttpGet("Get"), Authorize(Roles="User")]
    public async Task<ActionResult<SunriseSunsetTimes>> GetSunTime(string name)
    {
        try
        {
            var existingCity = await _context.Cities!.FirstOrDefaultAsync(c => c.Name == name);
            if (existingCity == null)
            {
                _logger.LogInformation($"Data for {name} not exists in the database.");
                return Ok(existingCity);
            }
            
            var existingSunTime = await _context.Times!.FirstOrDefaultAsync(sunTime => sunTime.CityId == existingCity.Id);
            if (existingSunTime == null)
            {
                return Ok(existingCity);
            }
            
            var result = new {
                Name = existingCity.Name,
                SunRiseTime = existingSunTime.SunRiseTime,
                SunSetTime = existingSunTime.SunSetTime
            };

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sun data");
            return NotFound("Error getting sun data");
        }
    }

    [HttpGet("GetCurrent"), Authorize(Roles="User, Admin")]
    public async Task<ActionResult<Model.SolarWatch>> GetCurrent([Required] string name)
    {
        Console.WriteLine(name);
        try
        {
            var weatherData = await _solar.GetCurrentAsync(name);
            var city = _jsonProcessor.Process(weatherData);

            var sunsetSunrise = await _solar.GetCurrentAsync(city.Latitude, city.Longitude);
            var time = _jsonProcessor.SunTimeProcess(sunsetSunrise);

            var newCity = new City(city.Name, city.Longitude, city.Latitude, city.State, city.Country);
            
            _context.Cities?.Add(newCity);
            await _context.SaveChangesAsync();
            
            var cityId = newCity.Id;

            var newSunTime = new SunriseSunsetTimes(time.SunRiseTime, time.SunSetTime) { CityId = cityId };
            
            _context.Times!.Add(newSunTime);
            await _context.SaveChangesAsync();

            var showCity = new
                {
                    Name = city.Name,
                    SunRiseTime = time.SunRiseTime,
                    SunSetTime = time.SunSetTime
                };

            //return Ok($"Name: {city.Name} SunRiseTime: {time.SunRiseTime}, SunSetTime: {time.SunSetTime}");
            return Ok(showCity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sun data");
            return NotFound("Error getting sun data");
        }
    }

    [HttpGet("GetCityWithSunriseSunsetTimes/{id}"), Authorize(Roles = "User")]
    public async Task<IActionResult> GetCityWithSunriseSunsetTimes(int id)
    {
        var existingCity = await _context.Cities!.FirstOrDefaultAsync(city => city.Id == id);
    
        if (existingCity == null)
        {
            return NotFound("City not found");
        }

        var existingSunTime = await _context.Times!.FirstOrDefaultAsync(sunTime => sunTime.CityId == existingCity.Id);

        if (existingSunTime == null)
        {
            return NotFound("SunTime not found for the city");
        }

        var showCity = new
        {
            Name = existingCity.Name,
            SunRiseTime = existingSunTime.SunRiseTime,
            SunSetTime = existingSunTime.SunSetTime
        };

        //return Ok($"Name: {existingCity.Name} SunRiseTime: {existingSunTime?.SunRiseTime}, SunSetTime: {existingSunTime?.SunSetTime}");
        return Ok(showCity);
    }
}