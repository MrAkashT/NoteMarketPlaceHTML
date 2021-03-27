using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.ViewModel
{
    public class BuyerReqViewModel
    {
        public IEnumerable<MyTable> BuyerReq { get; set; }
        public string search { get; set; }
    }
}