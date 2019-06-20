using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Project_Management_System.Models;
namespace Project_Management_System.Controllers
{
    public class JuniorDeveloperController : Controller
    {
        public ActionResult SetQualificationJD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SetQualificationJD(JD P)
        {
            //int x = 2;
            Project_ManagementEntities6 db = new Project_ManagementEntities6();
            if (ModelState.IsValid)
            {
                P.user_id = Convert.ToInt32(Session["id"]);

                db.JDs.Add(P);
                db.SaveChanges();
                return RedirectToAction("../Home/newHomePage");

            }

            return View(P);

        }
        user upuser;
        Models.Project_ManagementEntities6 db = new Models.Project_ManagementEntities6();

        // GET: JuniorDeveloper
        public ActionResult request_leaving()
        {


            return View();
        }
        public ActionResult checkRequestsForJoiningProject()// in profile Page 
        {
            return View();
        }
        public ActionResult sendResponseForJoiningProject()// in profile Page 
        {
            return View();
        }
        public ActionResult showPreviousOrManagingProjects()
        {
            return View();
        }
        public ActionResult leaveProject()// in profile Page 
        {
            return View();
        }
        public void show_feedback(object sender, EventArgs e)
        {

            String Junior_id = Session["type_id"].ToString();
            String project_id = Convert.ToString(Request.Form["Project_id"]);

            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

            data.Add("fname", new List<string>());
            data.Add("lname", new List<string>());
            data.Add("user_id", new List<string>());
            data.Add("rate", new List<string>());
            //data.Add("content", new List<string>());
            string queryString = "select fname, lname , user_id,rate,content from feedback INNER JOIN (SELECT fname, lname , user_id FROM JDs INNER JOIN user ON JDs.user_id = user.id;) as table1 ON feedback.JD_id = table1.user_id where JD_id =" + Junior_id + " and project_id = " + project_id + ";";
            var connectionString = ConfigurationManager.ConnectionStrings["Database1Entities"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        data["fname"].Add(reader[0].ToString());
                        data["lname"].Add(reader[1].ToString());
                        data["user_id"].Add(reader[2].ToString());
                        data["content"].Add(reader[3].ToString());
                    }
                }
                connection.Close();
            }
        }
        public ActionResult select_qualification(int id)
        {
            var jdinstance = db.comments.Find(id);
            if (jdinstance != null)
            {
                var value1 = from d in db.JDs
                             where d.user_id == id
                             select d.coding_skills;
                int val1 = Convert.ToInt32(value1);

                var value2 = from d in db.JDs
                             where d.user_id == id
                             select d.system_design;
                int val2 = Convert.ToInt32(value2);

            }
            return View();
        }
        public ActionResult EditMyProfile(int id = 0)
        {
            if (id != 0)
            {
                /*  if ((Convert.ToInt32(Session["Type"]) == 1))
                  { */
                upuser = db.users.Find(id);

                if (upuser == null)
                {
                    return HttpNotFound();
                }
                //  ViewBag.type = user.type_id;
                return View(upuser);
                //                }
                /* else
                 {
                     return RedirectToAction("AllUser");
                 }*/
            }
            else
            {
                return RedirectToAction("Index");
              //  return RedirectToAction("../Home/Login");

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

                    return RedirectToAction("Index");
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