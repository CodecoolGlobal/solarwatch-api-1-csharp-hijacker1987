using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Contracts;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Service;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UsersContext? _usersContext;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(IAuthService authenticationService, UsersContext usersContext, UserManager<IdentityUser> userManager)
    {
        _authService = authenticationService;
        _usersContext = usersContext;
        _userManager = userManager;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(request.Email, request.Username, request.Password, "User");

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        var newUser = new User(request.Username, request.Email, request.Password, result.Id);
        
        _usersContext!.UsersDb!.Add(newUser);
        await _usersContext.SaveChangesAsync();

        return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));
    }

    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        Console.WriteLine(request);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }

        return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
    }
    
    [HttpPatch("ChangePassword")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return BadRequest(existingUser);
            }

            var result = await _userManager.ChangePasswordAsync(existingUser, request.CurrentPassword, request.NewPassword);

            await _usersContext.SaveChangesAsync();

            if (result.Succeeded)
            {
                await _usersContext.SaveChangesAsync();
                return Ok($"Successful pass change {request.Email}");
            }
            else
            {
                return BadRequest($"Error pass change {request.Email}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound("Error delete sun data");
        }
    }
}