namespace SW_MVC.Utility;

public interface ISolarDataPro
{
    Task<string> GetCurrentAsync(string cityName);
    Task<string> GetCurrentAsync(double lat, double lon);
}