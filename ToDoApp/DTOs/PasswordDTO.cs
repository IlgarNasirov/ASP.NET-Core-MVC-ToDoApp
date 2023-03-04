using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DTOs
{
    public class PasswordDTO
    {
        [Required]
        [StringLength(65)]
        [MinLength(6, ErrorMessage = "The password field must be at least 6 characters long.")]
        public string Password { get; set; }
        [Compare("Password")]
        public string Repassword { get; set; }
    }
}
