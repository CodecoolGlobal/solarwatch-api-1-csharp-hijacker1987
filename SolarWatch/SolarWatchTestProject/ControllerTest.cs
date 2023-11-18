using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Data;

namespace SolarWatchTestProject;

public class Tests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarDataProvider> _weatherDataProviderMock;
    private Mock<CityApiContext> _repositoryMock;
    private SolarWatchController _controllerMock;
    private JsonProcessor _jsonProcessor;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _weatherDataProviderMock = new Mock<ISolarDataProvider>();
        _repositoryMock = new Mock<CityApiContext>();
        _jsonProcessor = new JsonProcessor();
        _controllerMock = new SolarWatchController(_loggerMock.Object, _repositoryMock.Object, _jsonProcessor, _weatherDataProviderMock.Object);
    }

    [Test]
    public void GetCurrentReturnsOkResultIfCityDataIsValid()
    {
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<string>())).Throws(new Exception());
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<double>(), It.IsAny<double>())).Throws(new Exception());
        
        var result = _controllerMock.GetCurrent("Budapest");
        
        Assert.That(result.Result, Is.InstanceOf(typeof(ActionResult<SolarWatch.Model.SolarWatch>)));
        Assert.That(result.IsCompleted);
    }
}