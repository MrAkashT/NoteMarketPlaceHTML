using NoteMarketPlace.DbContext;
using NoteMarketPlace.Filter;
using NoteMarketPlace.Models;
using NoteMarketPlace.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        //[AdminAuthFilter]
        //public ActionResult Dashboard()
        //{

        //    var months = Enumerable.Range(0, 7)
        //                      .Select(i => DateTime.Now.AddMonths(i - 6))
        //                      .Select(date => date.ToString("MM"));

        //    AdminDashboardViewModel model = new AdminDashboardViewModel
        //    {
        //        PublishedCount = _context.GetPublishedNotesCount(),
        //        NumberOfDownload = _context.GetLastSevenDaysDownload(),
        //        NumberOfReg = _context.GetLastSevenDaysNewRegistration(),
        //        PublishedNotes = _context.GetPublishedNotes(),
        //        LastSixMonth = months.Reverse(),
        //        month = DateTime.Now.ToString("MM")
        //    };

        //    foreach (var note in model.PublishedNotes)
        //    {
        //        if (note.IsPaid)
        //            note.SellType = "Paid";
        //        else
        //            note.SellType = "Free";

        //        FileInfo file = new FileInfo(note.AttachmentPath);
        //        long fileSize = file.Length;


        //        double tera = Math.Pow(1024, 4);
        //        double giga = Math.Pow(1024, 3);
        //        double mega = Math.Pow(1024, 2);
        //        double kilo = Math.Pow(1024, 1);

        //        if (fileSize > tera)
        //        {
        //            note.AttachmentSize = ((float)(fileSize / tera)).ToString("0.00 TB");
        //        }
        //        else if (fileSize > giga)
        //        {
        //            note.AttachmentSize = ((float)(fileSize / giga)).ToString("0.00 GB");
        //        }
        //        else if (fileSize > mega)
        //        {
        //            note.AttachmentSize = ((float)(fileSize / mega)).ToString("0.00 MB");
        //        }
        //        else if (fileSize > kilo)
        //        {
        //            note.AttachmentSize = (fileSize / kilo).ToString("0.00 KB");
        //        }
        //        else note.AttachmentSize = fileSize + " Bytes";

        //    }

        //    return View(model);
        //}

        //[AdminAuthFilter]
        //[HttpPost]
        //public ActionResult Dashboard(AdminDashboardViewModel m)
        //{
        //    int ms = Convert.ToInt32(m.month);
        //    var months = Enumerable.Range(0, 7)
        //                      .Select(i => DateTime.Now.AddMonths(i - 6))
        //                      .Select(date => date.ToString("MM"));
        //    AdminDashboardViewModel model = new AdminDashboardViewModel
        //    {
        //        PublishedCount = _context.GetPublishedNotesCount(),
        //        NumberOfDownload = _context.GetLastSevenDaysDownload(),
        //        NumberOfReg = _context.GetLastSevenDaysNewRegistration(),
        //        PublishedNotes = _context.GetAdminSearchedPublishedNotes(m.searchNote, ms),
        //        LastSixMonth = months.Reverse(),

        //        searchNote = m.searchNote
        //    };

        //    foreach (var note in model.PublishedNotes)
        //    {
        //        if (note.IsPaid)
        //            note.SellType = "Paid";
        //        else
        //            note.SellType = "Free";

        //        FileInfo file = new FileInfo(note.AttachmentPath);
        //        long fileSize = file.Length;


        //        double tera = Math.Pow(1024, 4);
        //        double giga = Math.Pow(1024, 3);
        //        double mega = Math.Pow(1024, 2);
        //        double kilo = Math.Pow(1024, 1);

        //        if (fileSize > tera)
        //        {
        //            note.AttachmentSize = ((float)(fileSize / tera)).ToString("0.00 TB");
        //        }
        //        else if (fileSize > giga)
        //        {
        //            note.AttachmentSize = ((float)(fileSize / giga)).ToString("0.00 GB");
        //        }
        //        else if (fileSize > mega)
        //        {
        //            note.AttachmentSize = ((float)(fileSize / mega)).ToString("0.00 MB");
        //        }
        //        else if (fileSize > kilo)
        //        {
        //            note.AttachmentSize = (fileSize / kilo).ToString("0.00 KB");
        //        }
        //        else note.AttachmentSize = fileSize + " Bytes";

        //    }

        //    return View(model);
        //}
        // // /// // // //
        [AdminAuthFilter]
        [HttpGet]
        public ActionResult Dashboard(string search , string month)
        {
            if (month == "")
                month = null;
            if (search == "")
                search = null;
            
            int ms = Convert.ToInt32(month);
            var months = Enumerable.Range(0, 7)
                              .Select(i => DateTime.Now.AddMonths(i - 6))
                              .Select(date => date.ToString("MM"));
            AdminDashboardViewModel model = new AdminDashboardViewModel
            {
                PublishedCount = _context.GetPublishedNotesCount(),
                NumberOfDownload = _context.GetLastSevenDaysDownload(),
                NumberOfReg = _context.GetLastSevenDaysNewRegistration(),
                PublishedNotes = _context.GetAdminSearchedPublishedNotes(search, ms),
                LastSixMonth = months.Reverse(),
                month = month,
                searchNote = search
            };
            if(search == null && month == null)
            {
                model.month = DateTime.Now.ToString("MM");
            }

            foreach (var note in model.PublishedNotes)
            {
                if (note.IsPaid)
                    note.SellType = "Paid";
                else
                    note.SellType = "Free";

                if(note.Price == null)
                    note.Price = 0;

                note.NumberOfDownloads = _context.GetNumberOfDownloads(note.Id);
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

        // // /// /// ////
        [AdminAuthFilter]
        [HttpGet]
        public ActionResult NotesUnderReview(string name, string search)
        {
            if (name == "" || name == null)
                name = "0"; 
            if (search == "")
                search = null;
            int sellerid = Convert.ToInt32(name);
            NotesUnderReviewViewModel model = new NotesUnderReviewViewModel
            {
                UnderReviewNotes = _context.GetUnderReviewNotes(name, search),
                Sellers = _context.GetAllSellers(),
                Seller = sellerid,
                Search = search
            };
            return View(model);
        }
        [AdminAuthFilter]
        [HttpGet]
        public ActionResult PublishedNotes(string name, string search)
        {
            if (name == "" || name == null)
                name = "0";
            if (search == "")
                search = null;
            int sellerid = Convert.ToInt32(name);
            AdminNotesViewModel model = new AdminNotesViewModel
            {
                PublishedNotes = _context.GetAdminPublishedNotes(name, search),
                Sellers = _context.GetAllPublishedSellers(),
                Seller = sellerid,
                Search = search
            };
            return View(model);
        }
        [AdminAuthFilter]
        [HttpGet]
        public ActionResult DownloadedNotes(string note, string buyer, string seller, string search)
        {
            if (note == "")
                note = null;
            if (buyer == "" || buyer == null)
                buyer = "0";
            if (seller == "" || seller == null)
                seller = "0";
            if (search == "")
                search = null;
            int buyerid = Convert.ToInt32(buyer);
            int sellerid = Convert.ToInt32(seller);
            AdminNotesViewModel model = new AdminNotesViewModel
            {
                DownloadedNotes = _context.GetAdminDownloadedNotes(note, buyer, seller, search),
                Sellers = _context.GetAllDownloadedSeller(),
                Buyers = _context.GetAllBuyers(),
                NotesTitles = _context.GetAllNotesName()
            };
            return View(model);
        }

        [AdminAuthFilter]
        [HttpGet]
        public ActionResult RejectedNotes(string search, string seller)
        {
            if (search == "")
                search = null;
            if (seller == "" || seller == null)
                seller = null;

            int sellerid = Convert.ToInt32(seller);
            AdminNotesViewModel model = new AdminNotesViewModel
            {
                RejectedNotes = _context.GetAdminRejectedNotes(search, seller),
                Sellers = _context.GetRejectedSellers(),
                Search = search,
                Seller = sellerid
            };
            return View(model);
        }

        [AdminAuthFilter]
        [HttpGet]
        public ActionResult Members(string search)
        {
            if (search == "")
                search = null;

            MembersViewModel model = new MembersViewModel
            {
                Members = _context.GetMembers(search),
                Search = search
            };
            return View(model);
        }
        [AdminAuthFilter]
        [HttpGet]
        public ActionResult MemberDetails(int id)
        {
            MemberDetailsViewModel model = new MemberDetailsViewModel
            {
                Member = _context.GetMemberDetails(id),
                MemberNotes = _context.GetMemberNotes(id)
            };
            ViewBag.defaultPic =  _context.GetDefaultUserPic("default image for user");
            return View(model);
        }
        [HttpPost]
        [AdminAuthFilter]
        public void Deactive(int id)
        {
            var member = _context.GetMemberById(id);
            member.IsActive = false;
            _context.UpdateUp();
        }

        [AdminAuthFilter]
        [HttpGet]
        public ActionResult Reports(string search)
        {
            if (search == "")
                search = null;
            SpamReportViewModel model = new SpamReportViewModel
            {
                Reports = _context.GetSpamReports(search)
            };
            return View(model);
        }
        [HttpPost]
        public void RemoveIssue(int id)
        {
            _context.RemoveReportedIssue(id);
        }

        [AdminAuthFilter]
        [HttpPost]
        public void UnpublishNote(int NoteId, string remark)
        {
            int id = (int)Session["userId"];
            var note = _context.GetSellerNoteByNoteId(NoteId);
            int statusId = _context.GetStatusId("removed");
            note.Status = statusId;
            note.ActionedBy = id;
            note.AdminRemarks = remark;
            _context.UpdateUp();

            var seller = _context.GetUser(note.SellerID);
            var fromEmail = new MailAddress("akashthakar008@gmail.com", "NoteMarketPlace");
            var toEmail = new MailAddress(seller.EmailID);
            var fromEmailPassword = "8460566920";
            string sub = "Sorry! We need to remove your notes from our portal. ";
            string body = "Hello " + seller.FirstName + ",<br/><br/>" +
                "We want to inform you that, your note " + note.Title + " has been removed from the portal." +
                "Please find remark as below,<br/><br/>" +
                remark + "<br/><br/>" +
                "Regards, <br/>" +
                "Notes MarketPlace";

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
        }

        [AdminAuthFilter]
        [HttpPost]
        public void Approve(int noteId)
        {
            int adminId = (int)Session["admin"];
            var note = _context.GetSellerNoteByNoteId(noteId);

            note.ActionedBy = adminId;
            note.PublishedDate = DateTime.Now;
            note.Status = _context.GetStatusId("published");
            note.ModifiedBy = adminId;
            note.ModifiedDate = DateTime.Now;
            _context.UpdateUp();
        }

        [AdminAuthFilter]
        [HttpPost]
        public void InReview(int noteId)
        {
            int adminId = (int)Session["admin"];
            var note = _context.GetSellerNoteByNoteId(noteId);

            note.ActionedBy = adminId;
            note.Status = _context.GetStatusId("in review");
            note.ModifiedBy = adminId;
            note.ModifiedDate = DateTime.Now;
            _context.UpdateUp();
        }

        [AdminAuthFilter]
        [HttpPost]
        public void Reject(int noteId, string remark)
        {
            int adminId = (int)Session["admin"];
            var note = _context.GetSellerNoteByNoteId(noteId);

            note.ActionedBy = adminId;
            note.AdminRemarks = remark;
            note.Status = _context.GetStatusId("rejected");
            note.ModifiedBy = adminId;
            note.ModifiedDate = DateTime.Now;
            _context.UpdateUp();
        }
        [AdminAuthFilter]
        public ActionResult ManageSystemConfiguration()
        {
            return View();
        }

        [AdminAuthFilter]
        [HttpPost]
        public ActionResult SaveSystemConfiguration(ManageSystemConfiguration model)
        {
            int id = (int)Session["super admin"];
            if (!ModelState.IsValid)
            {
                return View("ManageSystemConfiguration", model);
            }
            _context.RemoveSystemConfig();
            SystemConfiguration Configuration1 = new SystemConfiguration
            {
                key = "Support Email Address",
                Value = model.SupportEmail,
                CreatedDate = DateTime.Now,
                CreatedBy = id,
                ModifiedDate = DateTime.Now,
                ModifiedBy = id
            };
            _context.AddSystemConfig(Configuration1);
            SystemConfiguration Configuration2 = new SystemConfiguration
            {
                key = "Support Phone Number",
                Value = model.SupportPhoneNo,
                CreatedDate = DateTime.Now,
                CreatedBy = id,
                ModifiedDate = DateTime.Now,
                ModifiedBy = id
            };
            _context.AddSystemConfig(Configuration2);
            SystemConfiguration Configuration3 = new SystemConfiguration
            {
                key = "Email Addresses For Notify",
                Value = model.EmailForEvent,
                CreatedDate = DateTime.Now,
                CreatedBy = id,
                ModifiedDate = DateTime.Now,
                ModifiedBy = id
            };
            _context.AddSystemConfig(Configuration3);
            SystemConfiguration Configuration4 = new SystemConfiguration
            {
                key = "Facebook Url",
                Value = model.FacebookURL,
                CreatedDate = DateTime.Now,
                CreatedBy = id,
                ModifiedDate = DateTime.Now,
                ModifiedBy = id
            };
            _context.AddSystemConfig(Configuration4);
            SystemConfiguration Configuration5 = new SystemConfiguration
            {
                key = "Twitter Url",
                Value = model.TwitterURL,
                CreatedDate = DateTime.Now,
                CreatedBy = id,
                ModifiedDate = DateTime.Now,
                ModifiedBy = id
            };
            _context.AddSystemConfig(Configuration5);
            SystemConfiguration Configuration6 = new SystemConfiguration
            {
                key = "LinkedIn Url",
                Value = model.LinkedInURL,
                CreatedDate = DateTime.Now,
                CreatedBy = id,
                ModifiedDate = DateTime.Now,
                ModifiedBy = id
            };
            _context.AddSystemConfig(Configuration6);

            if (model.ImageForNotes != null)
            {
                string pic = Path.GetExtension(model.ImageForNotes.FileName);
                if (pic.ToLower() != ".jpg" && pic.ToLower() != ".jpeg")
                {
                    
                    ModelState.AddModelError("note pic format", "please upload .jpg or .jpeg file.");
                    return View("ManageSystemConfiguration", model);
                }
                var noteImageName = "note_" + Path.GetFileName(model.ImageForNotes.FileName);

                string imgPath = Path.Combine(Server.MapPath("~/DefaultImages/"), noteImageName);
                model.ImageForNotes.SaveAs(imgPath);

                SystemConfiguration Configuration7 = new SystemConfiguration
                {
                    key = "Default Image For Notes",
                    Value = noteImageName,
                    CreatedDate = DateTime.Now,
                    CreatedBy = id,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = id
                };
                _context.AddSystemConfig(Configuration7);
            }
            if (model.ImageForUser != null)
            {
                string pic = Path.GetExtension(model.ImageForUser.FileName);
                if (pic.ToLower() != ".jpg" && pic.ToLower() != ".jpeg")
                {

                    ModelState.AddModelError("user pic format", "please upload .jpg or .jpeg file.");
                    return View("ManageSystemConfiguration", model);
                }
                var userImageName = "user" + Path.GetFileName(model.ImageForUser.FileName);

                string imgPath = Path.Combine(Server.MapPath("~/DefaultImages/"), userImageName);
                model.ImageForUser.SaveAs(imgPath);

                SystemConfiguration Configuration8 = new SystemConfiguration
                {
                    key = "Default Image For User",
                    Value = userImageName,
                    CreatedDate = DateTime.Now,
                    CreatedBy = id,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = id
                };
                _context.AddSystemConfig(Configuration8);
            }

            _context.UpdateUp();
            
            return RedirectToAction("Dashboard", "Admin");
        }
        [AdminAuthFilter]
        public ActionResult ManageAdministrator(string search)
        {
            if (search == "")
                search = null;
            ManageViewModel model = new ManageViewModel
            {
                Manages = _context.GetManageAdmin(search)
            };
            return View(model);
        }

        [AdminAuthFilter]
        [HttpGet]
        public ActionResult ManageCategory(string search)
        {
            if (search == "")
                search = null;
            ManageViewModel model = new ManageViewModel
            {
                Manages = _context.GetManageCategories(search)
            };
            return View(model);
        }

        [AdminAuthFilter]
        [HttpGet]
        public ActionResult ManageType(string search)
        {
            if (search == "")
                search = null;
            ManageViewModel model = new ManageViewModel
            {
                Manages = _context.GetManageTypes(search)
            };
            return View(model);
        }

        [AdminAuthFilter]
        [HttpGet]
        public ActionResult ManageCountries(string search)
        {
            if (search == "")
                search = null;
            ManageViewModel model = new ManageViewModel
            {
                Manages = _context.GetManageCountries(search)
            };
            return View(model);
        }
        [AdminAuthFilter]
        public ActionResult AddCategory()
        {
            return View();
        }

        [AdminAuthFilter]
        [HttpPost]
        public ActionResult SaveCategory(AddCategory model)
        {
            int id = (int)Session["admin"];
            ModelState.Remove("cId");
            if (!ModelState.IsValid)
            {
                return View("AddCategory", model);
            }
            var categoryInDb = _context.GetCategory(model.cId);
            if(categoryInDb == null)
            {
                NoteCategory category = new NoteCategory
                {
                    Name = model.CategoryName,
                    Description = model.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = id,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = id,
                    IsActive = true
                };
                _context.AddCategory(category);
                return RedirectToAction("ManageCategory", "Admin");
            }
            else
            {
                categoryInDb.Name = model.CategoryName;
                categoryInDb.Description = model.Description;
                categoryInDb.ModifiedDate = DateTime.Now;
                categoryInDb.ModifiedBy = id;
                categoryInDb.IsActive = true;
                _context.UpdateUp();
                return RedirectToAction("ManageCategory", "Admin");
            }
            
        }
        [AdminAuthFilter]
        public ActionResult UpdateCategory(int cid)
        {
            var category = _context.GetCategory(cid);
            AddCategory model = new AddCategory
            {
                cId = cid,
                CategoryName = category.Name,
                Description = category.Description
            };

            return View("AddCategory", model);
        }
        [AdminAuthFilter]
        [HttpPost]
        public void DeleteCategory(int id)
        {
            var category = _context.GetCategory(id);
            category.IsActive = false;
            _context.UpdateUp();
        }
        [AdminAuthFilter]
        public ActionResult AddType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveType(AddType model)
        {
            int id = (int)Session["admin"];
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return View("AddType", model);
            }
            var typeInDb = _context.GetType(model.Id);
            if (typeInDb == null)
            {
                NoteType type = new NoteType
                {
                    Name = model.TypeName,
                    Description = model.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = id,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = id,
                    IsActive = true
                };
                _context.AddType(type);
                return RedirectToAction("ManageType", "Admin");
            }
            else
            {
                typeInDb.Name = model.TypeName;
                typeInDb.Description = model.Description;
                typeInDb.ModifiedDate = DateTime.Now;
                typeInDb.ModifiedBy = id;
                typeInDb.IsActive = true;
                _context.UpdateUp();
                return RedirectToAction("ManageType", "Admin");
            }

        }

        [AdminAuthFilter]
        public ActionResult UpdateType(int id)
        {
            var type = _context.GetType(id);
            AddType model = new AddType
            {
                Id = id,
                TypeName = type.Name,
                Description = type.Description
            };
            return View("AddType", model);
        }

        [AdminAuthFilter]
        [HttpPost]
        public void DeleteType(int id)
        {
            var type = _context.GetType(id);
            type.IsActive = false;
            _context.UpdateUp();
        }
        [AdminAuthFilter]
        public ActionResult AddCountries()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveCountry(AddCountries model)
        {
            int id = (int)Session["admin"];
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return View("AddCountries", model);
            }
            var countryInDb = _context.GetCountry(model.Id);
            if (countryInDb == null)
            {
                Country country = new Country
                {
                    Name = model.CountryName,
                    CountryCode = model.CountryCode,
                    CreatedDate = DateTime.Now,
                    CreatedBy = id,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = id,
                    IsActive = true
                };
                _context.AddCountry(country);
                return RedirectToAction("ManageCountries", "Admin");
            }
            else
            {
                countryInDb.Name = model.CountryName;
                countryInDb.CountryCode = model.CountryCode;
                countryInDb.ModifiedDate = DateTime.Now;
                countryInDb.ModifiedBy = id;
                countryInDb.IsActive = true;
                _context.UpdateUp();
                return RedirectToAction("ManageCountries", "Admin");
            }

        }


        [AdminAuthFilter]
        public ActionResult UpdateCountry(int id)
        {
            var country = _context.GetCountry(id);
            AddCountries model = new AddCountries
            {
                Id = id,
                CountryName = country.Name,
                CountryCode = country.CountryCode
            };
            return View("AddCountries", model);
        }

        [AdminAuthFilter]
        [HttpPost]
        public void DeleteCountry(int id)
        {
            var country = _context.GetCountry(id);
            country.IsActive = false;
            _context.UpdateUp();
        }

        [AdminAuthFilter]
        public ActionResult AddAdministrator()
        {
            AddAdministrator admin = new AddAdministrator();
            AddAdministratorViewModel model = new AddAdministratorViewModel
            {
                AddAdmin = admin,
                Countries = _context.GetCountries()
            };
            return View(model);
        }

        [AdminAuthFilter]
        [HttpPost]
        public ActionResult SaveAdmin(AddAdministratorViewModel admin)
        {
            int id = (int)Session["super admin"];
           
            if (!ModelState.IsValid)
            {  
                admin.Countries = _context.GetCountries();
                return View("AddAdministrator", admin);
            }
            var adminInDb = _context.GetAdmin(admin.AddAdmin.Id);

            if(adminInDb == null)
            {
                var IsExist = _context.CheckUserIsExistOrNot(admin.AddAdmin.Email);
                if (IsExist)
                {
                    ModelState.AddModelError("Exist", "Email is Already Exists.");
                    admin.Countries = _context.GetCountries();
                    return View("AddAdministrator", admin);
                }
                User addAdmin = new User
                {
                    RoleID = _context.GetRolesByName("admin"),
                    FirstName = admin.AddAdmin.FirstName,
                    LastName = admin.AddAdmin.LastName,
                    EmailID = admin.AddAdmin.Email,
                    Password = "Admin@123",
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = id,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = id,
                    IsActive = true
                };
                int adminId = _context.AddUser(addAdmin);

                UserProfile adminDetails = new UserProfile
                {
                    UserID = adminId,
                    PhoneNumberCounrtyCode = admin.AddAdmin.CountryCodeId,
                    PhoneNumber = admin.AddAdmin.PhoneNumber,
                    AddressLine1 = "Admin",
                    AddressLine2 = "Admin",
                    City = "Admin",
                    State = "Admin",
                    ZipCode = "Admin",
                    Country = "Admin",
                };
                _context.AddUserDetails(adminDetails);

                return RedirectToAction("ManageAdministrator", "Admin");
            }
            else
            {
                adminInDb.FirstName = admin.AddAdmin.FirstName;
                adminInDb.LastName = admin.AddAdmin.LastName;
                adminInDb.EmailID = admin.AddAdmin.Email;
                adminInDb.IsActive = true;
                var adminDetails = _context.GetUserProfileDetails(adminInDb.ID);
                adminDetails.PhoneNumber = admin.AddAdmin.PhoneNumber;
                adminDetails.PhoneNumberCounrtyCode = admin.AddAdmin.CountryCodeId;

                _context.UpdateUp();

                return RedirectToAction("ManageAdministrator", "Admin");
            }
           
        }

        public ActionResult UpdateAdmin(int id)
        {
            var admin = _context.GetAdmin(id);
            var adminDetails = _context.GetUserProfileDetails(admin.ID);
            AddAdministratorViewModel model = new AddAdministratorViewModel
            {
                AddAdmin = new AddAdministrator(),
                Countries = _context.GetCountries()
            };
            model.AddAdmin.Id = admin.ID;
            model.AddAdmin.FirstName = admin.FirstName;
            model.AddAdmin.LastName = admin.LastName;
            model.AddAdmin.Email = admin.EmailID;
            model.AddAdmin.PhoneNumber = adminDetails.PhoneNumber;
            model.AddAdmin.CountryCodeId = adminDetails.PhoneNumberCounrtyCode;
            return View("AddAdministrator", model);
        }

        [AdminAuthFilter]
        [HttpPost]
        public void DeleteAdmin(int id)
        {
            var admin = _context.GetAdmin(id);
            admin.IsActive = false;
            _context.UpdateUp();
        }

        [AdminAuthFilter]
        public ActionResult UpdateProfile()
        {
            int id = (int)Session["admin"];

            var admin = _context.GetAdmin(id);
            var adminDetails = _context.GetUserProfileDetails(id);

            MyProfile Adminmodel = new MyProfile
            {
                Id = admin.ID,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.EmailID,
                PhoneNumber = adminDetails.PhoneNumber,
                CountryCodeId = adminDetails.PhoneNumberCounrtyCode
            };

            MyProfileViewModel model = new MyProfileViewModel
            {
                Admin = Adminmodel,
                Countries = _context.GetCountries()
            };

            return View(model);
        }

        [AdminAuthFilter]
        [HttpPost]
        public ActionResult UpdateAdminProfile(MyProfile admin)
        {
            if (!ModelState.IsValid)
            {
                MyProfileViewModel model = new MyProfileViewModel
                {
                    Admin = admin,
                    Countries = _context.GetCountries()
                };

                return View("UpdateProfile", model);
            }

            var adminInDb = _context.GetAdmin(admin.Id);
            var adminDetails = _context.GetUserProfileDetails(admin.Id);

           

            adminInDb.FirstName = admin.FirstName;
            adminInDb.LastName = admin.LastName;
            adminDetails.PhoneNumber = admin.PhoneNumber;
            adminDetails.PhoneNumberCounrtyCode = admin.CountryCodeId;
            adminDetails.SecondaryEmailAddress = admin.SecondaryEmail;

            if (admin.AdminImage != null)
            {
                string pic = Path.GetExtension(admin.AdminImage.FileName);
                if (pic.ToLower() != ".jpg" && pic.ToLower() != ".jpeg")
                {
                    MyProfileViewModel model = new MyProfileViewModel
                    {
                        Admin = admin,
                        Countries = _context.GetCountries()
                    };
                    ModelState.AddModelError("img format", "Please upload .jpg or .jpeg file.");
                    return View("UpdateProfile", model);
                }

                if (!Directory.Exists("~/Members/" + admin.Id))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/Members/" + admin.Id)));
                    if (admin.AdminImage != null && admin.AdminImage.ContentLength > 0)
                    {
                        if(adminDetails.ProfilePicture !=null)
                        {
                            string dp = Path.Combine(Server.MapPath("~/Members/" + admin.Id), adminDetails.ProfilePicture);
                            FileInfo file = new FileInfo(dp);
                            if (file.Exists)
                                file.Delete();
                        }

                        var imgName = Path.GetFileName(admin.AdminImage.FileName);
                        adminDetails.ProfilePicture = imgName;

                        string path = Path.Combine(Server.MapPath("~/Members/" + admin.Id), imgName);
                        Session["AdminImg"] = imgName;
                        admin.AdminImage.SaveAs(path);
                    }
                }
                else
                {
                    if (admin.AdminImage != null && admin.AdminImage.ContentLength > 0)
                    {
                        if (adminDetails.ProfilePicture != null)
                        {
                            string dp1 = Path.Combine(Server.MapPath("~/Members/" + admin.Id), adminDetails.ProfilePicture);
                            FileInfo file1 = new FileInfo(dp1);
                            if (file1.Exists)
                                file1.Delete();
                        }

                        string dp = Path.Combine(Server.MapPath("~/Members/" + admin.Id), adminDetails.ProfilePicture);
                        FileInfo file = new FileInfo(dp);
                        if (file.Exists)
                            file.Delete();

                        var imgName = Path.GetFileName(admin.AdminImage.FileName);
                        string path = Path.Combine(Server.MapPath("~/Members/" + admin.Id), imgName);
                        Session["AdminImg"] = imgName;
                        admin.AdminImage.SaveAs(path);
                    }
                }

            }
            _context.UpdateUp();

            return RedirectToAction("Dashboard", "Admin");
        }
    }
}