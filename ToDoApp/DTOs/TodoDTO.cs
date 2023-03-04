using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DTOs
{
    public class TodoDTO
    {
        [Required]
        [StringLength(500)]
        [MinLength(5, ErrorMessage = "The text field must be at least 5 characters long.")]
        public string Text { get; set; }
    }
}
