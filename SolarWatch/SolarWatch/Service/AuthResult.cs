namespace SolarWatch.Service;

public record AuthResult(
    string Id,
    bool Success,
    string UserName,
    string Email,
    string Token)
{
    //Error code - error message
    public readonly Dictionary<string, string> ErrorMessages = new();
}