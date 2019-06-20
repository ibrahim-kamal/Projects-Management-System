using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Management_System.Models;

namespace ProjectManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        Project_ManagementEntities6 DB = new Project_ManagementEntities6();
        // GET: Admin
        post postUpdate;
        user user;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Posts()
        {
            return View();
        }
        public ActionResult editProfile()
        {
            return View();
        }
        public ActionResult Approve(int id = 0)
        {
            post Post;
            if (id != 0)
            {

                Post = DB.posts.Find(id);

                if (Post == null)
                {
                    return HttpNotFound();
                }
                Post.state = 1;
                DB.Entry(Post).State = EntityState.Modified;

                DB.SaveChanges();
                return RedirectToAction("AllPosts");


            }
            else
            {
                return RedirectToAction("AllPosts");
            }
        }

        public ActionResult AllPosts()
        {

            return View(DB.posts.Include(u => u.user).ToList());
        }
        public ActionResult AllUser()
        {
            /*    if ((Convert.ToInt32(Session["type"]) == 1))
            {
                return View(DB.users.ToList());

            }
            else
            {
                return RedirectToAction("../Home/Login");
            }*/

            return View(DB.users.Include(u => u.type).ToList());
        }

        [HttpGet]
        public ActionResult AddUser()
        {

            /* if ((Convert.ToInt32(Session["type"]) == 1))
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }*/

            return View();

        }

        [HttpPost]
        public ActionResult AddUser(user user, HttpPostedFileBase file)
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
                    if (!DB.users.Any(model => model.email == user.email))
                    {

                        DB.users.Add(user);

                        DB.SaveChanges();
                        ViewBag.Message = "User save changes";

                        return RedirectToAction("AllUser");
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

        

        public ActionResult DeleteUser(int id)
        {
            try
            {
                user = DB.users.Find(id);

                DB.users.Remove(user);

                ViewBag.Error = "ImageDeleted";

                DB.SaveChanges();

                return RedirectToAction("AllUser");
            }

            catch
            {
                return View();
            }
        }
        public ActionResult UpdateUser(int id = 0)
        {
            if (id != 0)
            {
                /*  if ((Convert.ToInt32(Session["UserType"]) == 1))
                  { */
                user = DB.users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
                //                }
                /* else
                 {
                     return RedirectToAction("AllUser");
                 }*/
            }
            else
            {
                return RedirectToAction("AllUser");
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(user user, HttpPostedFileBase file)
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

                if (ModelState.IsValid)
                {

                    DB.Entry(user).State = EntityState.Modified;

                    DB.SaveChanges();

                    return RedirectToAction("AllUser");
                }
                return View(user);

            }
            catch
            {
                return View(user);
            }
        }
        public ActionResult MyProfile()
        {
            //   if ((Convert.ToInt32(Session["type"]) == 1))
            // {
            //   int UserID = Convert.ToInt32(Session["UserID"]);

            user = DB.users.Find(1);

            return View(user);
            /*   }
               else
               {
                   return RedirectToAction("index");
               }*/
        }
        public ActionResult AddPost()
        {
             if ((Convert.ToInt32(Session["UserType"]) == 1))
             {
                 return View();
             }
             else
             {
            return View();
            }
        }


        [HttpPost]
        public ActionResult AddPost(post post)
        {
            if (ModelState.IsValid)
            {
                  int id = Convert.ToInt32(Session["id"]);
                  var writer = DB.users.Where(model => model.id == id).FirstOrDefault();
                post.state = 0;
                post.user_id = writer.id;
                DB.posts.Add(post);
                DB.SaveChanges();


                return RedirectToAction("AllPosts");
            }
            return View(post);
        }
        public ActionResult UpdatePost(int id = 0)
        {
            if (id != 0)
            {
                /* if ((Convert.ToInt32(Session["type"]) == 1))
                 {*/
                postUpdate = DB.posts.Find(id);

                if (postUpdate == null)
                {
                    return HttpNotFound();
                }

                return View(postUpdate);
            }
            else
            {
                ViewBag.Message = "this operation is wrong";

                return RedirectToAction("AllPosts");
            }
        }
        /*  else
          {
              return RedirectToAction("../Home/Login");
          }

 */
        [HttpPost]
        public ActionResult UpdatePost(post post)
        {
            var U = DB.posts.Single(u => u.id == post.id);

            try
            {
                if (ModelState.IsValid)
                {
                    U.title = post.title;
                    U.description = post.description;

                    DB.SaveChanges();
                    return RedirectToAction("AllPosts");
                }
                else
                {
                    return RedirectToAction("AllPosts");
                }
            }
            catch
            {
                return RedirectToAction("AllPosts");
            }
        }
        post post;


       


        public ActionResult DeletePost(int id)
        {
            try
            {
                post = DB.posts.Find(id);

                DB.posts.Remove(post);


                DB.SaveChanges();

                return RedirectToAction("AllPosts");
            }

            catch
            {
                ViewBag.Message = "this operation is wrong";

                return RedirectToAction("AllPosts");
            }
        }


    }

}
