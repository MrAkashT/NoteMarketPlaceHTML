using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteMarketPlace;
using NoteMarketPlace.Models;

namespace NoteMarketPlace.DbContext
{
    public class MyDbContext : DBContext
    {

        public int AddUser(User user)
        {
            Db.Users.Add(user);
            Db.SaveChanges();
            return user.ID;
        }

        public void Verify(int id)
        {
            var userInDb = Db.Users.SingleOrDefault(u => u.ID == id && u.IsActive==true);
            userInDb.IsEmailVerified = true;
            Db.SaveChanges();
        }
        public User GetUser(string email)
        {
            var user = Db.Users.Where(u => u.EmailID == email && u.IsActive==true).SingleOrDefault();
            return user;
        }
        public UserProfile GetUserProfileInfo(int id)
        {
            return Db.UserProfiles.SingleOrDefault(u => u.UserID == id);
        }
        public bool CheckUserIsExistOrNot(string email)
        {
            var user = Db.Users.SingleOrDefault(u => u.EmailID == email && u.IsActive == true);
            if (user != null)
                return true;
            else
                return false;
        }

        public string GetRoles(int id)
        {
            return Db.UserRoles.Where(r => r.ID == id && r.IsActive == true).FirstOrDefault().Role.ToLower();
        }
        public int GetRolesByName(string name)
        {
            return Db.UserRoles.Where(r => r.Role.ToLower() == name && r.IsActive == true).FirstOrDefault().ID;
        }
        // User PRofile

        public IEnumerable<Country> GetCountries()
        {
            return Db.Countries.Where(c => c.IsActive == true).ToList();
        }
        public IEnumerable<ReferenceData> GetGender()
        {
            return Db.ReferenceDatas.Where(m => m.RefCategory.ToLower().Equals("gender")).Where(m => m.IsActive == true);
        }

