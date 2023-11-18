using System.ComponentModel.DataAnnotations;
using SolarWatch.Contracts;

namespace SolarWatchTestProject.ContractsTests;

public class RegistrationTest
{
    [TestFixture]
    public class RegistrationResponseTests
    {
        [TestCase("test@example.com", "testUser", "Test@123", true)]
        public void Test_RegistrationResponse_Properties(string email, string username, string password, bool expectedIsValid)
        {
            var registrationResponse = new RegistrationResponse(email, username);
            Assert.Multiple(() =>
            {
                Assert.That(registrationResponse.Email, Is.EqualTo(email));
                Assert.That(registrationResponse.UserName, Is.EqualTo(username));
            });
        }

        [TestCase("test@example.com", "testUser", "Test@123", true)]
        public void Test_RegistrationRequest_Validation(string email, string username, string password, bool expectedIsValid)
        {
            var registrationRequest = new RegistrationRequest(email, username, password);
            var context = new ValidationContext(registrationRequest, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            
            var isValid = Validator.TryValidateObject(registrationRequest, context, results, validateAllProperties: true);
            
            Assert.That(isValid, Is.EqualTo(expectedIsValid));
        }
    }
}