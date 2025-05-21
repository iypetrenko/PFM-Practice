using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(24)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;
    }
}