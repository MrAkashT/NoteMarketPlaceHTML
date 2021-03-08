
using Newtonsoft.Json;
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
    public class UserController : Controller
    {
        private MyDbContext userRepo;
        public UserController()
        {
            userRepo = new MyDbContext();
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [UserAuthFilter]
        public ActionResult UserProfile()
        {
         
            int id = (int)Session["userId"];
            var LoginUser = userRepo.GetUser(id);

            var userProfile = userRepo.GetUserProfileDetails(id);

            if (userProfile == null)
            {
                var countries = userRepo.GetCountries();
                var gender = userRepo.GetGender();
                var userdetail = new UserProfileDetails
                {
                    UserId = id,
                    FirstName = LoginUser.FirstName,
                    LastName = LoginUser.LastName,
                    Email = LoginUser.EmailID
                };
                UserProfileViewModel user = new UserProfileViewModel
                {
                    User = userdetail,
                    Countries = countries,
                    Gender = gender
                };
                return View(user);
            }
            else
            {
                var countries = userRepo.GetCountries();
                var gender = userRepo.GetGender();
                Country country = userRepo.FindCountry(userProfile.Country);
                var userDetails = new UserProfileDetails
                {
                    UserId = id,
                    FirstName = LoginUser.FirstName,
                    LastName = LoginUser.LastName,
                    Email = LoginUser.EmailID,
                    DateOfbirth = userProfile.DOB,
                    GenderID = userProfile.Gender,
                    CountryCodeId = userProfile.PhoneNumberCounrtyCode,
                    PhoneNo = userProfile.PhoneNumber,
                    Address1 = userProfile.AddressLine1,
                    Address2 = userProfile.AddressLine2,
                    City = userProfile.City,
                    State = userProfile.State,
                    ZipCode = userProfile.ZipCode,
                    CountryId = country.ID,
                    University = userProfile.University,
                    College = userProfile.College
                };
                UserProfileViewModel user = new UserProfileViewModel
                {
                    User = userDetails,
                    Countries = countries,
                    Gender = gender
                };
                return View(user);
            }

            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserProfileDetails user, HttpPostedFileBase ProfilePic)
        {
            int id = (int)Session["userId"];
            if (!ModelState.IsValid)
            {
                var countries = userRepo.GetCountries();
                var gender = userRepo.GetGender();
                UserProfileViewModel userViewModel = new UserProfileViewModel
                {
                    User = user,
                    Countries = countries,
                    Gender = gender
                };
                return View("UserProfile", userViewModel);
            }
            var getUserProfile = userRepo.GetUserProfileDetails(user.UserId);

            var country = userRepo.FindCountryById((int)user.CountryId);

            if (ProfilePic != null)
            {
                user.ProfilePicture = "DP_" + Path.GetFileName(ProfilePic.FileName);
            }
            if (getUserProfile == null)
            {
             
                if(!Directory.Exists("~/Members/" + id))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/Members/" + id)));

                    if(ProfilePic != null && ProfilePic.ContentLength > 0)
                    {
                        user.ProfilePicture = Path.GetFileName(ProfilePic.FileName);
                        string path = Path.Combine(Server.MapPath("~/Members/"+id), user.ProfilePicture);
                        Session["img"] = user.ProfilePicture;
                        ProfilePic.SaveAs(path);
                    }
                }
                else
                {
                    if (ProfilePic != null && ProfilePic.ContentLength > 0)
                    {
                        user.ProfilePicture = Path.GetFileName(ProfilePic.FileName);
                        string path = Path.Combine(Server.MapPath("~/Members/" + id), user.ProfilePicture);
                        Session["img"] = user.ProfilePicture;
                        ProfilePic.SaveAs(path);
                    }
                }

                UserProfile userProfile = new UserProfile
                {
                    UserID = id,
                    DOB = user.DateOfbirth,
                    Gender = user.GenderID,
                    ProfilePicture = user.ProfilePicture,
                    PhoneNumberCounrtyCode = user.CountryCodeId,
                    PhoneNumber = user.PhoneNo,
                    AddressLine1 = user.Address1,
                    AddressLine2 = user.Address2,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    Country = country.Name,
                    University = user.University,
                    College = user.College
                };
                Session["profileId"] = userRepo.AddUserDetails(userProfile);
                return RedirectToAction("SearchNotes", "Home");
            }
            else
            {
                var loginuser = userRepo.GetUser(getUserProfile.UserID);

                if (!Directory.Exists("~/Members/" + id))
                {
                    Directory.CreateDirectory(Server.MapPath(string.Format("~/Members/" + id)));

                    if (ProfilePic != null && ProfilePic.ContentLength > 0)
                    {
                        if(getUserProfile.ProfilePicture != null)
                        {
                            string dp = Path.Combine(Server.MapPath("~/Members/" + id), getUserProfile.ProfilePicture);
                            FileInfo file = new FileInfo(dp);
                            if (file.Exists)
                                file.Delete();

                            string addDp = Path.Combine(Server.MapPath("~/Members/" + id), user.ProfilePicture);
                            Session["img"] = user.ProfilePicture;
                            ProfilePic.SaveAs(addDp);
                        }
                        else
                        {
                            Session["img"] = user.ProfilePicture;
                            string addDp = Path.Combine(Server.MapPath("~/Members/" + id), user.ProfilePicture);
                            ProfilePic.SaveAs(addDp);
                        } 
                    }
                }
                else
                {
                    if (ProfilePic != null && ProfilePic.ContentLength > 0)
                    {
                        if (getUserProfile.ProfilePicture != null)
                        {
                            string dp = Path.Combine(Server.MapPath("~/Members/" + id), getUserProfile.ProfilePicture);
                            FileInfo file = new FileInfo(dp);
                            if (file.Exists)
                                file.Delete();

                            Session["img"] = user.ProfilePicture;
                            string addDp = Path.Combine(Server.MapPath("~/Members/" + id), user.ProfilePicture);
                            ProfilePic.SaveAs(addDp);
                        }
                        else
                        {
                            Session["img"] = user.ProfilePicture;
                            string addDp = Path.Combine(Server.MapPath("~/Members/" + id), user.ProfilePicture);
                            ProfilePic.SaveAs(addDp);
                        }

                    }
                    
                }

               
                loginuser.FirstName = user.FirstName;
                loginuser.LastName = user.LastName;
                loginuser.EmailID = user.Email;
                getUserProfile.DOB = user.DateOfbirth;
                getUserProfile.Gender = user.GenderID;
                getUserProfile.PhoneNumber = user.PhoneNo;
                getUserProfile.AddressLine1 = user.Address1;
                getUserProfile.AddressLine2 = user.Address2;
                getUserProfile.City = user.City;
                getUserProfile.State = user.State;
                getUserProfile.ProfilePicture = user.ProfilePicture;
                getUserProfile.ZipCode = user.ZipCode;
                getUserProfile.Country = country.Name;
                getUserProfile.PhoneNumberCounrtyCode = user.CountryCodeId;
                getUserProfile.University = user.University;
                getUserProfile.College = user.College;
                userRepo.UpdateUserProfile();
                return RedirectToAction("SearchNotes", "Home");
               
            }


        }

        [UserAuthFilter]
        public ActionResult Dashboard()
        {
            int id = (int)Session["userId"];
            var sellerNotes = userRepo.GetSellerDraftNoteBySellerId(id);
            var publishednote = userRepo.GetSellerPublishedNoteBySellerId(id);
          
            BuyerReqViewModel model = new BuyerReqViewModel
            {
                BuyerReq = userRepo.GetBuyerReqData(id)
            };
            int count = model.BuyerReq.Count();
            DashBoardViewModel AllNotes = new DashBoardViewModel
            {
                Notes = sellerNotes,
                publishedNotes = publishednote,
                BuyerCount = count
            };
            foreach (var note in AllNotes.publishedNotes)
            {
                if (note.IsPaid)
                {
                    note.SellType = "Paid";
                }
                else
                {
                    note.SellType = "Free";
                }
            }
          
            return View(AllNotes);
        }

        [UserAuthFilter]
        [HttpPost]
        public ActionResult Dashboard(DashBoardViewModel m)
        {
            int id = (int)Session["userId"];
            var sellerNotes = userRepo.GetSellerDraftNoteBySellerId(id);
            var publishednote = userRepo.GetSellerPublishedNoteBySellerId(id);
            if(Request.Form["Name"] == "Inprogress")
            {
                if(m.SearchPublishNote == null)
                {
                    DashBoardViewModel model = new DashBoardViewModel
                    {
                        Notes = userRepo.GetSearchedDraftNotes(m.SearchDraftNote, id),
                        publishedNotes = publishednote,
                        SearchDraftNote = m.SearchDraftNote
                    };

                    return View(model);
                    
                }
                else
                {
                    DashBoardViewModel model = new DashBoardViewModel
                    {
                        Notes = userRepo.GetSearchedDraftNotes(m.SearchDraftNote, id),
                        publishedNotes = userRepo.GetSearchedPublishedNotes(m.SearchPublishNote, id),
                        SearchDraftNote = m.SearchDraftNote,
                        SearchPublishNote = m.SearchPublishNote
                    };
                    return View(model);
                }
            }
            else
            {
                if(m.SearchDraftNote == null)
                {
                    DashBoardViewModel model = new DashBoardViewModel
                    {
                        Notes = sellerNotes,
                        publishedNotes = userRepo.GetSearchedPublishedNotes(m.SearchPublishNote, id),
                        SearchPublishNote = m.SearchPublishNote
                    };
                    foreach (var note in model.publishedNotes)
                    {
                        if (note.IsPaid)
                        {
                            note.SellType = "Paid";
                        }
                        else
                        {
                            note.SellType = "Free";
                        }
                    }
                    return View(model);
                }
                else
                {
                    DashBoardViewModel model = new DashBoardViewModel
                    {
                        Notes = userRepo.GetSearchedDraftNotes(m.SearchDraftNote, id),
                        publishedNotes = userRepo.GetSearchedPublishedNotes(m.SearchPublishNote, id),
                        SearchDraftNote = m.SearchDraftNote,
                        SearchPublishNote = m.SearchPublishNote
                    };
                    foreach (var note in model.publishedNotes)
                    {
                        if (note.IsPaid)
                        {
                            note.SellType = "Paid";
                        }
                        else
                        {
                            note.SellType = "Free";
                        }
                    }
                    return View(model);
                }
            }
            

        }

        [UserAuthFilter]
        public ActionResult AddNote()
        {
            NoteDetails NoteDetailsModel = new NoteDetails();
            NoteDetailsViewModel NoteDetails = new NoteDetailsViewModel
            {
                NoteDetails = NoteDetailsModel,
                Countries = userRepo.GetCountries(),
                Types = userRepo.GetTypes(),
                Categories = userRepo.GetCategories(),
                SellingMode = userRepo.GetSellingModes()
            };

            return View(NoteDetails);
        }


        [UserAuthFilter]
        
        public ActionResult UpdateNote(int noteId)
        {
            SellerNote noteInDb = userRepo.GetSellerNoteByNoteId(noteId);

            var noteSellForId = userRepo.GetIdSellFor(noteInDb);

            NoteDetails NoteDetailsModel = new NoteDetails
            {
                SellerId = noteInDb.SellerID,
                NoteId = noteInDb.ID,
                Title = noteInDb.Title,
                CategoryId = noteInDb.Category,
                DisplayPicture = noteInDb.DisplayPicture,
                NoteTypeId = noteInDb.NoteType,
                NumberOfPages = noteInDb.NumberOfPages,
                Description = noteInDb.Description,
                InstituteName = noteInDb.UniversityName,
                CountryId = noteInDb.Country,
                CourseName = noteInDb.Course,
                CourseCode = noteInDb.CourseCode,
                Professor = noteInDb.Professor,
                SellFor = noteSellForId,
                SellPrice = noteInDb.SellingPrice,
                NotePreview = noteInDb.NotesPreview
            };

            NoteDetailsViewModel NoteDetails = new NoteDetailsViewModel
            {
                NoteDetails = NoteDetailsModel,
                Countries = userRepo.GetCountries(),
                Types = userRepo.GetTypes(),
                Categories = userRepo.GetCategories(),
                SellingMode = userRepo.GetSellingModes()
            };

            ViewBag.publish = "publish";

            return View("Addnote", NoteDetails);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthFilter]
        public ActionResult SaveNote(NoteDetails noteDetails,string publish, HttpPostedFileBase DisplayPic, HttpPostedFileBase NotePdf, HttpPostedFileBase PreviewNote)
        {
            if (!ModelState.IsValid)
            {
                NoteDetailsViewModel model = new NoteDetailsViewModel
                {
                    NoteDetails = noteDetails,
                    Categories = userRepo.GetCategories(),
                    Countries = userRepo.GetCountries(),
                    SellingMode = userRepo.GetSellingModes(),
                    Types = userRepo.GetTypes()
                };
                if(publish != null)
                {
                    ViewBag.publish = "publish";
                }
                return View("AddNote", model);
            }

            if(publish == null)
            {
                int id = (int)Session["userId"];
                var SellerNote = userRepo.GetSellerNoteByNoteId(noteDetails.NoteId);
                noteDetails.isPaid = userRepo.GetIsPaidStatus(noteDetails.SellFor);
                noteDetails.Status = userRepo.GetStatusId("Draft");

                if (DisplayPic != null)
                {
                    noteDetails.ForDisplay = "DP_" + Path.GetFileName(DisplayPic.FileName);
                }

                if (noteDetails.NotePdf != null)
                {
                    noteDetails.AttachmentPdf = "Attachment_" + Path.GetFileName(noteDetails.NotePdf.FileName);
                }

                if (PreviewNote != null)
                {
                    noteDetails.ForPreview = "Preview_" + Path.GetFileName(PreviewNote.FileName);
                }

                if (SellerNote == null)
                {
                    try
                    {
                        //insert
                        SellerNote NoteDetails = new SellerNote
                        {
                            SellerID = id,
                            Status = noteDetails.Status,
                            Title = noteDetails.Title,
                            Category = noteDetails.CategoryId,
                            DisplayPicture = noteDetails.ForDisplay,
                            NoteType = noteDetails.NoteTypeId,
                            NumberOfPages = noteDetails.NumberOfPages,
                            Description = noteDetails.Description,
                            UniversityName = noteDetails.InstituteName,
                            Country = noteDetails.CountryId,
                            Course = noteDetails.CourseName,
                            CourseCode = noteDetails.CourseCode,
                            Professor = noteDetails.Professor,
                            IsPaid = noteDetails.isPaid,
                            SellingPrice = noteDetails.SellPrice,
                            NotesPreview = noteDetails.ForPreview,
                            CreatedDate = DateTime.Now,
                            CreatedBy = id,
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = id,
                            IsActive = true
                        };

                        noteDetails.NoteId = userRepo.AddSellerNote(NoteDetails);

                        if (!Directory.Exists(Server.MapPath("~/Members/" + id + "/" + noteDetails.NoteId + "/Attachments")))
                        {
                            Directory.CreateDirectory(Server.MapPath(string.Format("~/Members/" + id + "/" + noteDetails.NoteId + "/Attachments")));

                            if (noteDetails.NotePdf != null && noteDetails.NotePdf.ContentLength > 0)
                            {

                                string attachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + noteDetails.NoteId + "/Attachments"), noteDetails.AttachmentPdf);

                                noteDetails.NotePdf.SaveAs(attachment);

                                SellerNotesAttachment sellerNotesAttachment = new SellerNotesAttachment
                                {
                                    NoteID = noteDetails.NoteId,
                                    FileName = noteDetails.AttachmentPdf,
                                    FilePath = attachment,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = id,
                                    ModifiedDate = DateTime.Now,
                                    ModifiedBy = id,
                                    IsActive = true
                                };
                                userRepo.AddSellerNoteAttachment(sellerNotesAttachment);

                            }

                            if (DisplayPic != null && DisplayPic.ContentLength > 0)
                            {


                                string dp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + noteDetails.NoteId), noteDetails.ForDisplay);

                                DisplayPic.SaveAs(dp);


                            }

                            if (PreviewNote != null && PreviewNote.ContentLength > 0)
                            {

                                string preview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + noteDetails.NoteId), noteDetails.ForPreview);

                                PreviewNote.SaveAs(preview);

                            }
                        }
                        int noteIdd = noteDetails.NoteId;
                        ViewBag.Message = "success";
                        
                        return RedirectToAction("Dashboard", "User");
                       
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = e;
                        return View("Save");
                    }
                }
                else
                {
                    if (Directory.Exists(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID)))
                    {
                        if (DisplayPic != null && DisplayPic.ContentLength > 0)
                        {
                            if (SellerNote.DisplayPicture != null)
                            {
                                string dp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), SellerNote.DisplayPicture);
                                FileInfo file = new FileInfo(dp);
                                if (file.Exists)
                                    file.Delete();

                                string AddDp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForDisplay);
                                DisplayPic.SaveAs(AddDp);
                            }
                            else
                            {
                                string AddDp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForDisplay);
                                DisplayPic.SaveAs(AddDp);
                            }
                        }
                        if (noteDetails.NotePdf != null && noteDetails.NotePdf.ContentLength > 0)
                        {
                            var attachment = userRepo.GetAttachmentByNoteId(SellerNote.ID);
                            if (attachment.FileName != null)
                            {
                                string attachmentPath = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), attachment.FileName);
                                FileInfo file = new FileInfo(attachmentPath);
                                if (file.Exists)
                                    file.Delete();
                                string addAttachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), noteDetails.AttachmentPdf);
                                noteDetails.NotePdf.SaveAs(addAttachment);

                                attachment.FileName = noteDetails.AttachmentPdf;
                                attachment.FilePath = addAttachment;
                                attachment.CreatedDate = DateTime.Now;
                                attachment.CreatedBy = id;
                                attachment.ModifiedDate = DateTime.Now;
                                attachment.ModifiedBy = id;
                            }
                            else
                            {
                                string addAttachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), noteDetails.AttachmentPdf);
                                noteDetails.NotePdf.SaveAs(addAttachment);

                                attachment.NoteID = SellerNote.ID;
                                attachment.FileName = noteDetails.AttachmentPdf;
                                attachment.FilePath = addAttachment;
                            }
                        }
                        if (PreviewNote != null && PreviewNote.ContentLength > 0)
                        {
                            if (SellerNote.NotesPreview != null)
                            {
                                string preview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), SellerNote.NotesPreview);
                                FileInfo file = new FileInfo(preview);
                                if (file.Exists)
                                    file.Delete();

                                string AddPreview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForPreview);
                                PreviewNote.SaveAs(AddPreview);
                            }
                            else
                            {
                                string AddPreview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForPreview);
                                PreviewNote.SaveAs(AddPreview);
                            }

                        }
                    }
                    else
                    {
                        //create directory
                        Directory.CreateDirectory(Server.MapPath(string.Format("~/Members/" + id + "/" + SellerNote.ID + "/Attachments")));
                        if (DisplayPic != null && DisplayPic.ContentLength > 0)
                        {
                            string AddDp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForDisplay);
                            DisplayPic.SaveAs(AddDp);
                        }
                        if (noteDetails.NotePdf != null && noteDetails.NotePdf.ContentLength > 0)
                        {
                            var attachment = userRepo.GetAttachmentByNoteId(SellerNote.ID);

                            string addAttachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), noteDetails.AttachmentPdf);
                            noteDetails.NotePdf.SaveAs(addAttachment);

                            attachment.NoteID = SellerNote.ID;
                            attachment.FileName = noteDetails.AttachmentPdf;
                            attachment.FilePath = addAttachment;
                        }
                        if (PreviewNote != null && PreviewNote.ContentLength > 0)
                        {
                            string AddPreview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForPreview);
                            PreviewNote.SaveAs(AddPreview);
                        }
                    }

                    SellerNote.Title = noteDetails.Title;
                    SellerNote.Category = noteDetails.CategoryId;
                    SellerNote.DisplayPicture = noteDetails.ForDisplay;
                    SellerNote.NoteType = noteDetails.NoteTypeId;
                    SellerNote.NumberOfPages = noteDetails.NumberOfPages;
                    SellerNote.Description = noteDetails.Description;
                    SellerNote.Country = noteDetails.CountryId;
                    SellerNote.Course = noteDetails.CourseName;
                    SellerNote.CourseCode = noteDetails.CourseCode;
                    SellerNote.Professor = noteDetails.Professor;
                    SellerNote.IsPaid = noteDetails.isPaid;
                    SellerNote.SellingPrice = noteDetails.SellPrice;
                    SellerNote.NotesPreview = noteDetails.ForPreview;
                    SellerNote.CreatedDate = DateTime.Now;
                    SellerNote.CreatedBy = id;
                    SellerNote.ModifiedDate = DateTime.Now;
                    SellerNote.ModifiedBy = id;
                    SellerNote.IsActive = true;

                    userRepo.UpdateUp();

                    return RedirectToAction("Dashboard", "User");
                }
            }
            else
            {
                //This is For Publish Note Button

                int id = (int)Session["userId"];
                var SellerNote = userRepo.GetSellerNoteByNoteId(noteDetails.NoteId);
                noteDetails.isPaid = userRepo.GetIsPaidStatus(noteDetails.SellFor);
                noteDetails.Status = userRepo.GetStatusId("Submitted For Review");

                if (DisplayPic != null)
                {
                    noteDetails.ForDisplay = "DP_" + Path.GetFileName(DisplayPic.FileName);
                }

                if (noteDetails.NotePdf != null)
                {
                    noteDetails.AttachmentPdf = "Attachment_" + Path.GetFileName(noteDetails.NotePdf.FileName);
                }

                if (PreviewNote != null)
                {
                    noteDetails.ForPreview = "Preview_" + Path.GetFileName(PreviewNote.FileName);
                }

                 // attachmnt and photos is store here

                    if (Directory.Exists(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments")))
                    {
                        if (DisplayPic != null && DisplayPic.ContentLength > 0)
                        {
                            if (SellerNote.DisplayPicture != null)
                            {
                                string dp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), SellerNote.DisplayPicture);
                                FileInfo file = new FileInfo(dp);
                                if (file.Exists)
                                    file.Delete();

                                string AddDp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForDisplay);
                                DisplayPic.SaveAs(AddDp);
                            }
                            else
                            {
                                string AddDp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForDisplay);
                                DisplayPic.SaveAs(AddDp);
                            }
                        }
                        if (noteDetails.NotePdf != null && noteDetails.NotePdf.ContentLength > 0)
                        {
                            var attachment = userRepo.GetAttachmentByNoteId(SellerNote.ID);
                            if (attachment.FileName != null)
                            {
                                string attachmentPath = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), attachment.FileName);
                                FileInfo file = new FileInfo(attachmentPath);
                                if (file.Exists)
                                    file.Delete();
                                string addAttachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), noteDetails.AttachmentPdf);
                                noteDetails.NotePdf.SaveAs(addAttachment);

                                attachment.FileName = noteDetails.AttachmentPdf;
                                attachment.FilePath = addAttachment;
                                attachment.CreatedDate = DateTime.Now;
                                attachment.CreatedBy = id;
                                attachment.ModifiedDate = DateTime.Now;
                                attachment.ModifiedBy = id;
                            }
                            else
                            {
                                string addAttachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), noteDetails.AttachmentPdf);
                                noteDetails.NotePdf.SaveAs(addAttachment);

                                attachment.NoteID = SellerNote.ID;
                                attachment.FileName = noteDetails.AttachmentPdf;
                                attachment.FilePath = addAttachment;
                            }
                        }
                        if (PreviewNote != null && PreviewNote.ContentLength > 0)
                        {
                            if (SellerNote.NotesPreview != null)
                            {
                                string preview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), SellerNote.NotesPreview);
                                FileInfo file = new FileInfo(preview);
                                if (file.Exists)
                                    file.Delete();

                                string AddPreview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForPreview);
                                PreviewNote.SaveAs(AddPreview);
                            }
                            else
                            {
                                string AddPreview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForPreview);
                                PreviewNote.SaveAs(AddPreview);
                            }

                        }
                    }
                    else
                    {
                        //create directory
                        Directory.CreateDirectory(Server.MapPath(string.Format("~/Members/" + id + "/" + SellerNote.ID + "/Attachments")));
                        if (DisplayPic != null && DisplayPic.ContentLength > 0)
                        {
                            string AddDp = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForDisplay);
                            DisplayPic.SaveAs(AddDp);
                        }
                        if (noteDetails.NotePdf != null && noteDetails.NotePdf.ContentLength > 0)
                        {
                            var attachment = userRepo.GetAttachmentByNoteId(SellerNote.ID);

                            string addAttachment = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID + "/Attachments"), noteDetails.AttachmentPdf);
                            noteDetails.NotePdf.SaveAs(addAttachment);

                            attachment.NoteID = SellerNote.ID;
                            attachment.FileName = noteDetails.AttachmentPdf;
                            attachment.FilePath = addAttachment;
                        }
                        if (PreviewNote != null && PreviewNote.ContentLength > 0)
                        {
                            string AddPreview = Path.Combine(Server.MapPath("~/Members/" + id + "/" + SellerNote.ID), noteDetails.ForPreview);
                            PreviewNote.SaveAs(AddPreview);
                        }
                    }

                    //update note when publish calledd
                   
                    SellerNote.Status = noteDetails.Status;
                    SellerNote.Title = noteDetails.Title;
                    SellerNote.Category = noteDetails.CategoryId;
                    SellerNote.DisplayPicture = noteDetails.ForDisplay;
                    SellerNote.NoteType = noteDetails.NoteTypeId;
                    SellerNote.NumberOfPages = noteDetails.NumberOfPages;
                    SellerNote.Description = noteDetails.Description;
                    SellerNote.Country = noteDetails.CountryId;
                    SellerNote.Course = noteDetails.CourseName;
                    SellerNote.CourseCode = noteDetails.CourseCode;
                    SellerNote.Professor = noteDetails.Professor;
                    SellerNote.IsPaid = noteDetails.isPaid;
                    SellerNote.SellingPrice = noteDetails.SellPrice;
                    SellerNote.NotesPreview = noteDetails.ForPreview;
                    SellerNote.CreatedDate = DateTime.Now;
                    SellerNote.CreatedBy = id;
                    SellerNote.ModifiedDate = DateTime.Now;
                    SellerNote.ModifiedBy = id;
                    SellerNote.IsActive = true;

                    userRepo.UpdateUp();

                    return RedirectToAction("Dashboard", "User");
                

            }

        }

        [UserAuthFilter]
        [HttpPost]
        public void DeleteDraftNote(int id)
        {

            var DraftNote = userRepo.GetSellerNoteByNoteId(id);

            if (DraftNote.DisplayPicture != null)
            {
                string DisplayPic = Path.Combine(Server.MapPath("~/Members/" + DraftNote.SellerID + "/" + id), DraftNote.DisplayPicture);
                FileInfo file1 = new FileInfo(DisplayPic);
                if (file1.Exists)
                    file1.Delete();
            }

            if (DraftNote.NotesPreview != null)
            {
                string Preview = Path.Combine(Server.MapPath("~/Members/" + DraftNote.SellerID + "/" + id), DraftNote.NotesPreview);
                FileInfo file2 = new FileInfo(Preview);
                if (file2.Exists)
                    file2.Delete();
            }

            var attachment = userRepo.GetAttachmentByNoteId(id);
            if (attachment != null && attachment.FileName != null)
            {
                string attachmentPath = Path.Combine(Server.MapPath("~/Members/" + DraftNote.SellerID + "/" + id + "/Attachments"), attachment.FileName);
                FileInfo file3 = new FileInfo(attachmentPath);
                if (file3.Exists)
                    file3.Delete();
            }

            userRepo.DeleteDraftNoteFromDb(id);
        }

        public ActionResult NoteDetails()
        {
            return View();
        }
        public ActionResult ViewNote(int id)
        {
            int uid = (int)Session["userId"];
            SingleNoteDetail noteModel = userRepo.GetSingleNoteDetail(id);
            var country = userRepo.GetCountryNameById(id);
            noteModel.BuyerName = userRepo.GetUserNameById(uid);
            noteModel.Country = country;
            return View("NoteDetails", noteModel);
        }

        [HttpPost]
        public ActionResult FilterNote(string searchVal, int CategoryId, int TypeId, string University, string Course, int CountryId)
        {
            var notes = userRepo.GetNotesByFilter(searchVal, CategoryId, TypeId, University, Course, CountryId);
            return Json(notes, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [UserAuthFilter]
        public ActionResult SendMailToAdmin(int id)
        {
            int downloaderId = (int)Session["userId"];
            var downloader = userRepo.GetUser(downloaderId);
            var note = userRepo.GetSellerNoteByNoteId(id);
            var seller = userRepo.GetUser(note.SellerID);
            string categoryName = userRepo.GetCategoryNameById(note.Category);
            Download buyerReq = new Download
            {
                NoteID = note.ID,
                Seller = note.SellerID,
                Downloader = downloaderId,
                IsSellerasAllowedDownload = false,
                IsAttachmentDownload = false,
                IsPaid = note.IsPaid,
                PurchasedPrice = note.SellingPrice,
                NoteTitle = note.Title,
                NoteCategory = categoryName,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            userRepo.AddDownload(buyerReq);

            var fromEmail = new MailAddress("akashthakar008@gmail.com", "NoteMarketPlace");
            var toEmail = new MailAddress(seller.EmailID);
            var fromEmailPassword = "8460566920";
            string sub = downloader.FirstName + " wants to purchase your notes";
            string body = "Hello " + seller.FirstName + ",<br/><br/>" +
                "We would like to inform you that, <Buyer name> wants to purchase your notes. Please see" +
                "Buyer Requests tab and allow download access to Buyer if you have received the payment from him  < br/><br/>" +
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

            return Json(note.ID, JsonRequestBehavior.AllowGet);
        }

        [UserAuthFilter]
        public ActionResult BuyerRequest()
        {
            int id = (int)Session["userId"];
            BuyerReqViewModel model = new BuyerReqViewModel
            {
                BuyerReq = userRepo.GetBuyerReqData(id)
            };
            foreach (var note in model.BuyerReq)
            {
                if (note.isPaid)
                {
                    note.SellType = "Paid";
                }
                else
                {
                    note.SellType = "Free";
                }
            }
            return View(model);
        }

      
        public ActionResult DownloadFile(int id)
        {
            var note = userRepo.GetSellerNoteByNoteId(id);
            var attachment = userRepo.GetAttachmentByNoteId(id);
            string fullName = attachment.FilePath;
           
            int downloaderId = (int)Session["userId"];
           
            string categoryName = userRepo.GetCategoryNameById(note.Category);
            Download buyerReq = new Download
            {
                NoteID = note.ID,
                Seller = note.SellerID,
                Downloader = downloaderId,
                IsSellerasAllowedDownload = true,
                IsAttachmentDownload = true,
                IsPaid = note.IsPaid,
                PurchasedPrice = note.SellingPrice,
                NoteTitle = note.Title,
                NoteCategory = categoryName,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            userRepo.AddDownload(buyerReq);
            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.FileName);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}
