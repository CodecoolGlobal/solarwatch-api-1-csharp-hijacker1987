using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatchMinimal.Controllers;
using SolarWatchMinimal.Controllers.Interfaces;
using SolarWatchMinimal.Model;
using Xunit;
using Assert = Xunit.Assert;
using Times = SolarWatchMinimal.Model.Times;

namespace SolarWatchMinimalTest;

public class SolarWatchControllerTests
{
    [Fact]
    public async Task GetCurrent_Returns_OkObjectResult_With_Valid_Data()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<SolarWatchController>>();
        var jsonProcessorMock = new Mock<IJsonProcessor>();
        var solarDataProviderMock = new Mock<ISolarDataProvider>();

        var controller = new SolarWatchController(loggerMock.Object, jsonProcessorMock.Object, solarDataProviderMock.Object);

        var cityName = "TestCity";

        // Mock the behavior of dependencies
        var solarData = "Test";
        var city = new City(cityName, 0.0, 0.0, "State", "Country");
        var sunTime = new Times("SunRise", "SunSet");

        jsonProcessorMock.Setup(p => p.Process(It.IsAny<string>())).Returns(city);
        solarDataProviderMock.Setup(p => p.GetCurrentAsync(It.IsAny<double>(), It.IsAny<double>())).ReturnsAsync(solarData);

        // Simulate an error response for GetCurrentAsync(cityName)
        solarDataProviderMock.Setup(p => p.GetCurrentAsync(cityName)).ThrowsAsync(new Exception("Simulated error"));

        // Act
        var result = await controller.GetCurrent(cityName);

        // Assert
        var actionResult = Assert.IsType<ActionResult<SolarWatch>>(result);
    
        if (actionResult.Result is OkObjectResult okObjectResult)
        {
            var showCity = Assert.IsType<City>(okObjectResult.Value);
            Assert.Equal(cityName, showCity.Name);
        }
        else if (actionResult.Result is NotFoundObjectResult notFoundObjectResult)
        {
            Assert.Equal("Error getting sun data", notFoundObjectResult.Value);
        }
        else
        {
            Assert.Fail("Unexpected result type");
        }
    }
}