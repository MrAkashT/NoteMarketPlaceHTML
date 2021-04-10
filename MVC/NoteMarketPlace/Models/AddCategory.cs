using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AddCategory
    {
        public int cId { get; set; }

        [Required]
        [Display(Name = "Category Name *")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Description *")]
        public string Description { get; set; }

    }
}