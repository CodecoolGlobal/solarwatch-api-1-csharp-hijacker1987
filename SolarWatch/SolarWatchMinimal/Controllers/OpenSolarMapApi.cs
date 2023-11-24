using SolarWatchMinimal.Controllers.Interfaces;

namespace SolarWatchMinimal.Controllers;

public class OpenSolarMapApi : ISolarDataProvider
{
    private readonly ILogger<OpenSolarMapApi> _logger;
    private readonly IConfiguration _configuration;
    
    public OpenSolarMapApi(ILogger<OpenSolarMapApi> logger, IConfiguration config)
    {
        _logger = logger;
        _configuration = config;
    }
    
    public async Task<string> GetCurrentAsync(double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> GetCurrentAsync(string cityName)
    {
        var api = _configuration["Api:ServiceApiKey"];
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={api}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling Geocoding API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}