using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Management_System.Models;


namespace Project_Management_System.Controllers
{
    public class PostController : Controller
    {
        private Project_ManagementEntities6 db = new Project_ManagementEntities6();
        public ActionResult postContent()
        {
            return View();
        }


        public ActionResult show_post()
        {
           /* if (Session["type"] != null)
            {
                var posts = db.posts.Include(d => d.user);
                string type = Session["type"].ToString();
                if (type == "5")
                {
                    string customer_id = Session["type_id"].ToString();
                    return View(posts.ToList().Where(p => p.user_id == 5));
                }
                else if (type == "1" || type == "2")
                {

                    return View(posts.ToList());
                }

            }*/
            return Redirect(Url.Content("~/"));

        }
        public ActionResult AddPost()
        {
            return View();
        }
    }
}