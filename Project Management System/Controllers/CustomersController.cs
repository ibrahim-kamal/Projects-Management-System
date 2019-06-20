using Project_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Management_System.Controllers
{
    public class CustomersController : Controller
    {
        Project_ManagementEntities6 db = new Models.Project_ManagementEntities6();
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPost()
        {
            if ((Convert.ToInt32(Session["UserType"]) == 5))
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
                var writer = db.users.Where(model => model.id == id).FirstOrDefault();
                post.state = 0;
                post.user_id = writer.id;
                db.posts.Add(post);
                db.SaveChanges();


                return RedirectToAction("newHomePage","Home");
            }
            return View(post);
        }
        public ActionResult recieveRequestsFromPM()  // in profile
        {
            return View();
        }

        public ActionResult assignPMtoProject(int pm_id, int post_id) 
            // in profile page
        /* choose one pm and then go to another page to create group then it 
       is deleted from posts table but added to project table*/
        {
            var postinstance = db.posts.Find(post_id);
            var pminstance = db.posts.Find(pm_id);
            

            if (postinstance != null && pminstance != null)
            {
                var value1 = from d in db.posts
                             where d.id == post_id
                             select d; // post record
                var data_post = value1.ToList();

                var value2 = from d in db.requestProjects
                             where d.PMs_id == pm_id && d.post_id == post_id
                             select d; // request of project records
                var data_request_project = value2.ToList();
                var value3 = from d in db.PMS
                             where d.id == pm_id
                             select d.user_id; // pm id in user table
                Models.project proj = new Models.project()
                {

                    user_id = Convert.ToInt32(value3),
                    title = data_post[0].title,
                    description = data_post[0].description,
                    state = 0,
                    price = data_request_project[0].price,
                    project_remove = 0,
                    start_date = DateTime.Now,
                    end_date = DateTime.Today.AddDays(data_request_project[0].duration.Day)
                };
                db.projects.Add(proj);
                db.SaveChanges();

                var value4 = db.projects.Max(d => d.id);

                Models.request req = new Models.request()
                {

                    PM_id = Convert.ToInt32(value3),
                    user_id = data_post[0].user_id,
                    project_id = value4,
                    url = null ,
                    request_state = 0,
                   
                };
                db.requests.Add(req);
                db.SaveChanges();

            }
            var delpost = db.posts.Find(post_id);
            if (delpost != null)
            {
                // delete the record of the post that is changed to project
                db.posts.Remove(delpost);
                db.SaveChanges();

            }

            return View();
        }
        public ActionResult removeProject()  // in profile page
                                             // delete posts before it is assigned to pm
        {
            return View();
        }
        public ActionResult editProject() // in profile page
                                          // edit name or deadline of project
        {
            return View();
        }
        public ActionResult showPreviousOrManagingProjects()
        {
            return View();
        }
        // GET: Customers
    
   

        public ActionResult assignPMtoProject() // in profile page
                                                /* choose one pm and then go to another page to create group then it 
                                                is deleted from posts table but added to project table*/
        {

            return View();
        }
       

    }
}