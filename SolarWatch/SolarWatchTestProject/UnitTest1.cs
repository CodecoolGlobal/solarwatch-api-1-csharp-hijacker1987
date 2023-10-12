using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch = SolarWatch.SolarWatch;

namespace SolarWatchTestProject;

[TestFixture]
public class WeatherForecastControllerTests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ISolarDataProvider> _weatherDataProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SolarWatchController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _weatherDataProviderMock = new Mock<ISolarDataProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _controller = new SolarWatchController(_loggerMock.Object, _weatherDataProviderMock.Object, _jsonProcessorMock.Object);
    }
    
    [Test]
    public void GetCurrentReturnsNotFoundResultIfWeatherDataProviderFails()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<float>(), It.IsAny<float>())).Throws(new Exception());

        // Act
        var result = _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public void GetCurrentReturnsNotFoundResultIfSolarDataIsInvalid()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrent(It.IsAny<float>(), It.IsAny<float>())).Returns(solarData);
        _jsonProcessorMock.Setup(x => x.Process(solarData, true)).Throws<Exception>();

        // Act
        var result = _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
}