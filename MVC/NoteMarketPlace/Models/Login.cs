using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NoteMarketPlace.Models
{
    public class Login
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}