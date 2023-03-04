using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DTOs
{
    public class ForgotPasswordEmailDTO
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
