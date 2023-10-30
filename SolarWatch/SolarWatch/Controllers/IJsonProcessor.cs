using SolarWatch.Model;

namespace SolarWatch.Controllers;

public interface IJsonProcessor
{
    City Process(string data);
    SunriseSunsetTimes SunTimeProcess(string data);
}