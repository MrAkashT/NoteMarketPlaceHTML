using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ForPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (UserModel)validationContext.ObjectInstance;
            if(user.Password == null)
            {
                return new ValidationResult("Password is Required");
            }
            bool hasLower = false, hasUpper = false;
            bool hasDigit = false, specialChar = false;
            for (int i = 0; i < user.Password.Length; i++)
            {
                if (Char.IsDigit(user.Password[i]))
                    hasDigit = true;
                if (Char.IsUpper(user.Password[i]))
                    hasUpper = true;
                if (Char.IsLower(user.Password[i]))
                    hasLower = true;
            }
            Regex rgx = new Regex("[^A-Za-z0-9]");
            specialChar = rgx.IsMatch(user.Password);
            if (hasLower && hasUpper && hasDigit && specialChar && user.Password.Length>=6 && user.Password.Length <= 24)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Password must contain 1 Uppecase, 1 Lowercase, 1 Digit and 1 Special Character");
            }
        }
    }
}