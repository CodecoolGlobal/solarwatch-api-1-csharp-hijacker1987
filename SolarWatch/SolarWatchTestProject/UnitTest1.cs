using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
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
    public async Task GetCurrentReturnsNotFoundResultIfWeatherDataProviderFails()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<float>(), It.IsAny<float>())).Throws(new Exception());

        // Act
        var result = await _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public async Task GetCurrentReturnsNotFoundResultIfSolarDataIsInvalid()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(solarData);
        _jsonProcessorMock.Setup(x => x.Process(solarData, true)).Throws<Exception>();

        // Act
        var result = await _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetCurrentAsyncReturnsNotFoundResultIfSolarDataIsInvalid()
    {
        // Arrange
        var solarData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(solarData);
        _jsonProcessorMock.Setup(x => x.Process(solarData, true)).Throws<Exception>();

        // Act
        var result = await _controller.GetCurrent(null);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    /*
    [Test]
    public async Task GetCurrentAsyncReturnsSolarWatchForValidData()
    {
        // Arrange
        var solarData = "{\"results\":{\"sunrise\":\"6:30:00 AM\",\"sunset\":\"6:30:00 PM\"},\"status\":\"OK\"}";
        var expectedSolarWatch = new global::SolarWatch.SolarWatch
        {
            City = "TestCity",
            Latitude = 123.456f,
            Longitude = 789.012f,
            Sunrise = "6:30:00 AM",
            Sunset = "6:30:00 PM"
        };

        _weatherDataProviderMock.Setup(x => x.GetCurrentAsync(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(solarData);
        _jsonProcessorMock.Setup(x => x.Process(solarData, false)).Returns(expectedSolarWatch);

        // Act
        var result = await _controller.GetCurrent("TestCity");

        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsInstanceOf(typeof(global::SolarWatch.SolarWatch), okResult?.Value);

        var solarWatch = okResult?.Value as global::SolarWatch.SolarWatch;
        Assert.That(solarWatch?.City, Is.EqualTo(expectedSolarWatch.City));
        Assert.That(solarWatch?.Latitude, Is.EqualTo(expectedSolarWatch.Latitude));
        Assert.That(solarWatch?.Longitude, Is.EqualTo(expectedSolarWatch.Longitude));
        Assert.That(solarWatch?.Sunrise, Is.EqualTo(expectedSolarWatch.Sunrise));
        Assert.That(solarWatch?.Sunset, Is.EqualTo(expectedSolarWatch.Sunset));
    }
    */
}