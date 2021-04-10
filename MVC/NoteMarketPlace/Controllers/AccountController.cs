using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteMarketPlace.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using NoteMarketPlace.Filter;
using NoteMarketPlace.DbContext;
using System.IO;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{
    public class AccountController : Controller
    {
        public MyDbContext userRepo;

        public AccountController()
        {
            userRepo = new MyDbContext();
        }
        // GET: Account
        public ActionResult Login()
        {
            Login model = new Login();
            if(Request.Cookies["Login"] != null)
            {
                model.Email = Request.Cookies["Login"].Values["EmailID"];
                model.Password = Request.Cookies["Login"].Values["Password"];
                model.RememberMe = Convert.ToBoolean(Request.Cookies["Login"].Values["Remember"]);
            }
           
            return View(model);
        }
        
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
           
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            TempData["a"] = "logout";
            ViewBag.logout = "logout";
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult userLogin(Login user, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.err = "error";
                return View("Login", user);
            }
               

            var userInDb = userRepo.GetUser(user.Email);
            if(userInDb == null)
            {
                ModelState.AddModelError("EmailWrong", "Email or Password is incorrect");
                ViewBag.err = "error";
                return View("Login", user);
            }

            if (userInDb.IsEmailVerified)
            {
                if (userInDb.EmailID == user.Email && userInDb.Password == user.Password)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, user.RememberMe);
                    Session["userId"] = userInDb.ID;
                    Session["Email"] = userInDb.EmailID;


                    if (user.RememberMe)
                    {
                        HttpCookie cookie = new HttpCookie("Login");
                        cookie.Values.Add("EmailID", userInDb.EmailID);
                        cookie.Values.Add("Password", userInDb.Password);
                        cookie.Values.Add("Remember", user.RememberMe.ToString());
                        cookie.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        if(Request.Cookies["Login"] != null)
                        {
                            var cookie = Request.Cookies["Login"];
                            cookie.Expires = DateTime.Now.AddMilliseconds(1);
                            Response.Cookies.Add(cookie);
                        }
                    }

                    var role = userRepo.GetRoles(userInDb.RoleID);
                    if(role == "admin")
                    {
                        var adminDetails = userRepo.GetUserProfileDetails(userInDb.ID);
                        if (adminDetails.ProfilePicture == null)
                        {
                            var img = userRepo.GetDefaultUserPic("default image for user");
                            Session["defaultImg"] = img;
                        }
                        else
                        {
                            Session["AdminImg"] = adminDetails.ProfilePicture;
                        }

                        Session["admin"] = userInDb.ID;
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if(role == "super admin")
                    {
                        var adminDetails = userRepo.GetUserProfileDetails(userInDb.ID);
                        if(adminDetails != null)
                        {
                            if (adminDetails.ProfilePicture == null)
                            {
                                var img = userRepo.GetDefaultUserPic("default image for user");
                                Session["defaultImg"] = img;
                            }
                            else
                            {
                                Session["AdminImg"] = adminDetails.ProfilePicture;
                            }
                        }
                        else
                        {
                            var img = userRepo.GetDefaultUserPic("default image for user");
                            Session["defaultImg"] = img;
                        }

                        Session["admin"] = userInDb.ID;
                        Session["super admin"] = userInDb.ID;
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        var userFeelProfileDetails = userRepo.GetUserProfileInfo(userInDb.ID);

                        if (userFeelProfileDetails == null)
                        {
                            var img = userRepo.GetDefaultUserPic("default image for user");
                            Session["defaultImg"] = img;
                            return RedirectToAction("UserProfile", "User");
                        }  
                        else
                        {
                            if (userFeelProfileDetails.ProfilePicture == null)
                            {
                                var img = userRepo.GetDefaultUserPic("default image for user");
                                Session["defaultImg"] = img;
                            }
                            else 
                            {
                                Session["img"] = userFeelProfileDetails.ProfilePicture;
                            }
                            return RedirectToAction("SearchNotes", "Home");
                        }
                    }

                    
                }
                else
                {
                    ModelState.AddModelError("EmailWrong", "Email or Password is incorrect");
                    ViewBag.err = "error";
                    return View("Login", user);
                }
            }
            else
            {
                // Add model err that specify not verified.
                ModelState.AddModelError("NotVerified", "Email is not verified. Please check your email.");
                
                return View("Login", user);

            }
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserModel userModel)
        {
            if ( !ModelState.IsValid)
            {
                ViewBag.err = "error";
                return View("SignUp", userModel);
            }
               

            var IsExist = userRepo.CheckUserIsExistOrNot(userModel.EmailID);
            if (IsExist)
            {
                ModelState.AddModelError("Exist", "Email is Already Exists.");
                return View("SignUp", userModel);
            }
            var roleId = userRepo.GetRolesByName("member");
            var createuser = new User
            {
                RoleID = roleId,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailID = userModel.EmailID,
                IsEmailVerified = false,
                Password = userModel.Password,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsActive = true
            };
            
            userModel.Id = userRepo.AddUser(createuser);

            var verifyUrl = "/Account/EmailVerify/" + userModel.Id;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("akashthakar008@gmail.com", "NoteMarketPlace");
            var toEmail = new MailAddress(userModel.EmailID);
            var fromEmailPassword = "8460566920";
            string sub = "Note Marketplace - Email Verification";
            string body = "Hello  " + userModel.FirstName + "<br/><br/>" +
                "Thank you for signing up with us. Please click on below link to verify " +
                "your email address and to do login. <br/><br/>" +
                "<a href='" + link + "'>" + link + "</a><br/><br/>" +
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


            ViewBag.msg = "success";

            ModelState.Clear();

            return View("SignUp");
        }

       
        public ActionResult EmailVerify(int id)
        {
            UserModel model = new UserModel
            {
                Id = id
            };
            return View(model);
        }

        public ActionResult Verify(int id)
        {
            userRepo.Verify(id);

            return RedirectToAction("Login", "Account");  
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ForgotPassword user)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", user);
            }
            var userInDb = userRepo.GetUser(user.Email);
            if (userInDb == null)
            {
                ModelState.AddModelError("Wrong Email", "Please Check Your Email");
                return View("ForgotPassword", user);
            }
            if(user.Email == userInDb.EmailID)
            {
                userInDb.Password = "A@1aeeeeeeeeeeeeeeeeeeee";
                var fromEmail = new MailAddress("akashthakar008@gmail.com", "Akash");
                var toEmail = new MailAddress(user.Email);
                var fromEmailPassword = "8460566920";
                string sub = "New Temporary Password has been created for you";
                string body = "Hello, <br/><br/>" +
                    "We have generated a new password for you <br/>" +
                    "Password: A@1aeeeeeeeeeeeeeeeeeeee <br/>"+
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
                userRepo.UpdateUserProfile();
                //Message to the user
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
                
        }
        [UserAuthFilter]
        public ActionResult ChangeUserPassword()
        {
            return View();
        }

        [UserAuthFilter]
        [HttpPost]
        public ActionResult ChangeUserPassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.err = "error";
                return View(model);
            }

            int id = (int)Session["userId"];
            var userInDb = userRepo.GetUser(id);

            if(userInDb.Password == model.OldPassword)
            {
                userInDb.Password = model.NewPassword;
                userRepo.UpdateUp();
            }
            else
            {
                ModelState.AddModelError("incorrect", "Password is incorrect.");
                return View(model);
            }
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login", "Account");
        }
    }
}