using Project_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace Project_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private Project_ManagementEntities6 db = new Project_ManagementEntities6();

        // GET: Home
        user updateUser;
        public ActionResult HomePage()
        {
            int user_id = Convert.ToInt32(Session["id"]);

            if (Convert.ToInt32(Session["type_id"]) == 2)   // pm
            {
                var list = db.projects.ToList().Where(p => p.user_id == user_id);
                return View(list);
            }
           
            return RedirectToAction("HomePage_staff","Home");
        }
        public ActionResult HomePage_staff()
        {
            int user_id = Convert.ToInt32(Session["id"]);


            var list = db.teams.Include(u => u.project).ToList().Where(u => u.user_id == user_id);
            return View(list);
        }
        public ActionResult myProfile()
        {
            int id = Convert.ToInt32(Session["id"]);
          if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (true)
            {
                ViewBag.link = "Post/AddPost"; // customer
            }
            else
            {
                ViewBag.link = "Requests/requestPost";
            }
            return View(user);
        }

        public ActionResult newHomePage()
        {
            var posts =  db.posts.Include(u => u.user).ToList();
            return View(posts);
        }
        public ActionResult adminmyProfile()
        {
            return View();
        }

        public ActionResult Login()
       {
           return View("Login");
       }

       
       public ActionResult Logout()
       {
           Session.Abandon();
           return View("Registration", "Home");
       }

        public ActionResult EditMyProfile(int id = 0)
        {
            if (id != 0)
            {
                if ((Convert.ToInt32(Session["Type"]) == 1))
                {
                    updateUser = db.users.Find(id);

                    if (updateUser == null)
                    {
                        return HttpNotFound();
                    }
                      ViewBag.type = user.type_id;
                    return View(updateUser);
                                    }
                     else
                     {
                         return RedirectToAction("AllUser");
                     }
                }
                else
                {
                    return RedirectToAction("../Home/HomePage");

                }
           }
        
        [HttpPost]
        public ActionResult EditMyProfile(user user, HttpPostedFileBase file)
        {
            var U = db.users.Single(u => u.id == user.id);

            try
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);

                    string imePath = Path.Combine(Server.MapPath("~/images/"), filename);
                    file.SaveAs(imePath);
                    user.photo = imePath;
                }

                if (ModelState.IsValid)
                {
                    U.job = user.job;
                    U.fname = user.fname;
                    U.lname = user.lname;
                    U.email = user.email;
                    U.mobile = user.mobile;
                    db.SaveChanges();
                    ViewBag.Message = "Success";

                    return RedirectToAction("newHomePage","Home");
                }
                return View(user);

            }
            catch
            {
                return View(user);
            }
        }




            // GET: Home
            user user;
            public ActionResult Index()
            {
                return View();
            }
           

            [HttpPost]
            public ActionResult Login(user person)
            {

                if (ModelState.IsValid)
                {
                    using (Project_ManagementEntities6 db = new Project_ManagementEntities6())
                    {
                        var v = db.users.Where(a => a.email == person.email && a.password == person.password).FirstOrDefault();

                        if (v != null)
                        {

                            Session["id"] = v.id;
                            Session["fname"] = v.fname;
                            Session["lname"] = v.lname;
                            Session["type_id"] = v.type_id;

                            if (Session["type_id"].Equals(1))
                            {
                                return RedirectToAction("newHomePage", "Home");
                            }
                            else if (Session["type_id"].Equals(2)) // pm
                            {
                            int c = db.PMS.ToList().Where(d => d.user_id == v.id).Count();
                            if (c == 0)
                            {
                                return RedirectToAction("SetQualificationPM", "ProjectManger");
                            }
                            Session["type_user_id"] = db.PMS.Single(p => p.user_id == v.id).id;
                            return RedirectToAction("newHomePage", "Home");
                            }
                            else if (Session["type_id"].Equals(3)) // tl
                        {
                            int c = db.TLs.ToList().Where(d => d.user_id == v.id).Count();
                            if (c == 0)
                            {
                                return RedirectToAction("SetQualificationTL", "TeamLeader");
                            }
                            Session["type_user_id"] = db.TLs.Single(t => t.user_id == v.id).id;
                            return RedirectToAction("newHomePage", "Home");
                            }
                            else if (Session["type_id"].Equals(4)) //  jd
                            {
                            int c = db.PMS.ToList().Where(d => d.user_id == v.id).Count();
                            if (c == 0)
                            {
                                return RedirectToAction("SetQualificationJD", "JuniorDeveloper");
                            }
                            Session["type_user_id"] = db.JDs.Single(p => p.user_id == v.id).id;
                                return RedirectToAction("newHomePage", "Home");
                            }
                            else if (Session["type_id"].Equals(5))
                            {
                                return RedirectToAction("newHomePage", "Home");
                            }
                            else
                            {
                                ViewBag.Login = "Email or Password Not Vaild .";
                                return RedirectToAction("Registration", "Home");
                            }

                        }
                    }
                }
                ViewBag.Login = "Email Or Password Not Vaild .";
                return RedirectToAction("Registration", "Home");
            }


          
            public ActionResult Registration()
            {
                return View();
            }
            [HttpPost]
            public ActionResult Registration(user user, HttpPostedFileBase file)
            {
                try
                {

                    if (file != null)
                    {
                        string filename = Path.GetFileName(file.FileName);

                        string imePath = Path.Combine(Server.MapPath("~/images/"), filename);
                        file.SaveAs(imePath);
                        user.photo = imePath;
                    }
                    else
                    {
                        ViewBag.Message = "Please choose Photo";
                        return View(user);

                    }
                    if (ModelState.IsValid)
                    {
                        if (!db.users.Any(model => model.email == user.email))
                        {

                            db.users.Add(user);

                            db.SaveChanges();
                            ViewBag.Message = "User save changes";

                            return RedirectToAction("../Home/Login");
                        }
                        ViewBag.Message = "User With This Email Already Exist";
                        return View(user);

                    }

                    return View(user);
                }
                catch
                {
                    return View(user);
                }
            }
        }
    

}
