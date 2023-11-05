/*using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatchMvp.Controllers;
using SolarWatchMvp.Data;
using SolarWatchMvp.Model;

namespace SunriseSunsetTest;

public class AdminControllerTests
{
    private Mock<ILogger<WeatherForecastController>> _loggerMock;
    private Mock<IWeatherDataProvider> _weatherDataProviderMock;
    private Mock<WeatherApiContext> _repositoryMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private CrudAdminController _controllerMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<WeatherForecastController>>();
        _weatherDataProviderMock = new Mock<IWeatherDataProvider>();
        _repositoryMock = new Mock<WeatherApiContext>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _controllerMock = new CrudAdminController(_loggerMock.Object, _weatherDataProviderMock.Object,
            _jsonProcessorMock.Object, _repositoryMock.Object);
    }
    
    [Test]
    public void GetCurrentReturnsOkResultIfCityDataIsValid()
    {
        _weatherDataProviderMock.Setup(x => x.GetCoordinates(It.IsAny<string>())).Throws(new Exception());
        _weatherDataProviderMock.Setup(x => x.GetSunTime(It.IsAny<double>(), It.IsAny<double>())).Throws(new Exception());
        
        var result = _controllerMock.Delete(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.InstanceOf(typeof(ActionResult<SunTime>)));
            Assert.That(result.IsCompleted);
        });
    }
}*/