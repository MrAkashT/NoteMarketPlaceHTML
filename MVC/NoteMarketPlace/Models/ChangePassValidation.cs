using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ChangePassValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ChangePassword)validationContext.ObjectInstance;
            if (model.OldPassword == null)
            {
                return new ValidationResult("Password is Required");
            }
            bool hasLower = false, hasUpper = false;
            bool hasDigit = false, specialChar = false;
            for (int i = 0; i < model.OldPassword.Length; i++)
            {
                if (Char.IsDigit(model.OldPassword[i]))
                    hasDigit = true;
                if (Char.IsUpper(model.OldPassword[i]))
                    hasUpper = true;
                if (Char.IsLower(model.OldPassword[i]))
                    hasLower = true;
            }
            Regex rgx = new Regex("[^A-Za-z0-9]");
            specialChar = rgx.IsMatch(model.OldPassword);
            if (hasLower && hasUpper && hasDigit && specialChar && model.OldPassword.Length >= 6 && model.OldPassword.Length <= 24)
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