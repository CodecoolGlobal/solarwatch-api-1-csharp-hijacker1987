using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Xunit.Abstractions;
using Assert = NUnit.Framework.Assert;

public class SolarWatchControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SolarWatchControllerIntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GET_retrieves_weather_forecast()
    {
        try
        {
            var response = await _client.GetAsync("/");
            var stringResult = await response.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine(stringResult);

            Assert.Equals("", stringResult);
        }
        catch (Exception ex)
        {
            _testOutputHelper.WriteLine($"Exception: {ex}");
            throw;
        }
    }
}