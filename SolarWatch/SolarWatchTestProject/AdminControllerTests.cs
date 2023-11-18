using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatchTestProject;

public class AdminControllerTests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarDataProvider> _weatherDataProviderMock;
    private Mock<CityApiContext> _repositoryMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private CrudAdminController _controllerMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _weatherDataProviderMock = new Mock<ISolarDataProvider>();
        _repositoryMock = new Mock<CityApiContext>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _controllerMock = new CrudAdminController(_loggerMock.Object, _weatherDataProviderMock.Object, _jsonProcessorMock.Object, _repositoryMock.Object);
    }
    
    [Test]
    public void GetCurrentReturnsOkResultIfCityDataIsValid()
    {
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<string>())).Throws(new Exception());
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<double>(), It.IsAny<double>())).Throws(new Exception());
        
        var result = _controllerMock.Delete(1);
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.InstanceOf(typeof(ActionResult<SunriseSunsetTimes>)));
            Assert.That(result.IsCompleted);
        });
    }
}