using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Models
{
    public class RegisterDto
    {

       // [Required]
        //public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[RegularExpression("^(.{0,7}|[^0-9]*|[^A-Z]*|[^a-z]*|[a-zA-Z0-9]*)$", ErrorMessage = "Password must be complicated.")]
        public string Password { get; set; }

        public string Username { get; set; }

    }
}
