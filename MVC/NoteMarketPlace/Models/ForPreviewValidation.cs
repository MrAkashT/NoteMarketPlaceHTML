using NoteMarketPlace.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ForPreviewValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var note = (NoteDetails)validationContext.ObjectInstance;
            if(note.SellFor == 4)
            {
                if(note.NotePreview != null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Note Preview Is Required For Paid Notes");
                }
            }
            else
            {
                return ValidationResult.Success;
            }
           
        }
  
    }
}