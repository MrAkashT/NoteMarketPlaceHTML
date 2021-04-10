using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class AddAdministratorViewModel
    {
        public AddAdministrator AddAdmin { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}