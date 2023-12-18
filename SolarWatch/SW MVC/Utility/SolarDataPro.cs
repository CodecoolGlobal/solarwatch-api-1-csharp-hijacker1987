namespace SW_MVC.Utility;

public class SolarDataPro : ISolarDataPro
{ 
    public async Task<string> GetCurrentAsync(double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        using var client = new HttpClient();

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> GetCurrentAsync(string cityName)
    {
        var api = "1593b000aeea6d27a1247cb005f6103b";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={api}";

        using var client = new HttpClient();

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}