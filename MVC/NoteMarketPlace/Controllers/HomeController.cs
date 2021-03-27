using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using NoteMarketPlace.DbContext;
using NoteMarketPlace.Models;


namespace NoteMarketPlace.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext _context;

        public HomeController()
        {
            _context = new MyDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Contact()
        {
            
            if(Session["userId"] != null)
            {
                int id = (int)Session["userId"];
                var user = _context.GetUser(id);
                ContactUs model = new ContactUs
                {
                    FirstName = user.FirstName,
                    Email = user.EmailID
                };
                ViewBag.contact = "UserContact";
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(ContactUs model)
        {
            if (!ModelState.IsValid)
            {
                return View("Contact", model);
            }

            var fromEmail = new MailAddress("akashthakar008@gmail.com", "NoteMarketPlace");

            //here the temporary email , but we need to send email to admin.
            var toEmail = new MailAddress("akash.thakar42@gmail.com");
            var fromEmailPassword = "8460566920";
            string sub = model.FirstName + " - Query";
            string body = "Hello, <br/><br/>" +
                model.Comments + "<br/><br/>" +
                "Regards, <br/>" + model.FirstName;
                

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            })

            smtp.Send(message);
            ModelState.Clear();
            ViewBag.send = "success";
            return View("Contact");
        }

        public ActionResult SearchNotes()
        {
        
            var notes = _context.GetSellerNote();
            string[] rating = { "1+", "2+", "3+", "4+","5" };
            List<string> ratings = new List<string>(rating);

            SearchNotes model = new SearchNotes
            {
                sellerNotes = notes.Skip(0).Take(6),
                Categories = _context.GetCategories(),
                Types = _context.GetTypes(),
                Universities = _context.GetUniversities(),
                Courses = _context.GetCourses(),
                Countries = _context.GetCountries(),
                RatingList = ratings,
                totalCount = notes.Count(),
                perPageCount = 6
            };
            foreach (var note in model.sellerNotes)
            {
                note.avg = _context.GetAvgRatingByNoteId(note.ID);
                note.count = _context.GetRatingCount(note.ID);
                note.inappropriateCount = _context.GetNotesReportedIssueCount(note.ID);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult SearchNotesPagination(int start = 0, int count = 6)
        {
            var notes = _context.GetSellerNote();
           
            SearchNotes model = new SearchNotes
            {
                sellerNotes = notes.Skip(start).Take(count)
            };
            foreach (var note in model.sellerNotes)
            {
                note.avg = _context.GetAvgRatingByNoteId(note.ID);
                note.count = _context.GetRatingCount(note.ID);
                note.inappropriateCount = _context.GetNotesReportedIssueCount(note.ID);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}