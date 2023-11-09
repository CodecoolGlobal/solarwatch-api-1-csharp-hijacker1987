using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatchMvp.Controllers;

[ApiController]
[Route("[controller]")]
public class CrudAdminController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ISolarDataProvider _weatherDataProvider;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly CityApiContext _repository;

    public CrudAdminController(ILogger<SolarWatchController> logger, ISolarDataProvider weatherDataProvider, IJsonProcessor jsonProcessor, CityApiContext cityApiContext)
    {
        _logger = logger;
        _weatherDataProvider = weatherDataProvider;
        _jsonProcessor = jsonProcessor;
        _repository = cityApiContext;
    }

    [HttpPut("Put"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunriseSunsetTimes>> Put(
        int id,
        string incomingCityName,
        double incomingCityLongitude,
        double incomingCityLatitude,
        string incomingCityState,
        string incomingCityCountry,
        string incomingSunRiseTime,
        string incomingSunSetTime
        )
    {
        try
        {
            var existingCity = await _repository.Cities!.FirstOrDefaultAsync(city => city.Id == id);
            if (existingCity == null)
            {
                _logger.LogInformation($"Data for id: {id} doesn't exists in the database.");
                return Ok(existingCity);
            }
            else
            {
                existingCity.ChangeCityData(incomingCityName,
                    incomingCityLongitude,
                    incomingCityLatitude,
                    incomingCityState,
                    incomingCityCountry);
            }
            
            var existingSunTime = await _repository.Times!.FirstOrDefaultAsync(sunTime => sunTime.CityId == existingCity.Id);
            if (existingSunTime == null)
            {
                return Ok(existingSunTime);
            }
            else
            {
                existingSunTime.ChangeSunTimeData(incomingSunRiseTime, incomingSunSetTime);
            }
            
            await _repository.SaveChangesAsync();
            
            return Ok($"City with id: {id} successfully updated!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("Delete"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunriseSunsetTimes>> Delete(int id)
    {
        try
        {
            var existingCity = await _repository.Cities!.FirstOrDefaultAsync(city => city.Id == id);
            if (existingCity == null)
            {
                _logger.LogInformation($"Data for id: {id} doesnt't exists in the database.");
                return Ok(existingCity);
            }
            
            var existingSunTime = await _repository.Times!.FirstOrDefaultAsync(sunTime => sunTime.CityId == existingCity.Id);
            if (existingSunTime == null)
            {
                return Ok(existingCity);
            }

            _repository.Cities!.Remove(existingCity);
            _repository.Times!.Remove(existingSunTime);
            await _repository.SaveChangesAsync();

            return Ok("Successful!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("Post"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunriseSunsetTimes>> Post(string name)
    {
        try
        {
            var existingCity = await _repository.Cities.FirstOrDefaultAsync(c => c.Name == name);
            if (existingCity != null)
            {
                _logger.LogInformation($"Data for {name} already exists in the database.");
                return Ok(existingCity);
            }

            var weatherData = await _weatherDataProvider.GetCurrentAsync(name);
            var city = _jsonProcessor.Process(weatherData);

            var sunsetSunrise = await _weatherDataProvider.GetCurrentAsync(city.Latitude, city.Longitude);
            var time = _jsonProcessor.SunTimeProcess(sunsetSunrise);

            var newCity = new City(city.Name, city.Longitude, city.Latitude, city.State, city.Country);
            
            _repository.Cities.Add(newCity);
            await _repository.SaveChangesAsync();
            
            var cityId = newCity.Id;

            var newSunTime = new SunriseSunsetTimes(time.SunRiseTime, time.SunSetTime) { CityId = cityId };
            
            _repository.Times!.Add(newSunTime);
            await _repository.SaveChangesAsync();

            return Ok(time);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sun data");
            return NotFound("Error getting sun data");
        }
    }
}
