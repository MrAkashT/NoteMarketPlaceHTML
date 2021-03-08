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
            Session.Abandon();
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
                    

                    var userFeelProfileDetails = userRepo.GetUserProfileInfo(userInDb.ID);

                    if (userFeelProfileDetails == null)
                        return RedirectToAction("UserProfile", "User");
                    else
                        return RedirectToAction("../Home/SearchNotes", "Home");

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
               

            //userModel.Id = 1;

            var IsExist =userRepo.CheckUserIsExistOrNot(userModel.EmailID);
            if (IsExist)
            {
                ModelState.AddModelError("Exist", "Email is Already Exists.");
                return View("SignUp", userModel);
            }

            var createuser = new User
            {
                RoleID = 1,
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

            //return View("EmailVerify", userModel);
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
    }
}