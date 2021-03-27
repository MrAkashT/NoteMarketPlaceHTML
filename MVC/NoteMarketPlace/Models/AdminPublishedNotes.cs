﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AdminPublishedNotes
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentSize { get; set; }
        public string SellType { get; set; }
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedDate { get; set; }
        public int NumberOfDownloads { get; set; }

    }
}