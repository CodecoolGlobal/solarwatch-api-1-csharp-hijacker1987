using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Contracts;

public record ChangePasswordRequest([Required] string Email, [Required] string CurrentPassword, [Required][MinLength(6)]string NewPassword);

