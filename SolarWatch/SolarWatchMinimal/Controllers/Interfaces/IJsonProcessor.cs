using SolarWatchMinimal.Model;

namespace SolarWatchMinimal.Controllers.Interfaces;

public interface IJsonProcessor
{
    City Process(string data);
    Times SunTimeProcess(string data);
}