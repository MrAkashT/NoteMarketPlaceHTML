using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NoteMarketPlace.Models
{
    public class ForgotPassword
    {
        [Required]
        public string Email { get; set; }
    }
}