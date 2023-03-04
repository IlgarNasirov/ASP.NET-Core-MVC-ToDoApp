using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Data.Models
{
    public class Todo
    {
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        [MinLength(5, ErrorMessage = "The text field must be at least 5 characters long.")]
        public string Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
