using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
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
            SearchNotes model = new SearchNotes
            {
                sellerNotes = notes,
                Categories = _context.GetCategories(),
                Types = _context.GetTypes(),
                Universities = _context.GetUniversities(),
                Courses = _context.GetCourses(),
                Countries = _context.GetCountries(),
                
            };
            return View(model);
        }
    }
}