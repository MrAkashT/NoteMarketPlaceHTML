using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class NoteDetails
    {
        public int SellerId { get; set; }
        public int NoteId { get; set; }

        [Required]
        [Display(Name = "Title *")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Category *")]
        public int CategoryId { get; set; }

        public string ForDisplay { get; set; }

        public string AttachmentPdf { get; set; }

        public string ForPreview { get; set; }

        [Display(Name = "Display Picture")]
        [RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG|.jpeg|.JPEG)$", ErrorMessage = "Please Upload Jpg or Jpeg File.")]

        public HttpPostedFileBase DisplayPicture { get; set; }

        [Required]
        [Display(Name = "Upload Notes *")]
       // [RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$", ErrorMessage = "Please Upload Pdf File.")]

        public HttpPostedFileBase NotePdf{ get; set; }

        [Display(Name = "Type")]
        public int? NoteTypeId { get; set; }

        [Display(Name = "Number Of Pages")]
        public int? NumberOfPages { get; set; }

        [Required]
        [Display(Name = "Description *")]
        public string Description { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Professor / Lecturer")]
        public string Professor { get; set; }

        [Required]
        [Display(Name = "Sell For *")]
        
        public int SellFor { get; set; }

        public bool isPaid { get; set; }

        [ForPriceValidation]
        [Display(Name = "Sell Price *")]
        public decimal? SellPrice { get; set; }

        
        [Display(Name = "Note Preview")]
        [RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$", ErrorMessage = "Please Upload Pdf File.")]

        public HttpPostedFileBase NotePreview { get; set; }

        public int Status { get; set; }
    }
}