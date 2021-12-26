using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }

    }
}
