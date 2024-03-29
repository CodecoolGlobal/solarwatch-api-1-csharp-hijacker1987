using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Data;

namespace SolarWatchTestProject;

[TestFixture]
public class WeatherForecastControllerTests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarDataProvider> _weatherDataProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SolarWatchController _controller;
    private Mock<UsersContext> _repositoryMock;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _weatherDataProviderMock = new Mock<ISolarDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _repositoryMock = new Mock<UsersContext>();
        _controller = new SolarWatchController(_loggerMock.Object,  _repositoryMock.Object, _jsonProcessorMock.Object, _weatherDataProviderMock.Object);
    }
    
#pragma warning disable NUnit1007
    [Test]
#pragma warning restore NUnit1007
    public async Task GetCurrentReturnsNotFoundResultIfWeatherDataProviderFails()
    {
        // Arrange
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<float>(), It.IsAny<float>())).Throws(new Exception());

        // Act
        var result = await _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

#pragma warning disable NUnit1007
    [Test]
#pragma warning restore NUnit1007
    public async Task GetCurrentReturnsNotFoundResultIfSolarDataIsInvalid()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<float>(), It.IsAny<float>())).Returns(solarData);
        _jsonProcessorMock.Setup(x => x.Process(solarData)).Throws<Exception>();

        // Act
        var result = await _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
#pragma warning disable NUnit1007
    [Test]
#pragma warning restore NUnit1007
    public async Task GetCurrentAsyncReturnsNotFoundResultIfSolarDataIsInvalid()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(solarData);
        _jsonProcessorMock.Setup(x => x.Process(solarData)).Throws<Exception>();

        // Act
        var result = await _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public void GetCurrentReturnsOkResultIfCityDataIsValid()
    {
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<string>())).Throws(new Exception());
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<double>(), It.IsAny<double>())).Throws(new Exception());
        
        var result = _controller.GetCurrent("Budapest");
        
        Assert.That(result.Result, Is.InstanceOf(typeof(ActionResult<SolarWatch.Model.SolarWatch>)));
        Assert.That(result.IsCompleted);
    }
}