using NoteMarketPlace.DbContext;
using NoteMarketPlace.Filter;
using NoteMarketPlace.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminController : Controller
    {
        private MyDbContext _context;

        public AdminController()
        {
            _context = new MyDbContext();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [AdminAuthFilter]
        public ActionResult Dashboard()
        {

            var months = Enumerable.Range(0, 7)
                              .Select(i => DateTime.Now.AddMonths(i - 6))
                              .Select(date => date.ToString("MM"));
            
            AdminDashboardViewModel model = new AdminDashboardViewModel
            {
                PublishedCount = _context.GetPublishedNotesCount(),
                NumberOfDownload = _context.GetLastSevenDaysDownload(),
                NumberOfReg = _context.GetLastSevenDaysNewRegistration(),
                PublishedNotes = _context.GetPublishedNotes(),
                LastSixMonth = months.Reverse(),
               // month = DateTime.Now.ToString("MM")
            };

            foreach (var note in model.PublishedNotes)
            {
                if (note.IsPaid)
                    note.SellType = "Paid";
                else
                    note.SellType = "Free";

                FileInfo file = new FileInfo(note.AttachmentPath);
                long fileSize = file.Length;


                double tera =Math.Pow(1024, 4);
                double giga =Math.Pow(1024, 3);
                double mega =Math.Pow(1024, 2);
                double kilo =Math.Pow(1024, 1);
                
                if (fileSize > tera)
                {
                    note.AttachmentSize = ((float)(fileSize / tera)).ToString("0.00 TB");
                }
                else if (fileSize > giga)
                {
                    note.AttachmentSize = ((float)(fileSize / giga)).ToString("0.00 GB");
                }
                else if (fileSize > mega)
                {
                    note.AttachmentSize = ((float)(fileSize / mega)).ToString("0.00 MB");
                }
                else if (fileSize > kilo)
                {
                    note.AttachmentSize = (fileSize / kilo).ToString("0.00 KB");
                }
                else note.AttachmentSize = fileSize + " Bytes";

            }

            return View(model);
        }

        [AdminAuthFilter]
        [HttpPost]
        public ActionResult Dashboard(AdminDashboardViewModel m)
        {
            int ms = Convert.ToInt32(m.month);
            var months = Enumerable.Range(0, 7)
                              .Select(i => DateTime.Now.AddMonths(i - 6))
                              .Select(date => date.ToString("MM"));
            AdminDashboardViewModel model = new AdminDashboardViewModel
            {
                PublishedCount = _context.GetPublishedNotesCount(),
                NumberOfDownload = _context.GetLastSevenDaysDownload(),
                NumberOfReg = _context.GetLastSevenDaysNewRegistration(),
                PublishedNotes = _context.GetAdminSearchedPublishedNotes(m.searchNote, ms),
                LastSixMonth = months.Reverse(),
                //month = m.month,
                searchNote = m.searchNote
            };

            foreach (var note in model.PublishedNotes)
            {
                if (note.IsPaid)
                    note.SellType = "Paid";
                else
                    note.SellType = "Free";

                FileInfo file = new FileInfo(note.AttachmentPath);
                long fileSize = file.Length;


                double tera = Math.Pow(1024, 4);
                double giga = Math.Pow(1024, 3);
                double mega = Math.Pow(1024, 2);
                double kilo = Math.Pow(1024, 1);

                if (fileSize > tera)
                {
                    note.AttachmentSize = ((float)(fileSize / tera)).ToString("0.00 TB");
                }
                else if (fileSize > giga)
                {
                    note.AttachmentSize = ((float)(fileSize / giga)).ToString("0.00 GB");
                }
                else if (fileSize > mega)
                {
                    note.AttachmentSize = ((float)(fileSize / mega)).ToString("0.00 MB");
                }
                else if (fileSize > kilo)
                {
                    note.AttachmentSize = (fileSize / kilo).ToString("0.00 KB");
                }
                else note.AttachmentSize = fileSize + " Bytes";

            }

            return View(model);
        }
        public ActionResult NotesUnderReview()
        {
            return View();
        }
        public ActionResult PublishedNotes()
        {
            return View();
        }
        public ActionResult DownloadedNotes()
        {
            return View();
        }
        public ActionResult RejectedNotes()
        {
            return View();
        }
        public ActionResult Members()
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }
    }
}