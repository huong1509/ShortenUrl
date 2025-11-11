using System.ComponentModel.DataAnnotations;
namespace Authentication_Service.Models
{
    public class User
    {
        public int Id {  get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }
        public bool ResetCodeVerified { get; set; } = false;

    }
}
