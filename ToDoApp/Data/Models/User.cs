using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Data.Models
{
        public class User
        {
            public int Id { get; set; }
            [Required]
            [StringLength(30)]
            public string FullName { get; set; }
            [Required]
            [StringLength(50)]
            public string Email { get; set; }
            [StringLength(65)]
            public string? PasswordHash { get; set; }
            [StringLength(130)]
            public string? ActivateAccountToken { get; set; }
            public DateTime? ActivatedTime { get; set; }
            [StringLength(130)]
            public string? PasswordResetToken { get; set; }
            public DateTime? ResetExpireDate { get; set; }
            public bool Status { get; set; } = false;
            public ICollection<Todo> Todos { get; set; }
        }
}
