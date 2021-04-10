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
                    status.Value.ToLower().Contains("submitted for review") || status.Value.Contains("in review")) 
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
                        status.Value.ToLower().Contains("submitted for review") || status.Value.ToLower().Contains("in review")) &&
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
                        status.Value.ToLower().Contains("submitted for review") || status.Value.ToLower().Contains("in review")) && sellernote.IsActive == true
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
                        Id = review.ID,
                        pic = userPic.ProfilePicture,
                        SellerId = note.SellerID,
                        Name = user.FirstName + " " + user.LastName,
                        comments = review.Comments,
                        rating = review.Ratings
                    }
                    ).ToList();
            //return Db.SellerNotesReviews.Where(n => n.NoteID == id && n.IsActive == true).ToList();
        }

        public void RemoveCustomerReview(int id)
        {
            var review = Db.SellerNotesReviews.SingleOrDefault(r => r.ID == id);

            Db.SellerNotesReviews.Remove(review);
            Db.SaveChanges();
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
                    where note.IsActive == true && ( refer.Value.ToLower().Equals("submitted for review") || refer.Value.ToLower().Equals("in review") )&& 
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
                    where note.IsActive == true && note.PublishedDate.Value.Year == DateTime.Now.Year &&
                    note.PublishedDate.Value.Month == DateTime.Now.Month &&
                    refer.Value.ToLower().Equals("published")
                    orderby note.PublishedDate descending
                    select new AdminPublishedNotes
                    {
                        Title = note.Title,
                        Category = category.Name,
                        AttachmentPath = attachment.FilePath,
                        SellType = refer.Value,
                        IsPaid = note.IsPaid,
                        Price = note.SellingPrice,
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
                         where note.IsActive == true && refer.Value.ToLower().Equals("published") && 
                         note.PublishedDate.Value.Year == DateTime.Now.Year || note.PublishedDate.Value.Year == DateTime.Now.Year - 1
                        
                         select new AdminPublishedNotes
                         {
                             Id = note.ID,
                             Title = note.Title,
                             Category = category.Name,
                             AttachmentPath = attachment.FilePath,
                             SellType = refer.Value,
                             IsPaid = note.IsPaid,
                             Price = note.SellingPrice,
                             Publisher = user.FirstName + " " + user.LastName,
                             PublishedDate = (DateTime)note.PublishedDate
                         }
                    ).OrderByDescending(n => n.PublishedDate).ToList();
            if(month != 0)
            {
                notes = notes.Where(n => n.PublishedDate.Month == month).ToList();
            }
            else
            {
                notes = notes.Where(n => n.PublishedDate.Month == DateTime.Now.Month && n.PublishedDate.Year == DateTime.Now.Year).ToList();
            }
            if (search != null)
            {
                search = search.ToLower();
                notes = notes.Where(n => (n.Title.ToLower().Contains(search) || n.Category.ToLower().Contains(search)) ).ToList();
                
            }
            return notes;
            
        }

        public IEnumerable<AdminTable> GetUnderReviewNotes(string name, string search)
        {
            int id = Convert.ToInt32(name);
            var notes = (from note in Db.SellerNotes
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join seller in Db.Users on note.SellerID equals seller.ID
                    join refer in Db.ReferenceDatas on note.Status equals refer.ID
                    where note.IsActive == true && seller.IsActive == true &&
                    (refer.Value.ToLower() == "submitted for review" || refer.Value.ToLower() == "in review")
                    orderby note.CreatedDate ascending
                    select new AdminTable
                    {
                        Id = note.ID,
                        FirstName = seller.FirstName,
                        SellerId = seller.ID,
                        Seller = seller.FirstName + " " + seller.LastName,
                        Title = note.Title,
                        Category = category.Name,
                        Status = refer.Value,
                        DateAdded = (DateTime)note.CreatedDate
                    }
                    ).ToList();
            //if(name != null)
            //{
            //    name = name.ToLower();
            //    notes = notes.Where(n => n.FirstName.ToLower() == name).ToList();
            //}
            if (id != 0)
            {
                //name = name.ToLower();
                notes = notes.Where(n => n.SellerId == id).ToList();
            }
            if (search != null)
            {
                search = search.ToLower();
                notes = notes.Where(n => (n.Title.ToLower().Contains(search) || n.Category.ToLower().Contains(search) 
                        || n.Status.ToLower().Contains(search))).ToList();
            }
            return notes;
        }

        public IEnumerable<User> GetAllSellers()
        {
            var Submitedid = GetStatusId("submitted for review");
            var inReviewId = GetStatusId("in review");
            //return (from note in Db.SellerNotes
            //        join seller in Db.Users on note.SellerID equals seller.ID
            //        where note.IsActive == true && seller.IsActive == true
            //        && (note.Status == Submitedid || note.Status == inReviewId)
            //        select seller).Select(n => n.FirstName).Distinct().ToList();

            return (from note in Db.SellerNotes
                    join seller in Db.Users on note.SellerID equals seller.ID
                    where note.IsActive == true && seller.IsActive == true
                    && (note.Status == Submitedid || note.Status == inReviewId)
                    select seller).Distinct().ToList();
        }

        public IEnumerable<User> GetAllPublishedSellers()
        {
            var publishedId = GetStatusId("published");
          
            return (from note in Db.SellerNotes
                    join seller in Db.Users on note.SellerID equals seller.ID
                    where note.IsActive == true && seller.IsActive == true
                    && note.Status == publishedId
                    select seller).Distinct().ToList();
        }

        public IEnumerable<User> GetAllBuyers()
        {
            return (from download in Db.Downloads
                    join buyer in Db.Users on download.Downloader equals buyer.ID
                    where buyer.IsActive == true && download.IsAttachmentDownload
                    select buyer
                    ).Distinct().ToList();
        }
        public IEnumerable<User> GetAllDownloadedSeller()
        {
            return (from download in Db.Downloads
                    join seller in Db.Users on download.Seller equals seller.ID
                    where seller.IsActive == true && download.IsAttachmentDownload
                    select seller
                    ).Distinct().ToList();
        }

        public IEnumerable<User> GetRejectedSellers()
        {
            var rejectStatusId = GetStatusId("rejected");

            return (from note in Db.SellerNotes
                    join seller in Db.Users on note.SellerID equals seller.ID
                    where note.Status == rejectStatusId && note.IsActive==true 
                    && seller.IsActive==true
                    select seller
                    ).Distinct().ToList();
        }

        public IEnumerable<string> GetAllNotesName()
        {
            return (from download in Db.Downloads
                    join note in Db.SellerNotes on download.NoteID equals note.ID
                    where note.IsActive == true && download.IsAttachmentDownload == true
                    select note
                    ).Select(n => n.Title).Distinct().ToList();
        }

        public int GetNumberOfDownloads(int id)
        {
            return Db.Downloads.Where(n => n.NoteID == id && n.IsAttachmentDownload).ToList().Count();
        }

        public IEnumerable<AdminPublishedNotes> GetAdminPublishedNotes(string name, string search)
        {
            int sellerid = Convert.ToInt32(name);

            var notes = (from note in Db.SellerNotes
                         join category in Db.NoteCategories on note.Category equals category.ID
                         join refer in Db.ReferenceDatas on note.Status equals refer.ID
                         join seller in Db.Users on note.SellerID equals seller.ID
                         join admin in Db.Users on note.ActionedBy equals admin.ID
                         where note.IsActive == true && refer.Value.ToLower().Equals("published")
                       
                         select new AdminPublishedNotes
                         {
                             Id = note.ID,
                             FirstName = seller.FirstName,
                             SellerId = seller.ID,
                             Title = note.Title,
                             Category = category.Name,
                             IsPaid = note.IsPaid,
                             Price = note.SellingPrice,
                             Seller = seller.FirstName + " " + seller.LastName,
                             PublishedDate = (DateTime)note.PublishedDate,
                             ApproveBy = admin.FirstName + " " + admin.LastName
                         }
                         ).OrderByDescending(n => n.PublishedDate).ToList();
            foreach(var note in notes)
            {
                if (note.IsPaid)
                {
                    note.SellType = "Paid";
                }
                else
                {
                    note.SellType = "Free";
                }

                if (note.Price == null)
                    note.Price = 0;

                note.NumberOfDownloads = GetNumberOfDownloads(note.Id);
            }
            if(search != null)
            {
                search = search.ToLower();
                notes = notes.Where(n => (n.Title.ToLower().Contains(search) || n.Category.ToLower().Contains(search) ||
                        n.SellType.ToLower().Contains(search) || n.ApproveBy.ToLower().Contains(search) ||
                        n.Price.ToString().Contains(search) || n.NumberOfDownloads.ToString().Contains(search))).ToList();
            }
            if (sellerid != 0)
            {
                notes = notes.Where(n => n.SellerId == sellerid).ToList();
            }
            return notes;
            
        }

        public IEnumerable<AdminTable> GetAdminDownloadedNotes(string noteName, string buyerName, string sellerName, string search)
        {
           
            int buyerid = Convert.ToInt32(buyerName);
            int sellerid = Convert.ToInt32(sellerName);

            var notes = (from download in Db.Downloads
                         join note in Db.SellerNotes on download.NoteID equals note.ID
                         join category in Db.NoteCategories on note.Category equals category.ID
                         join seller in Db.Users on download.Seller equals seller.ID
                         join buyer in Db.Users on download.Downloader equals buyer.ID
                         where download.IsAttachmentDownload == true && note.IsActive == true && 
                         download.AttachmentDownloadedDate != null
                        
                         select new AdminTable
                         {
                             Id = note.ID,
                             Title = note.Title,
                             SellerId = seller.ID,
                             BuyerId = buyer.ID,
                             Category = category.Name,
                             Buyer = buyer.FirstName + " " + buyer.LastName,
                             Seller = seller.FirstName + " " + seller.LastName,
                             IsPaid = note.IsPaid,
                             Price = note.SellingPrice,
                             DownloadDate = download.AttachmentDownloadedDate.Value
                         }
                         ).OrderByDescending(n => n.DownloadDate).ToList();

            foreach (var note in notes)
            {
                if (note.IsPaid)
                    note.SellType = "Paid";
                else
                    note.SellType = "Free";

                if(note.Price == null)
                {
                    note.Price = 0;
                }
            }
            if (noteName != null)
            {
                noteName = noteName.ToLower();
                notes = notes.Where(n => n.Title.ToLower() == noteName).ToList();
            }
            if (buyerid != 0)
            {
                notes = notes.Where(n => n.BuyerId== buyerid ).ToList();
            }
            if (sellerid != 0)
            {
                notes = notes.Where(n => n.SellerId == sellerid).ToList();
            }
            if (search != null)
            {
                search = search.ToLower();
                notes = notes.Where(n => (n.Title.ToLower().Contains(search) || n.Category.ToLower().Contains(search)
                        || n.Buyer.ToLower().Contains(search) || n.Seller.ToLower().Contains(search) ||
                        n.SellType.ToLower().Contains(search) || n.Price.ToString().Contains(search)
                        )).ToList();
            }

            return notes;
        }

        public IEnumerable<AdminTable> GetAdminRejectedNotes(string search, string sellerName)
        {
            int sellerid = Convert.ToInt32(sellerName);

            var rejectStatusId = GetStatusId("rejected");
            var notes = (from note in Db.SellerNotes
                         join refer in Db.ReferenceDatas on note.Status equals refer.ID
                         join seller in Db.Users on note.SellerID equals seller.ID
                         join rejectBy in Db.Users on note.ActionedBy equals rejectBy.ID
                         where note.IsActive == true && note.Status == rejectStatusId
                         select new AdminTable
                         {
                             Id = note.ID,
                             Title = note.Title,
                             SellerId = seller.ID,
                             FirstName = seller.FirstName,
                             Category = note.NoteCategory.Name,
                             Seller = seller.FirstName + " " + seller.LastName,
                             DateAdded = (DateTime)note.ModifiedDate,
                             RejectBy = rejectBy.FirstName + " " + rejectBy.LastName,
                             Remark = note.AdminRemarks
                         }
                         ).ToList();

            if(search != null)
            {
                search = search.ToLower();
                notes = notes.Where(n => (n.Title.ToLower().Contains(search) || n.Category.ToLower().Contains(search)
                        || n.Seller.ToLower().Contains(search) || n.RejectBy.ToLower().Contains(search) ||
                        n.Remark.ToLower().Contains(search))).ToList();
            }
            if(sellerid != 0)
            {
                notes = notes.Where(n => n.SellerId == sellerid).ToList();
            }
            return notes;
        }
        public int GetUnderReviewNotesCountForMember(int id)
        {
            var SubmittedStatus = GetStatusId("submitted for review");
            var InReviewStatus = GetStatusId("in review");
            return Db.SellerNotes.Where(n => n.SellerID == id && (n.Status == SubmittedStatus || n.Status == InReviewStatus)).Count();
        }

        public int GetPublishedNotesCountForMember(int id)
        {
            var publishedStatus = GetStatusId("published");
            return Db.SellerNotes.Where(n => n.SellerID == id && n.Status == publishedStatus).Count();
        }

        public int GetDownloadNotesCountForMember(int id)
        {
            return Db.Downloads.Where(n => n.Downloader == id && n.IsAttachmentDownload == true).Count();
        }

        public decimal GetTotalExpensiveForMember(int id)
        {
            var downlaods = Db.Downloads.Where(n => n.Downloader == id && n.IsAttachmentDownload == true).ToList();
            decimal sum = 0;
            foreach(var note in downlaods)
            {
                if(note.PurchasedPrice != null)
                {
                    sum += (decimal)note.PurchasedPrice;
                }
            }
            return sum;
        }
        public decimal GetTotalEarningForMember(int id)
        {
            var sales = Db.Downloads.Where(n => n.Seller == id && n.IsSellerasAllowedDownload == true).ToList();
            decimal sum = 0;
            foreach (var note in sales)
            {
                if (note.PurchasedPrice != null)
                {
                    sum += (decimal)note.PurchasedPrice;
                }
            }
            return sum;
        }

        public IEnumerable<Members> GetMembers(string search)
        {
            var role = GetRolesByName("member");
            var members = (from member in Db.Users
                           where member.IsActive == true && member.RoleID == role
                         select new Members
                         {
                             Id = member.ID,
                             FirstName = member.FirstName,
                             LastName = member.LastName,
                             Email = member.EmailID,
                             JoiningDate = (DateTime)member.CreatedDate
                         }
                         ).OrderByDescending(n => n.JoiningDate).ToList();

           

            foreach (var member in members)
            {
                member.UnderReviewNotesCount = GetUnderReviewNotesCountForMember(member.Id);
                member.PublishedNotesCount = GetPublishedNotesCountForMember(member.Id);
                member.DownloadedNotesCount = GetDownloadNotesCountForMember(member.Id);
                member.TotalExpensive = GetTotalExpensiveForMember(member.Id);
                member.TotalEarning = GetTotalEarningForMember(member.Id);
            }

            if (search != null)
            {
                search = search.ToLower();
                members = members.Where(m => (m.FirstName.ToLower().Contains(search) || m.LastName.ToLower().Contains(search) ||
                          m.Email.ToLower().Contains(search) || m.TotalEarning.ToString().Contains(search) ||
                          m.TotalExpensive.ToString().Contains(search) || m.JoiningDate.ToString("dd-MM-yyyy, HH:mm").Contains(search))).ToList();
            }

            return members;
        }

        public MemberDetails GetMemberDetails(int id)
        {
            return (from member in Db.Users
                        join details in Db.UserProfiles on member.ID equals details.UserID
                        where member.ID == id && member.IsActive == true
                        select new MemberDetails
                        {
                            Id = member.ID,
                            FirstName = member.FirstName,
                            LastName = member.LastName,
                            Email = member.EmailID,
                            DisplayPic = details.ProfilePicture,
                            PhoneNumber = details.PhoneNumber,
                            Dob = (DateTime)details.DOB,
                            College = details.University,
                            Address1 = details.AddressLine1,
                            Address2 = details.AddressLine2,
                            City = details.City,
                            State = details.State,
                            Country = details.Country,
                            ZipCode = details.ZipCode
                        }
                        ).SingleOrDefault();
 
        }
        public User GetMemberById(int id)
        {
            return Db.Users.SingleOrDefault(u => u.ID == id && u.IsActive == true);
        }

        public int GetDownloadNoteCountForMember(int noteid, int sellerid)
        {
            return Db.Downloads.Where(n => n.NoteID == noteid && n.Seller == sellerid && n.IsAttachmentDownload).Count();
        }

        public decimal GetTotalEarningForParticularNoteForMember(int noteid, int sellerid)
        {
            var sales = Db.Downloads.Where(n => n.NoteID == noteid && n.Seller == sellerid && n.IsSellerasAllowedDownload).ToList();
            decimal sum = 0;
            foreach (var note in sales)
            {
                if (note.PurchasedPrice != null)
                {
                    sum += (decimal)note.PurchasedPrice;
                }
            }
            return sum;
        }

        public IEnumerable<AdminTable> GetMemberNotes(int id)
        {
            var draftStatus = GetStatusId("draft");
            var notes = (from note in Db.SellerNotes
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join status in Db.ReferenceDatas on note.Status equals status.ID
                    where note.SellerID == id && note.Status != draftStatus && note.IsActive == true
                    
                    select new AdminTable
                    {
                        Id = note.ID,
                        Title = note.Title,
                        Category = category.Name,
                        Status = status.Value,
                        DateAdded = (DateTime)note.CreatedDate,
                        PublishedDate = note.PublishedDate
                    }
                    ).OrderByDescending(n => n.DateAdded).ToList();

            foreach(var note in notes)
            {
                note.DownloadedNotes = GetDownloadNoteCountForMember(note.Id, id);
                note.TotalEarning = GetTotalEarningForParticularNoteForMember(note.Id, id);
            }

            return notes;
        }

        public IEnumerable<AdminTable> GetSpamReports(string search)
        {
            var reports = (from report in Db.SellerNotesReportedIssues
                           join note in Db.SellerNotes on report.NoteID equals note.ID
                           join category in Db.NoteCategories on note.Category equals category.ID
                           join reportedBy in Db.Users on report.ReportedByID equals reportedBy.ID
                           where note.IsActive == true
                          
                           select new AdminTable
                           {
                               Id = note.ID,
                               reportId = report.ID,
                               ReportedBy = reportedBy.FirstName + " " + reportedBy.LastName,
                               Title = note.Title,
                               Category = category.Name,
                               DateAdded = (DateTime)report.CreatedDate,
                               Remark = report.Remarks
                           }
                           ).Distinct().OrderByDescending(r => r.DateAdded).ToList();

            if(search != null)
            {
                search = search.ToLower();
                reports = reports.Where(r => (r.ReportedBy.ToLower().Contains(search) || r.Title.ToLower().Contains(search)
                          || r.Category.ToLower().Contains(search) || r.DateAdded.ToString("dd-MM-yyyy, HH:ss").Contains(search)
                          || r.Remark.ToLower().Contains(search))).ToList();
            }

            return reports;
        }

        public void RemoveReportedIssue(int id)
        {
            var report = Db.SellerNotesReportedIssues.SingleOrDefault(r => r.ID == id);
            Db.SellerNotesReportedIssues.Remove(report);
            Db.SaveChanges();
        }

        public IEnumerable<Manage> GetManageCategories(string search)
        {
            var categories = (from category in Db.NoteCategories
                              join admin in Db.Users on category.ModifiedBy equals admin.ID
                              select new Manage
                              {
                                  Id = category.ID,
                                  Name = category.Name,
                                  Description = category.Description,
                                  DateAdded = (DateTime)category.ModifiedDate,
                                  AddedBy = admin.FirstName + " " + admin.LastName,
                                  IsActive = category.IsActive
                              }
                              ).OrderByDescending(m => m.DateAdded).ToList();

            foreach(var category in categories)
            {
                if (category.IsActive)
                    category.Active = "Yes";
                else
                    category.Active = "No";
            }

            if(search != null)
            {
                search = search.ToLower();
                categories = categories.Where(c => (c.Name.ToLower().Contains(search) || c.Description.ToLower().Contains(search)
                             || c.DateAdded.ToString("dd-MM-yyyy, HH:mm").Contains(search) || c.AddedBy.ToLower().Contains(search)
                             || c.Active.ToLower().Contains(search))).ToList();
            }
            return categories;
        }
        public void AddCategory(NoteCategory category)
        {
            Db.NoteCategories.Add(category);
            Db.SaveChanges();
        }
        public NoteCategory GetCategory(int id)
        {
            return Db.NoteCategories.SingleOrDefault(c => c.ID == id);
        }

        public IEnumerable<Manage> GetManageTypes(string search)
        {
            var types = (from type in Db.NoteTypes
                              join admin in Db.Users on type.ModifiedBy equals admin.ID
                              select new Manage
                              {
                                  Id = type.ID,
                                  Name = type.Name,
                                  Description = type.Description,
                                  DateAdded = (DateTime)type.ModifiedDate,
                                  AddedBy = admin.FirstName + " " + admin.LastName,
                                  IsActive = type.IsActive
                              }
                              ).OrderByDescending(m => m.DateAdded).ToList();

            foreach (var type in types)
            {
                if (type.IsActive)
                    type.Active = "Yes";
                else
                    type.Active = "No";
            }

            if (search != null)
            {
                search = search.ToLower();
                types = types.Where(c => (c.Name.ToLower().Contains(search) || c.Description.ToLower().Contains(search)
                             || c.DateAdded.ToString("dd-MM-yyyy, HH:mm").Contains(search) || c.AddedBy.ToLower().Contains(search)
                             || c.Active.ToLower().Contains(search))).ToList();
            }
            return types;
        }

        public void AddType(NoteType Type)
        {
            Db.NoteTypes.Add(Type);
            Db.SaveChanges();
        }
        public NoteType GetType(int id)
        {
            return Db.NoteTypes.SingleOrDefault(t => t.ID == id );
        }
        public IEnumerable<Manage> GetManageCountries(string search)
        {
            var countries = (from country in Db.Countries
                         join admin in Db.Users on country.ModifiedBy equals admin.ID
                         select new Manage
                         {
                             Id = country.ID,
                             Name = country.Name,
                             CountryCode = country.CountryCode,
                             DateAdded = (DateTime)country.ModifiedDate,
                             AddedBy = admin.FirstName + " " + admin.LastName,
                             IsActive = country.IsActive
                         }
                         ).OrderByDescending(m => m.DateAdded).ToList();

            foreach (var country in countries)
            {
                if (country.IsActive)
                    country.Active = "Yes";
                else
                    country.Active = "No";
            }

            if (search != null)
            {
                search = search.ToLower();
                countries = countries.Where(c => (c.Name.ToLower().Contains(search) || c.CountryCode.ToLower().Contains(search)
                             || c.DateAdded.ToString("dd-MM-yyyy, HH:mm").Contains(search) || c.AddedBy.ToLower().Contains(search)
                             || c.Active.ToLower().Contains(search))).ToList();
            }
            return countries;
        }

        public void AddCountry(Country country)
        {
            Db.Countries.Add(country);
            Db.SaveChanges();
        }
        public Country GetCountry(int id)
        {
            return Db.Countries.SingleOrDefault(c => c.ID == id);
        }
        public void RemoveSystemConfig()
        {
            var all = Db.SystemConfigurations;
            if(all != null)
            {
                Db.SystemConfigurations.RemoveRange(all);
            }
            
        }
        public void AddSystemConfig(SystemConfiguration config)
        {
            Db.SystemConfigurations.Add(config);
        }

        public IEnumerable<Manage> GetManageAdmin(string search)
        {
            int role = GetRolesByName("admin");
            var admins = (from admin in Db.Users
                          join adminDetails in Db.UserProfiles on admin.ID equals adminDetails.UserID
                          where admin.RoleID == role
                          select new Manage
                          {
                              Id = admin.ID,
                              Name = admin.FirstName,
                              LastName = admin.LastName,
                              Email = admin.EmailID,
                              PhoneNumber = adminDetails.PhoneNumber,
                              DateAdded = (DateTime)admin.CreatedDate,
                              IsActive = admin.IsActive
                          }
                          ).OrderByDescending(a => a.DateAdded).ToList();

            foreach (var admin in admins)
            {
                if (admin.IsActive)
                    admin.Active = "Yes";
                else
                    admin.Active = "No";
            }
            if (search != null)
            {
                search = search.ToLower();
                admins = admins.Where(c => (c.Name.ToLower().Contains(search) || c.LastName.ToLower().Contains(search)
                             || c.Email.ToLower().Contains(search) || c.PhoneNumber.ToLower().Contains(search)
                             || c.DateAdded.ToString("dd-MM-yyyy, HH:mm").Contains(search)
                             || c.Active.ToLower().Contains(search))).ToList();
            }
            return admins;
        }

        public User GetAdmin(int id)
        {
            return Db.Users.SingleOrDefault(a => a.ID == id);
        }

        public string GetDefaultUserPic(string name)
        {
            return Db.SystemConfigurations.SingleOrDefault(s => s.key.ToLower() == name).Value;
        }
        public string GetDefaultNotePic(string name)
        {
            return Db.SystemConfigurations.SingleOrDefault(s => s.key.ToLower() == name).Value;
        }
        public string GetIconUrl(string name)
        {
            return Db.SystemConfigurations.SingleOrDefault(s => s.key.ToLower() == name).Value;
        }
    }
}