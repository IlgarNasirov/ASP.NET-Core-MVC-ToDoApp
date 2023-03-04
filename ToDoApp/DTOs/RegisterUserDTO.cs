using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        [StringLength(30)]
        [MinLength(5)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
