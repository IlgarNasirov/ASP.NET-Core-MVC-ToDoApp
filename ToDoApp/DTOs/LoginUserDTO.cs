using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "The password field must be at least 6 characters long.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
