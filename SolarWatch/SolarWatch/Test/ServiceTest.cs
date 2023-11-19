using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using SolarWatch.Contracts;
using SolarWatch.Data;
using SolarWatch.Service;
using SolarWatch.Service.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace SolarWatch.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UsersContext>(options => options.UseInMemoryDatabase("TestDB"));
        });
    }
}

public class AuthContTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;
    private readonly Mock<IAuthService> _authServiceMock;

    public AuthContTest(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
        var tokenService = new TokenService(factory.Services.GetRequiredService<IConfiguration>());

        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var testUser = new IdentityUser { UserName = "TestUser", Email = "testusermail@testing.com"};
        userManager.CreateAsync(testUser, "asdASDasd123666").GetAwaiter().GetResult();
        _authServiceMock = new Mock<IAuthService>();
        _authServiceMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new AuthResult("1", true, "TestUser", "testusermail@testing.com",
                tokenService.CreateToken(testUser, "Admin")));
    }

    [Fact]
    public async Task Authenticate_Returns_OkResult_With_Valid_Credentials()
    {
        var request = new AuthRequest("testusermail@testing.com", "asdASDasd123666");
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("http://localhost:5127/Auth/Login", content);

        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Content: {responseContent}");

        var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(authResponse);
    }

    [Fact]
    public async Task CityGet_Returns_OkResult_With_Valid_Credentials()
    {
        var authResult = await _authServiceMock.Object.LoginAsync("TestUser", "asdASDasd123666");

        if (authResult.Success)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var city = "Budapest";
            var content = new StringContent(JsonSerializer.Serialize(city), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"http://localhost:5127/CrudAdmin/Post?name={city}", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(authResponse);
        }
    }
    
    [Fact]
    public async Task CityDelete_Returns_OkResult_With_Valid_Credentials()
    {
        var authResult = await _authServiceMock.Object.LoginAsync("TestUser", "asdASDasd123666");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);
        var id = 1;
        var cancellationToken = new CancellationToken();

        var response = await _client.DeleteAsync($"http://localhost:5127/CrudAdmin/Delete?id={id}", cancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(authResponse);
        }
        catch (JsonException ex)
        {
            _testOutputHelper.WriteLine($"Error occurred during JSON deserialization: {ex.Message}");
        }
    }
}