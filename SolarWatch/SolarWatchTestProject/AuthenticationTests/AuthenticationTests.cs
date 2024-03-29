﻿using SolarWatch.Service;

namespace SolarWatchTestProject.AuthenticationTests;
    
[TestFixture]
public class AuthResultTests
{
    [Test]
    public void AuthResult_Initialization_Success()
    {
        var id = "1";
        var success = true;
        var email = "test@example.com";
        var userName = "testuser";
        var token = "testtoken";
        var errorMessages = new Dictionary<string, string>
        {
            { "Error1", "Error message 1" },
            { "Error2", "Error message 2" }
        };

        var authResult = new AuthResult(id, success, email, userName, token);
        Assert.Multiple(() =>
        {
            Assert.That(authResult.Success, Is.EqualTo(success));
        });
    }
}
