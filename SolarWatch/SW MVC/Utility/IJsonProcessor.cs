using SW_MVC.Models;

namespace SW_MVC.Utility;

public interface IJsonProcessor
{
    City Process(string data);
    SunriseSunsetTimes SunTimeProcess(string data);
}