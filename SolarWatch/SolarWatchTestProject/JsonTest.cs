using SolarWatch.Controllers;

namespace SolarWatchTestProject;

[TestFixture]
public class JsonProcessorTests
{
    private JsonProcessor _jsonProcessor;

    [SetUp]
    public void Setup()
    {
        _jsonProcessor = new JsonProcessor();
    }

    [Test]
    public void CoordinateProcess_ValidData_ReturnsCityCoordinate()
    {
        const string dataWithOutState = "[{\"name\": \"Budapest\", \"lat\": 40.7128, \"lon\": -74.0060, \"country\": \"HU\"}]";
        const string dataWithState = "[{\"name\": \"Chicago\", \"lat\": 41.8755616, \"lon\": -87.6244212, \"state\": \"Illinois\", \"country\": \"US\"}]";
        
        var resultWithOutState = _jsonProcessor.Process(dataWithOutState);
        Assert.Multiple(() =>
        {
            Assert.That(resultWithOutState.Name, Is.EqualTo("Budapest"));
            Assert.That(resultWithOutState.Latitude, Is.EqualTo(40.7128));
            Assert.That(resultWithOutState.Longitude, Is.EqualTo(-74.0060));
            Assert.That(resultWithOutState.State, Is.EqualTo("-"));
            Assert.That(resultWithOutState.Country, Is.EqualTo("HU"));
        });
        
        var resultWithState = _jsonProcessor.Process(dataWithState);
        Assert.Multiple(() =>
        {
            Assert.That(resultWithState.Name, Is.EqualTo("Chicago"));
            Assert.That(resultWithState.Latitude, Is.EqualTo(41.8755616));
            Assert.That(resultWithState.Longitude, Is.EqualTo(-87.6244212));
            Assert.That(resultWithState.State, Is.EqualTo("Illinois"));
            Assert.That(resultWithState.Country, Is.EqualTo("US"));
        });
    }

    [Test]
    public void SunTimeProcess_ValidData_ReturnsSolarWatch()
    {
        const string data = "{\"results\":{\"sunrise\":\"2023-10-14T06:45:00+00:00\",\"sunset\":\"2023-10-14T18:20:00+00:00\"}}";
        
        var result = _jsonProcessor.SunTimeProcess(data);
        Assert.Multiple(() =>
        {
            Assert.That(result.SunRiseTime, Is.EqualTo("2023-10-14T06:45:00+00:00"));
            Assert.That(result.SunSetTime, Is.EqualTo("2023-10-14T18:20:00+00:00"));
        });
    }
}
