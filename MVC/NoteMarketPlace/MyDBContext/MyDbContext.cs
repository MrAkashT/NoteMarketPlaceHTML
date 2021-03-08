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
        // User PRofile

        public IEnumerable<Country> GetCountries()
        {
            return Db.Countries.Where(c => c.IsActive == true).ToList();
        }
        public IEnumerable<ReferenceData> GetGender()
        {
            return Db.ReferenceDatas.Where(m => m.RefCategory.Equals("Gender")).Where(m => m.IsActive == true);
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
            return Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Selling Mode") && c.IsActive == true);
        }
        public string GetSellingModeById(int id)
        {
            var sellMode = Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Selling Mode") && c.IsActive == true);
            return sellMode.SingleOrDefault(s => s.ID == id).Value;
        }
        public string GetNoteStatusById(int id)
        {
            var ReferenceInDB = Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Notes Status") && c.IsActive == true);
            return ReferenceInDB.SingleOrDefault(c => c.ID == id).Value;
        }
        public bool GetIsPaidStatus(int SellingModeId)
        {
            var SellingMode =  Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Selling Mode") && c.IsActive==true);
            foreach (var i in SellingMode)
            {
                if (SellingModeId == i.ID && i.Value == "Paid" && i.IsActive == true)
                    return true;
                else if (SellingModeId == i.ID && i.Value == "Free" && i.IsActive == true)
                    return false;
            }
            return false;
        }
        
        public int GetStatusId(string name)
        {
            var status = Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Notes Status") && c.IsActive == true);
            return status.SingleOrDefault(s => s.Value == name).ID;
        }
        public int AddSellerNote(SellerNote Note)
        {
            Db.SellerNotes.Add(Note);
            Db.SaveChanges();
            return Note.ID;
        }
        public List<DashBoard> GetSellerDraftNoteBySellerId(int SellerId)
        {
            //return Db.SellerNotes.Where(s => s.SellerID == SellerId && s.IsActive == true);
            return (from sellernote in Db.SellerNotes
                   join category in Db.NoteCategories on sellernote.Category equals category.ID
                   join status in Db.ReferenceDatas on sellernote.Status equals status.ID
                   where sellernote.SellerID == SellerId && sellernote.Status != 9 && sellernote.Status != 10 && sellernote.IsActive == true
                   && category.IsActive == true && status.IsActive == true orderby sellernote.CreatedDate
                   descending
                   select new DashBoard
                   {
                       NoteId = sellernote.ID,
                       AddedDate =(DateTime) sellernote.CreatedDate,
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
                    where sellernote.SellerID == SellerId && sellernote.Status == 9 && sellernote.IsActive == true
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
                        sellernote.SellerID == SellerId && sellernote.Status == 9 && sellernote.IsActive == true
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
                        where sellernote.SellerID == SellerId && sellernote.Status == 9 && sellernote.IsActive == true
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
                        sellernote.SellerID == SellerId && sellernote.Status != 9 && sellernote.Status != 10 && sellernote.IsActive == true
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
                        where sellernote.SellerID == SellerId && sellernote.Status != 9 && sellernote.Status != 10 && sellernote.IsActive == true
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
                        sellerNote = note,
                       // Country = country.Name,
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
                var forId = Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Selling Mode") && c.IsActive ==true);
                return forId.SingleOrDefault(c => c.Value == "Paid").ID;
            }
            else
            {
                var forId = Db.ReferenceDatas.Where(c => c.RefCategory.Equals("Selling Mode") && c.IsActive == true);
                return forId.SingleOrDefault(c => c.Value == "Free").ID;
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

        public List<SellerNote> GetSellerNote()
        {
            Db.Configuration.ProxyCreationEnabled = false;
            return Db.SellerNotes.Where(n => n.Status == 9 && n.IsActive == true).ToList();
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

        public List<SellerNote> GetNotesByFilter(string search, int Category, int Type, string University, string Course, int Country)
        {
            Db.Configuration.ProxyCreationEnabled = false;
            search = search.ToLower();
            Course = Course.ToLower();
            University = University.ToLower();
            //return Db.SellerNotes.Where(c => c.Category == Category && c.NoteType == Type && c.Country == Country).ToList();
            var notes = Db.SellerNotes.Where(n => n.Status == 9 && n.IsActive == true).ToList();

            if(search != null)
                notes = notes.Where(c => c.Title.ToLower().Contains(search) && c.Status == 9 && c.IsActive == true).ToList();
            
            if(Category != 0)
                notes = notes.Where(c => c.Category == Category && c.Status == 9 && c.IsActive==true).ToList();
            
            if (Type != 0)
                notes = notes.Where(c => c.NoteType == Type && c.Status == 9 && c.IsActive == true).ToList();
            
            if (University != "0")
                notes = notes.Where(c => c.UniversityName.ToLower().Contains(University) && c.Status == 9 && c.IsActive == true).ToList();

            if (Course != "0")
                notes = notes.Where(c => c.Course.ToLower().Contains(Course) && c.Status == 9 && c.IsActive == true).ToList();
           
            if(Country != 0)
                notes = notes.Where(c => c.Country == Country && c.Status == 9 && c.IsActive == true).ToList();
            
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

        public void AddDownload(Download obj)
        {
            Db.Downloads.Add(obj);
            Db.SaveChanges();
        } 

        public IEnumerable<BuyerReq> GetBuyerReqData(int sellerId)
        {
            return (from download in Db.Downloads
                    join note in Db.SellerNotes on download.NoteID equals note.ID
                    join category in Db.NoteCategories on note.Category equals category.ID
                    join downloader in Db.Users on download.Downloader equals downloader.ID
                    join downloaderEmail in Db.UserProfiles on download.Downloader equals downloaderEmail.UserID
                    where download.Seller == sellerId && note.IsActive == true && downloader.IsActive == true
                    orderby download.CreatedDate descending
                    select new BuyerReq
                    {
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
}