using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SolarWatch.Contracts;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Service;

namespace SolarWatchTestProject.RegisterControllerTests;

[TestFixture]
public class AuthControllerTests
{
    private AuthController _authController;
    private Mock<IAuthService> _mockAuthService;
    private UsersContext _usersContext;
    private UserManager<IdentityUser> _userManager;

    [SetUp]
    public void Setup()
    {
        _mockAuthService = new Mock<IAuthService>();

        // Configure in-memory database for testing
        var options = new DbContextOptionsBuilder<UsersContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;

        _usersContext = new UsersContext(options);
        _authController = new AuthController(_mockAuthService.Object, _usersContext, _userManager);
    }

#pragma warning disable NUnit1007
    [Test]
#pragma warning restore NUnit1007
    public async Task Register_ValidInput_ReturnsCreatedResult()
    {
        var registrationRequest = new RegistrationRequest("test@example.com", "testuser", "testpassword");

        var registrationResponse = new RegistrationResponse(registrationRequest.Email, registrationRequest.Username);
        var authResult = new AuthResult("1", true, registrationRequest.Username, registrationRequest.Email, "");

        _mockAuthService.Setup(service => service.RegisterAsync(
            registrationRequest.Email, 
            registrationRequest.Username, 
            registrationRequest.Password, 
            "User")).ReturnsAsync(authResult);

        var result = await _authController.Register(registrationRequest);

        Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        var createdResult = (CreatedAtActionResult)result.Result!;
        Assert.That(createdResult?.ActionName, Is.EqualTo("Register"));
        var data = (RegistrationResponse)createdResult!.Value!;
        Assert.That(data.Email, Is.EqualTo(registrationResponse.Email));
        Assert.That(data.UserName, Is.EqualTo(registrationResponse.UserName));
    }

#pragma warning disable NUnit1007
    [Test]
#pragma warning restore NUnit1007
    public async Task Authenticate_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var authRequest = new AuthRequest("test@example.com", "testpassword");

        var authResponse = new AuthResponse(authRequest.Email, "testuser", "testtoken");
        var authResult = new AuthResult("1",true, "testuser", authRequest.Email, "testtoken");

        _mockAuthService.Setup(service => service.LoginAsync(authRequest.Email, authRequest.Password)).ReturnsAsync(authResult);
        
        var result = await _authController.Authenticate(authRequest);
        
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult)result!.Result!;
        var data = (AuthResponse)okResult!.Value!;
        Assert.Multiple(() =>
        {
            Assert.That(data?.Email, Is.EqualTo(authResponse.Email));
            Assert.That(data?.UserName, Is.EqualTo(authResponse.UserName));
            Assert.That(data?.Token, Is.EqualTo(authResponse.Token));
        });
    }
}
