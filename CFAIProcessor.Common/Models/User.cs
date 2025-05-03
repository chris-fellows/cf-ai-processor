using CFAIProcessor.Enums;
using System.ComponentModel.DataAnnotations;

namespace CFAIProcessor.Models
{
    public class User
    {
        [MaxLength(50)]
        public string Id { get; set; } = String.Empty;

        [MaxLength(100)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Password { get; set; } = String.Empty;

        /// <summary>
        /// Password salt
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Salt { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = String.Empty;

        /// <summary>
        /// Whether user is active. Inactive users can't log in.
        /// </summary>
        public bool Active { get; set; } = true;

        [Required]
        [MaxLength(50)]
        public string Color { get; set; } = String.Empty;

        [Required]
        [MaxLength(50)]
        public string ImageSource { get; set; } = String.Empty;

        public UserTypes GetUserType() => Name.Equals("System") ?
                    UserTypes.System : UserTypes.Normal;
    }
}
