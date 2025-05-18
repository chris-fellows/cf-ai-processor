using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Models
{
    public class LoginCredentials
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a username")]
        public string? Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a password")]
        public string? Password { get; set; }
    }
}
