using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AdminTable
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public int reportId { get; set; }
        public int BuyerId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public bool IsPaid { get; set; }
        public string RejectBy { get; set; }
        public string ReportedBy { get; set; }
        public string Remark { get; set; }
        public decimal? Price { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DownloadDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string SellType { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public int DownloadedNotes { get; set; }
        public decimal TotalEarning { get; set; }
    }
}