using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ManageSystemConfiguration
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Support Email Address *")]
        public string SupportEmail { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Support Phone Number *")]
        public string SupportPhoneNo { get; set; }

        [Required]
        [Display(Name = "Email Address (es) (for various events system will sen notification to these users) *")]
        public string EmailForEvent { get; set; }

        [Display(Name = "Facebook URL")]
        public string FacebookURL { get; set; }

        [Display(Name = "Twitter URL")]
        public string TwitterURL { get; set; }

        [Display(Name = "LinkedIn URL")]
        public string LinkedInURL { get; set; }

        [Display(Name = "Default image for notes (if seller do not upload)")]
        public HttpPostedFileBase ImageForNotes { get; set; }

        [Display(Name = "Default profile picture (if seller do not upload)")]
        public HttpPostedFileBase ImageForUser { get; set; }


    }
}