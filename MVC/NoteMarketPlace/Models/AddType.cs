using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AddType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Type Name *")]
        public string TypeName { get; set; }

        [Required]
        [Display(Name = "Description *")]
        public string Description { get; set; }
    }
}