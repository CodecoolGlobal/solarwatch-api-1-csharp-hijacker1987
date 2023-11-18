using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatch.Contracts;
using SolarWatch.Controllers;
using SolarWatch.Service;

namespace SolarWatchTestProject.RegisterControllerTests;

[TestFixture]
public class AuthControllerTests
{
    private AuthController _authController;
    private Mock<IAuthService> _mockAuthService;

    [SetUp]
    public void Setup()
    {
        _mockAuthService = new Mock<IAuthService>();
        _authController = new AuthController(_mockAuthService.Object);
    }

    [Test]
    public async Task Register_ValidInput_ReturnsCreatedResult()
    {
        var registrationRequest = new RegistrationRequest("test@example.com", "testuser", "testpassword");

        var registrationResponse = new RegistrationResponse(registrationRequest.Email, registrationRequest.Username);
        var authResult = new AuthResult(true, registrationRequest.Email, registrationRequest.Username, "");

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

    [Test]
    public async Task Authenticate_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var authRequest = new AuthRequest("testuser", "testpassword");

        var authResponse = new AuthResponse(authRequest.Email, "test@example.com", "testtoken");
        var authResult = new AuthResult(true, authRequest.Email, "test@example.com", "testtoken");

        _mockAuthService.Setup(service => service.LoginAsync(authRequest.Email, authRequest.Password)).ReturnsAsync(authResult);
        
        var result = await _authController.Authenticate(authRequest);
        
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult)result!.Result!;
        var data = (AuthResponse)okResult!.Value!;
        Assert.That(data?.Email, Is.EqualTo(authResponse.Email));
        Assert.That(data?.UserName, Is.EqualTo(authResponse.UserName));
        Assert.That(data?.Token, Is.EqualTo(authResponse.Token));
    }
}
