﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SingleNoteDetail
    {
        public SellerNote sellerNote { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string BuyerName { get; set; }

    }
}