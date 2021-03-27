using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ChangePassword
    {
        [Required]
        [ChangePassValidation]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [NewPassValidation]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}