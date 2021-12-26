using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }

    }
}