        public User GetUser(int id)
        {
            return Db.Users.SingleOrDefault(u => u.ID == id && u.IsActive == true);
        }
        public int AddUserDetails(UserProfile user)
        {
            Db.UserProfiles.Add(user);
            Db.SaveChanges();
            return user.ID;
        }
        public UserProfile GetUserProfileDetails(int id)
        {
            return Db.UserProfiles.SingleOrDefault(u => u.UserID == id);
        }
        public void UpdateUserProfile()
        {
            Db.SaveChanges();
        }
        public void UpdateUp()
        {
            Db.SaveChanges();
        }
        public Country FindCountry(string name)
        {
            return Db.Countries.SingleOrDefault(c => c.Name == name && c.IsActive == true);
        }
        public Country FindCountryById(int id)
        {
            return Db.Countries.SingleOrDefault(c => c.ID == id && c.IsActive == true);
        }
        public IEnumerable<NoteType> GetTypes()
        {
            return Db.NoteTypes.Where(c => c.IsActive == true).ToList();
        }
        public IEnumerable<NoteCategory> GetCategories()
        {
            return Db.NoteCategories.Where(c => c.IsActive == true).ToList();
        }
        public string GetCategoryNameById(int id)
        {
            return Db.NoteCategories.SingleOrDefault(c => c.ID == id && c.IsActive == true).Name;
        }
        public IEnumerable<ReferenceData> GetSellingModes()
        {
            return Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("selling mode") && c.IsActive == true);
        }
        public string GetSellingModeById(int id)
        {
            var sellMode = Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("selling mode") && c.IsActive == true);
            return sellMode.SingleOrDefault(s => s.ID == id).Value.ToLower();
        }
        public string GetNoteStatusById(int id)
        {
            var ReferenceInDB = Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("notes status") && c.IsActive == true);
            return ReferenceInDB.SingleOrDefault(c => c.ID == id).Value.ToLower();
        }
        public bool GetIsPaidStatus(int SellingModeId)
        {
            var SellingMode =  Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("selling mode") && c.IsActive==true);
            foreach (var i in SellingMode)
            {
                if (SellingModeId == i.ID && i.Value.ToLower() == "paid" && i.IsActive == true)
                    return true;
                else if (SellingModeId == i.ID && i.Value.ToLower() == "free" && i.IsActive == true)
                    return false;
            }
            return false;
        }
        
        public int GetStatusId(string name)
        {
            var status = Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("notes status") && c.IsActive == true);
            return status.SingleOrDefault(s => s.Value.ToLower() == name).ID;
        }
        public int AddSellerNote(SellerNote Note)
        {
            Db.SellerNotes.Add(Note);
            Db.SaveChanges();
            return Note.ID;
        }
        public List<DashBoard> GetSellerDraftNoteBySellerId(int SellerId)
        {

            return (from sellernote in Db.SellerNotes
                    join category in Db.NoteCategories on sellernote.Category equals category.ID
                    join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                    where sellernote.SellerID == SellerId && (status.Value.ToLower().Contains("draft") || 
                    status.Value.ToLower().Contains("submitted for review") || status.Value.Contains("inreview")) 
                    && sellernote.IsActive == true && category.IsActive == true && status.IsActive == true
                    orderby sellernote.CreatedDate
                    descending
                    select new DashBoard
                    {
                        NoteId = sellernote.ID,
                        AddedDate = (DateTime)sellernote.CreatedDate,
                        Title = sellernote.Title,
                        Category = category.Name,
                        Status = status.Value
                    }).ToList();
        }
        public List<DashBoard> GetSellerPublishedNoteBySellerId(int SellerId)
        {
            
            return (from sellernote in Db.SellerNotes
                    join category in Db.NoteCategories on sellernote.Category equals category.ID
                    join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                    where sellernote.SellerID == SellerId && status.Value.ToLower().Contains("published") && sellernote.IsActive == true
                    && category.IsActive == true && status.IsActive == true
                    orderby sellernote.CreatedDate
                    descending
                    select new DashBoard
                    {
                        NoteId = sellernote.ID,
                        AddedDate = (DateTime)sellernote.CreatedDate,
                        Title = sellernote.Title,
                        Category = category.Name,
                        Status = status.Value,
                        IsPaid = sellernote.IsPaid,
                        Price = sellernote.SellingPrice
                    }).ToList();
        }
        public List<DashBoard> GetSearchedPublishedNotes(string search, int SellerId)
        {
            if (search != null)
            {
                return (from sellernote in Db.SellerNotes
                        join category in Db.NoteCategories on sellernote.Category equals category.ID
                        join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                        where (sellernote.Title.Contains(search) || status.Value.Contains(search) || category.Name.Contains(search)) &&
                        sellernote.SellerID == SellerId && status.Value.ToLower().Contains("published") && sellernote.IsActive == true
                        && category.IsActive == true && status.IsActive == true
                        orderby sellernote.CreatedDate
                        descending
                        select new DashBoard
                        {
                            NoteId = sellernote.ID,
                            AddedDate = (DateTime)sellernote.CreatedDate,
                            Title = sellernote.Title,
                            Category = category.Name,
                            Status = status.Value,
                            IsPaid = sellernote.IsPaid,
                            Price = sellernote.SellingPrice
                        }).ToList();
            }
            else
            {
                return (from sellernote in Db.SellerNotes
                        join category in Db.NoteCategories on sellernote.Category equals category.ID
                        join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                        where sellernote.SellerID == SellerId && status.Value.ToLower().Contains("published") && sellernote.IsActive == true
                        && category.IsActive == true && status.IsActive == true
                        orderby sellernote.CreatedDate
                        descending
                        select new DashBoard
                        {
                            NoteId = sellernote.ID,
                            AddedDate = (DateTime)sellernote.CreatedDate,
                            Title = sellernote.Title,
                            Category = category.Name,
                            Status = status.Value,
                            IsPaid = sellernote.IsPaid,
                            Price = sellernote.SellingPrice
                        }).ToList();
            }
        }
        public List<DashBoard> GetSearchedDraftNotes(string search, int SellerId)
        {
            if(search != null)
            {
                return (from sellernote in Db.SellerNotes
                        join category in Db.NoteCategories on sellernote.Category equals category.ID
                        join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                        where (sellernote.Title.Contains(search) || status.Value.Contains(search) || category.Name.Contains(search)) &&
                        sellernote.SellerID == SellerId && (status.Value.ToLower().Contains("draft") ||
                        status.Value.ToLower().Contains("submitted for review") || status.Value.ToLower().Contains("inreview")) &&
                        sellernote.IsActive == true
                        && category.IsActive == true && status.IsActive == true
                        orderby sellernote.CreatedDate
                        descending
                        select new DashBoard
                        {
                            NoteId = sellernote.ID,
                            AddedDate = (DateTime)sellernote.CreatedDate,
                            Title = sellernote.Title,
                            Category = category.Name,
                            Status = status.Value
                        }).ToList();
            }
            else
            {
                return (from sellernote in Db.SellerNotes
                        join category in Db.NoteCategories on sellernote.Category equals category.ID
                        join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                        where sellernote.SellerID == SellerId && (status.Value.ToLower().Contains("draft") ||
                        status.Value.ToLower().Contains("submitted for review") || status.Value.ToLower().Contains("inreview")) && sellernote.IsActive == true
                        && category.IsActive == true && status.IsActive == true
                        orderby sellernote.CreatedDate
                        descending
                        select new DashBoard
                        {
                            NoteId = sellernote.ID,
                            AddedDate = (DateTime)sellernote.CreatedDate,
                            Title = sellernote.Title,
                            Category = category.Name,
                            Status = status.Value
                        }).ToList();
            }
        }
        public SellerNote GetSellerNoteByNoteId(int NoteId)
        {
            return Db.SellerNotes.SingleOrDefault(n => n.ID == NoteId && n.IsActive ==true);
        }
        public SingleNoteDetail GetSingleNoteDetail(int NoteId)
        {
            return (from note in Db.SellerNotes
                    join category in Db.NoteCategories on note.Category equals category.ID
                    //join country in Db.Countries on note.Country equals country.ID
                    where note.ID == NoteId && note.IsActive == true
                    select new SingleNoteDetail
                    {
                        Id = note.ID,
                        SellerId = note.SellerID,
                        sellerNote = note,  
                        Category = category.Name
                    }).SingleOrDefault();
        }
        public string GetUserNameById(int id)
        {
            return Db.Users.Where(u => u.ID == id && u.IsActive == true).Select(u => u.FirstName).FirstOrDefault();
        }
        public string GetCountryNameById(int id)
        {
            return Db.Countries.Where(n => n.ID == id && n.IsActive == true).Select(n => n.Name).SingleOrDefault();
        }
        public int GetIdSellFor(SellerNote note)
        {
            if(note.IsPaid == true)
            {
                var forId = Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("selling mode") && c.IsActive ==true);
                return forId.SingleOrDefault(c => c.Value.ToLower() == "paid").ID;
            }
            else
            {
                var forId = Db.ReferenceDatas.Where(c => c.RefCategory.ToLower().Equals("selling mode") && c.IsActive == true);
                return forId.SingleOrDefault(c => c.Value.ToLower() == "free").ID;
            }
        }

        public void AddSellerNoteAttachment(SellerNotesAttachment attachment)
        {
            Db.SellerNotesAttachments.Add(attachment);
            Db.SaveChanges();
        }
        public SellerNotesAttachment GetAttachmentByNoteId(int noteId)
        {
            return Db.SellerNotesAttachments.SingleOrDefault(a => a.NoteID == noteId);
        }

        public void DeleteDraftNoteFromDb(int id)
        {
            var noteInDb = Db.SellerNotes.SingleOrDefault(n => n.ID == id);
            var attachment = Db.SellerNotesAttachments.SingleOrDefault(a => a.NoteID == id);

            Db.SellerNotes.Remove(noteInDb);
            if(attachment != null)
                Db.SellerNotesAttachments.Remove(attachment);

            Db.SaveChanges();
        }

        public List<SearchNoteWrap> GetSellerNote()
        {
            Db.Configuration.ProxyCreationEnabled = false;
            //return Db.SellerNotes.Where(n => n.Status == 9 && n.IsActive == true).ToList();

            return (from note in Db.SellerNotes
                   // join review in Db.SellerNotesReviews on note.ID equals review.NoteID
                    join refer in Db.ReferenceDatas on note.Status equals refer.ID
                    where refer.Value.ToLower().Contains("publish") && note.IsActive == true && refer.IsActive == true
                    
                    select new SearchNoteWrap
                    {
                        ID = note.ID,
                        SellerID = note.SellerID,
                        CreatedDate = note.CreatedDate,
                        DisplayPicture = note.DisplayPicture,
                        NumberOfPages = note.NumberOfPages,
                        Title = note.Title,
                        UniversityName = note.UniversityName,
                        CountryName = note.Country1.Name
                    }
                    ).ToList();
        }
        public List<SellerNote> GetNotesByCategory(int id)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            return Db.SellerNotes.Where(c => c.Category == id).ToList();
        }
        public List<SellerNote> GetNotesByType(int id)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            return Db.SellerNotes.Where(c => c.NoteType == id).ToList();
        }

        public List<SellerNote> Getnote(int categoryId, int type)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            return Db.SellerNotes.Where(c => c.NoteType == type && c.Category == categoryId).ToList();
        }

        public List<SearchNoteWrap> GetNotesByFilter(string search, int Category, int Type, string University, string Course, int Country, string rate)
        {

            Db.Configuration.ProxyCreationEnabled = false;
            
            search = search.ToLower();
            Course = Course.ToLower();
            University = University.ToLower();

            var notes = (from note in Db.SellerNotes
                         join refer in Db.ReferenceDatas on note.Status equals refer.ID
                         where refer.Value.ToLower().Contains("published") && note.IsActive == true
                         select new SearchNoteWrap
                         {
                             ID = note.ID,
                             SellerID = note.SellerID,
                             CreatedDate = note.CreatedDate,
                             DisplayPicture = note.DisplayPicture,
                             NumberOfPages = note.NumberOfPages,
                             Title = note.Title,
                             UniversityName = note.UniversityName,
                             Category = note.Category,
                             NoteType = note.NoteType,
                             Country = note.Country,
                             Course = note.Course,
                             CountryName = note.Country1.Name
                         }
                         ).ToList();
            if(rate != "0")
            {
                int rating = Convert.ToInt32(rate.Substring(0, 1));
                foreach (var note in notes)
                {
                    note.avg = GetAvgRatingByNoteId(note.ID);
                }
                notes = notes.Where(n => n.avg > rating).ToList();

            }
            if (search != "")
            {
                notes = notes.Where(c => c.Title.ToLower().Contains(search)).ToList();
               
            }

            if (Category != 0)
            {
                notes = notes.Where(c => c.Category == Category).ToList();
              
            }
            if (Type != 0)
            {
                notes = notes.Where(c => c.NoteType == Type).ToList();
               
            }

            if (University != "0")
            {
                notes = notes.Where(c => c.UniversityName.ToLower().Contains(University)).ToList();
            }

            if (Course != "0")
            {
                notes = notes.Where(c => c.Course.ToLower().Contains(Course)).ToList();
                
            }

            if (Country != 0)
            {
                notes = notes.Where(c => c.Country == Country).ToList();
            }

            return notes;
        }

        public List<string> GetUniversities()
        {
            return Db.SellerNotes.Where(n => n.UniversityName != null && n.Status == 9 && n.IsActive == true).Select(n => n.UniversityName).Distinct().ToList();
        }
        public List<string> GetCourses()
        {
            return Db.SellerNotes.Where(n => n.Course != null && n.Status == 9 && n.IsActive ==true).Select(n => n.Course).Distinct().ToList();
        }

        public int AddDownload(Download obj)
        {
            Db.Downloads.Add(obj);
            Db.SaveChanges();
            return obj.ID;
        } 

        public IEnumerable<MyTable> GetBuyerReqData(int sellerId)
        {
            return (from download in Db.Downloads
                    join note in Db.SellerNotes on download.NoteID equals note.ID
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join downloader in Db.Users on download.Downloader equals downloader.ID
                    join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                    where download.Seller == sellerId && note.IsPaid == true && download.IsSellerasAllowedDownload == false
                    && note.IsActive == true && downloader.IsActive == true
                    orderby download.CreatedDate descending
                    select new MyTable
                    {
                        DownloadId = download.ID,
                        NoteId = note.ID,
                        Title = note.Title,
                        Category = category.Name,
                        Buyer = downloader.EmailID,
                        code = downloaderEmail.PhoneNumberCounrtyCode,
                        PhoneNo = downloaderEmail.PhoneNumber,
                        isPaid = note.IsPaid,
                        Price = note.SellingPrice,
                        downloadDate = (DateTime)download.CreatedDate
                    }).ToList();

        }

        public IEnumerable<MyTable> GetBuyerReqSearchData(int sellerId, string search)
        {
           if(search != null)
            {
                search = search.ToLower();

                return (from download in Db.Downloads
                        join note in Db.SellerNotes on download.NoteID equals note.ID
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join downloader in Db.Users on download.Downloader equals downloader.ID
                        join userDetail in Db.UserProfiles on download.Downloader equals userDetail.UserID
                        where (note.Title.ToLower().Contains(search) || category.Name.ToLower().Contains(search)
                        || userDetail.PhoneNumber.Contains(search) || note.SellingPrice.ToString().Contains(search)) &&
                        download.Seller == sellerId && note.IsPaid == true && download.IsSellerasAllowedDownload == false
                        && note.IsActive == true && downloader.IsActive == true
                        orderby download.CreatedDate descending
                        select new MyTable
                        {
                            DownloadId = download.ID,
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Buyer = downloader.EmailID,
                            code = userDetail.PhoneNumberCounrtyCode,
                            PhoneNo = userDetail.PhoneNumber,
                            isPaid = note.IsPaid,
                            Price = note.SellingPrice,
                            downloadDate = (DateTime)download.CreatedDate
                        }).ToList();
            }
            else
            {
                return (from download in Db.Downloads
                        join note in Db.SellerNotes on download.NoteID equals note.ID
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join downloader in Db.Users on download.Downloader equals downloader.ID
                        join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                        where download.Seller == sellerId && note.IsActive == true && downloader.IsActive == true
                        orderby download.CreatedDate descending
                        select new MyTable
                        {
                            DownloadId = download.ID,
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Buyer = downloader.EmailID,
                            code = downloaderEmail.PhoneNumberCounrtyCode,
                            PhoneNo = downloaderEmail.PhoneNumber,
                            isPaid = note.IsPaid,
                            Price = note.SellingPrice,
                            downloadDate = (DateTime)download.CreatedDate
                        }).ToList();
            }

        }

        public Download GetFromDownload(int id)
        {
            return Db.Downloads.SingleOrDefault(d => d.ID == id);
        }

        public IEnumerable<MyTable> GetMyDownloadsNotes(int downloaderId)
        {
            return (from download in Db.Downloads
                    join note in Db.SellerNotes on download.NoteID equals note.ID
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join downloader in Db.Users on download.Downloader equals downloader.ID
                    join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                    where download.Downloader == downloaderId && note.IsActive == true && downloader.IsActive == true
                    orderby download.CreatedDate descending
                    select new MyTable
                    {
                        DownloadId = download.ID,
                        NoteId = note.ID,
                        Title = note.Title,
                        Category = category.Name,
                        Buyer = downloader.EmailID,
                        code = downloaderEmail.PhoneNumberCounrtyCode,
                        isPaid = note.IsPaid,
                        Price = note.SellingPrice,
                        downloadDate = (DateTime)download.CreatedDate
                    }).ToList();
        }

        public IEnumerable<MyTable> GetMyDownloadsSearchedNotes(int downloaderId, string search)
        {
            if (search != null)
            {
                search = search.ToLower();

                return (from download in Db.Downloads
                        join note in Db.SellerNotes on download.NoteID equals note.ID
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join downloader in Db.Users on download.Downloader equals downloader.ID
                        join userDetail in Db.UserProfiles on download.Downloader equals userDetail.UserID
                        where (note.Title.ToLower().Contains(search) || category.Name.ToLower().Contains(search)
                        || userDetail.PhoneNumber.Contains(search) || note.SellingPrice.ToString().Contains(search)) &&
                        download.Seller == downloaderId && note.IsActive == true && downloader.IsActive == true
                        orderby download.CreatedDate descending
                        select new MyTable
                        {
                            DownloadId = download.ID,
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Buyer = downloader.EmailID,
                            code = userDetail.PhoneNumberCounrtyCode,
                            PhoneNo = userDetail.PhoneNumber,
                            isPaid = note.IsPaid,
                            Price = note.SellingPrice,
                            downloadDate = (DateTime)download.CreatedDate
                        }).ToList();
            }
            else
            {
                return (from download in Db.Downloads
                        join note in Db.SellerNotes on download.NoteID equals note.ID
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join downloader in Db.Users on download.Downloader equals downloader.ID
                        join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                        where download.Seller == downloaderId && note.IsActive == true && downloader.IsActive == true
                        orderby download.CreatedDate descending
                        select new MyTable
                        {
                            DownloadId = download.ID,
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Buyer = downloader.EmailID,
                            code = downloaderEmail.PhoneNumberCounrtyCode,
                            PhoneNo = downloaderEmail.PhoneNumber,
                            isPaid = note.IsPaid,
                            Price = note.SellingPrice,
                            downloadDate = (DateTime)download.CreatedDate
                        }).ToList();
            }
        }

        public void AddReview(SellerNotesReview obj)
        {
            Db.SellerNotesReviews.Add(obj);
            Db.SaveChanges();
        }

        public void addReportedIsuue(SellerNotesReportedIssue obj)
        {
            Db.SellerNotesReportedIssues.Add(obj);
            Db.SaveChanges();
        }

        public Download CheckDownloadEntry(int noteId, int sellerId, int downloaderId)
        {
            return Db.Downloads.Where(d => d.NoteID == noteId && d.Seller == sellerId && d.Downloader == downloaderId).FirstOrDefault();
        }

        public IEnumerable<MyTable> GetMySoldNotes(int sellerId)
        {
            return (from download in Db.Downloads
                    join note in Db.SellerNotes on download.NoteID equals note.ID
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join downloader in Db.Users on download.Downloader equals downloader.ID
                    join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                    where download.Seller == sellerId && download.IsAttachmentDownload==true && download.IsSellerasAllowedDownload==true 
                    && note.IsActive == true && downloader.IsActive == true
                    orderby download.CreatedDate descending
                    select new MyTable
                    {
                        DownloadId = download.ID,
                        NoteId = note.ID,
                        Title = note.Title,
                        Category = category.Name,
                        Buyer = downloader.EmailID,
                        code = downloaderEmail.PhoneNumberCounrtyCode,
                        isPaid = note.IsPaid,
                        Price = note.SellingPrice,
                        downloadDate = (DateTime)download.CreatedDate
                    }).ToList();
        }

        public IEnumerable<MyTable> GetMySoldSearchedNotes(int sellerId, string search)
        {
            if (search != null)
            {
                search = search.ToLower();

                return (from download in Db.Downloads
                        join note in Db.SellerNotes on download.NoteID equals note.ID
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join downloader in Db.Users on download.Downloader equals downloader.ID
                        join userDetail in Db.UserProfiles on download.Downloader equals userDetail.UserID
                        where (note.Title.ToLower().Contains(search) || category.Name.ToLower().Contains(search)
                        || userDetail.PhoneNumber.Contains(search) || note.SellingPrice.ToString().Contains(search)) &&
                        download.Seller == sellerId && note.IsActive == true && downloader.IsActive == true
                        orderby download.CreatedDate descending
                        select new MyTable
                        {
                            DownloadId = download.ID,
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Buyer = downloader.EmailID,
                            code = userDetail.PhoneNumberCounrtyCode,
                            PhoneNo = userDetail.PhoneNumber,
                            isPaid = note.IsPaid,
                            Price = note.SellingPrice,
                            downloadDate = (DateTime)download.CreatedDate
                        }).ToList();
            }
            else
            {
                return (from download in Db.Downloads
                        join note in Db.SellerNotes on download.NoteID equals note.ID
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join downloader in Db.Users on download.Downloader equals downloader.ID
                        join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                        where download.Seller == sellerId && note.IsActive == true && downloader.IsActive == true
                        orderby download.CreatedDate descending
                        select new MyTable
                        {
                            DownloadId = download.ID,
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Buyer = downloader.EmailID,
                            code = downloaderEmail.PhoneNumberCounrtyCode,
                            PhoneNo = downloaderEmail.PhoneNumber,
                            isPaid = note.IsPaid,
                            Price = note.SellingPrice,
                            downloadDate = (DateTime)download.CreatedDate
                        }).ToList();
            }
        }

        public IEnumerable<MyTable> GetRejectedNotes(int id)
        {
            return (from note in Db.SellerNotes
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join refer in Db.ReferenceDatas on note.Status equals refer.ID
                    where refer.Value.ToLower().Contains("rejected") && note.SellerID == id &&
                    note.IsActive == true && refer.IsActive == true
                    orderby note.ModifiedDate descending
                    select new MyTable
                    {
                        NoteId = note.ID,
                        Title = note.Title,
                        Category = category.Name,
                        Remarks = note.AdminRemarks,
                        DateEdited = (DateTime)note.ModifiedDate
                    }).ToList();
        }
        public IEnumerable<MyTable> GetRejectedSearchedNotes(int id, string search)
        {
            if(search != null)
            {
                search = search.ToLower();

                return (from note in Db.SellerNotes
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join refer in Db.ReferenceDatas on note.Status equals refer.ID
                        where note.SellerID == id && ( note.Title.ToLower().Contains(search) || note.AdminRemarks.ToLower().Contains(search) || category.Name.ToLower().Contains(search) ) && 
                        refer.Value.ToLower().Contains("rejected") && note.IsActive == true
                        && refer.IsActive == true
                        orderby note.ModifiedDate descending
                        select new MyTable
                        {
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Remarks = note.AdminRemarks,
                            DateEdited = (DateTime)note.ModifiedDate
                        }).ToList();
            }
            else
            {
                return (from note in Db.SellerNotes
                        join category in Db.NoteCategories on note.Category equals category.ID
                        join refer in Db.ReferenceDatas on note.Status equals refer.ID
                        where note.SellerID == id && refer.Value.ToLower().Contains("rejected") && 
                        note.IsActive == true && refer.IsActive == true
                        orderby note.ModifiedDate descending
                        select new MyTable
                        {
                            NoteId = note.ID,
                            Title = note.Title,
                            Category = category.Name,
                            Remarks = note.AdminRemarks,
                            DateEdited = (DateTime)note.ModifiedDate
                        }).ToList();
            }
        }

        public decimal GetAvgRatingByNoteId(int id)
        {
            List<SellerNotesReview> a = Db.SellerNotesReviews.Where(n => n.NoteID == id && n.IsActive == true).ToList();
            if (a.Count() != 0)
            {
                var sum = a.Sum(n => n.Ratings);
                var count = a.Count();
                return (sum / count);
            }
            else
            {
                return 0;
            }
        }

        public int GetRatingCount(int id)
        {
            return Db.SellerNotesReviews.Where(n => n.NoteID == id && n.IsActive == true).Count();
        }

        public List<SingleReview> GetNotesReview(int id)
        {
            return (from review in Db.SellerNotesReviews
                    join user in Db.Users on review.ReviewedByID equals user.ID
                    join userPic in Db.UserProfiles on user.ID equals userPic.UserID
                    join note in Db.SellerNotes on review.NoteID equals note.ID
                    where review.NoteID == id && review.IsActive == true
                    select new SingleReview
                    {
                        pic = userPic.ProfilePicture,
                        SellerId = note.SellerID,
                        Name = user.FirstName + " " + user.LastName,
                        comments = review.Comments,
                        rating = review.Ratings
                    }
                    ).ToList();
            //return Db.SellerNotesReviews.Where(n => n.NoteID == id && n.IsActive == true).ToList();
        }
        public int GetNotesReportedIssueCount(int id)
        {
            return Db.SellerNotesReportedIssues.Where(n => n.NoteID == id).Count();
        }

        // Admin
        public int GetPublishedNotesCount()
        {
            return (from note in Db.SellerNotes
                    join refer in Db.ReferenceDatas on note.Status equals refer.ID
                    where note.IsActive == true && ( refer.Value.ToLower().Equals("submitted for review") || refer.Value.ToLower().Equals("inreview") )&& 
                    refer.IsActive == true
                    select note).ToList().Count();
        }

        public int GetLastSevenDaysDownload()
        {
            var date = DateTime.Now.AddDays(-7);
            return (from note in Db.Downloads
                    where note.AttachmentDownloadedDate > date
                    select note).ToList().Count();
        }

        public int GetLastSevenDaysNewRegistration()
        {
            var date = DateTime.Now.AddDays(-7);
            return (from user in Db.Users
                    where user.CreatedDate > date
                    select user).ToList().Count();
        }

        public IEnumerable<AdminPublishedNotes> GetPublishedNotes()
        {
            return (from note in Db.SellerNotes
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join attachment in Db.SellerNotesAttachments on note.ID equals attachment.NoteID
                    join refer in Db.ReferenceDatas on note.Status equals refer.ID
                    join user in Db.Users on note.ActionedBy equals user.ID
                    where note.IsActive == true && refer.Value.ToLower().Equals("published")
                    orderby note.PublishedDate descending
                    select new AdminPublishedNotes
                    {
                        Title = note.Title,
                        Category = category.Name,
                        AttachmentPath = attachment.FilePath,
                        SellType = refer.Value,
                        IsPaid = note.IsPaid,
                        Price = (decimal)note.SellingPrice,
                        Publisher = user.FirstName + " " +user.LastName,
                        PublishedDate = (DateTime)note.PublishedDate
                    }
                    ).ToList();
        }

        public IEnumerable<AdminPublishedNotes> GetAdminSearchedPublishedNotes(string search, int month)
        {

            var notes = (from note in Db.SellerNotes
                         join category in Db.NoteCategories on note.Category equals category.ID
                         join attachment in Db.SellerNotesAttachments on note.ID equals attachment.NoteID
                         join refer in Db.ReferenceDatas on note.Status equals refer.ID
                         join user in Db.Users on note.ActionedBy equals user.ID
                         where note.IsActive == true && refer.Value.ToLower().Equals("published")
                         && note.PublishedDate.Value.Month == month && note.PublishedDate.Value.Year == DateTime.Now.Year
                         orderby note.PublishedDate descending
                         select new AdminPublishedNotes
                         {
                             Title = note.Title,
                             Category = category.Name,
                             AttachmentPath = attachment.FilePath,
                             SellType = refer.Value,
                             IsPaid = note.IsPaid,
                             Price = (decimal)note.SellingPrice,
                             Publisher = user.FirstName + " " + user.LastName,
                             PublishedDate = (DateTime)note.PublishedDate
                         }
                    ).ToList();

            if (search != null)
            {
                search = search.ToLower();
                notes = notes.Where(n => (n.Title.ToLower().Contains(search) || n.Category.ToLower().Contains(search)) 
                        && n.PublishedDate.Month == month && n.PublishedDate.Year == DateTime.Now.Year ).ToList();
                
            }
            return notes;
            
        }

        

    }
}