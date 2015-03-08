using CMPS_383_Phase_1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;


namespace CMPS_383_Phase_1.Controllers
{


    public class UserController : Controller
    {


        TimeClockApplicationDbContext db = new TimeClockApplicationDbContext();
        //
        // GET: /User/
        [AuthorizeUser(Roles = "1")]
        [HttpGet]
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var users = from m in db.User
                        select m;
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }
            return View(users.ToList());
            

        }

        //[AuthorizeUser(Users = "Admin")]
        public ActionResult Search(string searchString)
        {
            var users = from m in db.User
                        select m;

            if (String.IsNullOrEmpty(searchString))
            {
                return RedirectToAction("NoUserFound");
            }
            else
            {
                users = users.Where(c => c.FirstName.Contains(searchString));

                if (!users.Any())
                {
                    return RedirectToAction("NoUserFound");
                }
                else
                {
                    return View(users);
                }
            }


        }

        // if no search results
        public ActionResult NoUserFound()
        {
            return View();
        }


        // GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        public ActionResult Login(Models.Users User)
        {
            if (ModelState.IsValid)
            {
                if (isValid(User.UserName, User.Password))
                {
                    FormsAuthentication.SetAuthCookie(db.User.Where(u => u.UserName == User.UserName).FirstOrDefault().UserName, false);

                    return RedirectToAction("Index", "TimeEntry");
                }
                else
                {
                    ModelState.AddModelError("", "Error processing credentials.");
                }
            }
            return View(User);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


        private bool isValid(string UserName, string Password)
        {
            bool isValid = false;

            var user = db.User.Where(u => u.UserName == UserName).FirstOrDefault();

            if (user != null)
            {
                if (Crypto.VerifyHashedPassword(user.Password, Password))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        // GET
        [AuthorizeUser(Roles = "1")]
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Role, "RoleId", "RoleName");
            return View();
        }

        // POST

       [AuthorizeUser(Roles = "1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId, UserName, FirstName, LastName, Password, RoleId")] Users User)
        {
            if(ModelState.IsValid)
            {
                User.Password = Crypto.HashPassword(User.Password);
                db.User.Add(User);

                if(db.User.Any(u => u.UserName == User.UserName))
                {
                    ViewBag.Duplicate = "Username not available.";
                    ViewBag.RoleId = new SelectList(db.Role, "RoleId", "RoleName");
                    return View(User);
                }
                else
                {
                    if (!System.Web.Security.Roles.RoleExists("1"))
                        System.Web.Security.Roles.CreateRole("1");
                    if (!System.Web.Security.Roles.RoleExists("2"))
                        System.Web.Security.Roles.CreateRole("2");
                    System.Web.Security.Roles.AddUserToRole(User.UserName, Convert.ToString(User.RoleId));
                    db.SaveChanges();
                    return RedirectToAction("Index", "User");
                }
            }
            ViewBag.RoleId = new SelectList(db.Role, "RoleId", "RoleName", User.RoleId);
            return View(User);
        }

        [AuthorizeUser(Roles="1")]

        // GET
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users User = db.User.Find(id);

            if(User == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Role, "RoleId", "RoleName", User.RoleId);
            return View(User);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit([Bind(Include = "UserId, UserName, FirstName, LastName,Password, RoleId")] Users users)
        {
            if (ModelState.IsValid)
            {
                users.Password = Crypto.HashPassword(users.Password);
              
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            ViewBag.RoleId = new SelectList(db.Role, "RoleId", "RoleName", users.RoleId);
            return View(users);
        }

        // GET
        [AuthorizeUser(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users User = db.User.Find(id);
            
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVerified(int id)
        {
            Users User = db.User.Find(id);
            db.User.Remove(User);
            db.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        // GET
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users User = db.User.Find(id);

            if (User == null)
            {
                return HttpNotFound();

            }
            return View(User);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}