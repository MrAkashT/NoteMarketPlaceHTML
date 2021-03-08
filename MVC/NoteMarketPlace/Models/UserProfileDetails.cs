using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
//using NoteMarketPlace.Entities;

namespace NoteMarketPlace.Models
{
    public class UserProfileDetails
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required.")]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is Required.")]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfbirth { get; set; }

        [Display(Name = "Gender")]
        public int? GenderID { get; set; }
        
        [Required(ErrorMessage = "Phone Number is Required.")]
        public string PhoneNo { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }
        

        [Required(ErrorMessage = "Address Line 1 is Required.")]
        [Display(Name = "Address Line 1*")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "Address Line 2 is Required.")]
        [Display(Name = "Address Line 2*")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "City is Required.")]
        [Display(Name = "City *")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is Required.")]
        [Display(Name = "State *")]
        public string State { get; set; }

        [Required(ErrorMessage = " is Required.")]
        [Display(Name = "Zip Code *")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Country is Required.")]
        [Display(Name = "Country *")]
        public int? CountryId { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Code is Required.")]
        public string CountryCodeId { get; set; }

        public string University { get; set; }

        public string College { get; set; }


    }
}