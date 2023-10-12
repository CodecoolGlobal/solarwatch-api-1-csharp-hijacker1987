namespace SolarWatch.Controllers;

public interface IJsonProcessor
{
    SolarWatch Process(string data, bool picker);
}