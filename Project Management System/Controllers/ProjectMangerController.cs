using Project_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace Project_Management_System.Controllers
{
    public class ProjectMangerController : Controller
    {
        private Project_ManagementEntities6 db = new Project_ManagementEntities6();
        public ActionResult SetQualificationPM()
        {
            return View();
        }

        public ActionResult show_request()
        {
            int pm_id = Convert.ToInt32(Session["type_id"].ToString());
            var project_assign = db.requests.Include(j => j.project);
            var data = project_assign.ToList().Where(u => u.PM_id == pm_id).Where(u => u.request_state == 0);
            return View(data);
        }

        [HttpPost]
        public ActionResult SetQualificationPM(PM P)
        {
            Project_ManagementEntities6 db = new Project_ManagementEntities6();
            if (ModelState.IsValid)
            {
                P.user_id = Convert.ToInt32(Session["id"]);
                db.PMS.Add(P);
                db.SaveChanges();
                return RedirectToAction("../Home/newHomePage");
            }
            return View(P);


        }

        // GET: ProjectManger
        public ActionResult requestProject() // in Home Page
        {
            return View();
        }


        public ActionResult deliverProject() // in Home Page
        {
            return View();
        }
        public ActionResult showPreviousOrManagingProjects() // in profile
        {
            return View();
        }

        public ActionResult searchForTLorJD() // in profile
        {
            return View();
        }
		[Route("ProjectManger/sendRequestForJoining/{userid}/{projectid}")]
		public ActionResult sendRequestForJoining(int userid, int projectid) // in profile
        {
            Models.team tm = new Models.team()
            {
                user_id = userid,
                project_id = projectid,
                user_state = 1,
                user_remove = 0
            };
           
            db.teams.Add(tm);
            db.SaveChanges();
            // it then should be remove the member that recieved the request from the ui
            return View();
        }

        public ActionResult fire(int userid , int projectid)
        {
            var firedperson = from d in db.teams
                              where d.id == userid && d.project_id == projectid
                              select d.user_remove;
             var usrfire = Convert.ToInt32( firedperson.ToList());
            usrfire = 2;
            // it will be removed from ui in the project
            return View();
        }
        public ActionResult controlProjects()
        // in profile Page (set schedule , set status , write comments , remove member , leave project)
        {
            return View();
        }
        public ActionResult approveRequestForLeaving()// in profile Page 
        {
            return View();
        }


        [Route("ProjectManger/leaveProject/{userid}/{projectid}")]
        public void leaveProject(int userid, int projectid)
        {
            var project = db.projects.Find(projectid);
            db.projects.Remove(project);
            db.SaveChanges();
        }

        public void request_project(requestProject requestProject)
        {
            string type = Session["type"].ToString();
            if (type == "2")
            {
                db.requestProjects.Add(requestProject);
                db.SaveChanges();
            }
        }

        public void approve_disapprove_leaving(object sender, EventArgs e)
        {

            PM pm = new PM();
            String project_id = Convert.ToString(Request.Form["Project_id"]);
            String user_id = Convert.ToString(Request.Form["user_id"]);
            bool accept = Convert.ToBoolean(Request.Form["accept"]);

            string queryString = "";
            if (!accept)
            {
                queryString = "update team set user_remove = 0 where user_id =  " + user_id + " and project_id =" + project_id;
            }
            else
            {
                queryString = "delete from team where user_id =  " + user_id + " and project_id =" + project_id;
            }
            var connectionString = ConfigurationManager.ConnectionStrings["Database1Entities"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    connection.Close();
            }
        }


        public void show_feedback(object sender, EventArgs e)
        {

            String project_Manager_id = Session["type_id"].ToString();
            String project_id = Convert.ToString(Request.Form["Project_id"]);

            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

            data.Add("fname", new List<string>());
            data.Add("lname", new List<string>());
            data.Add("user_id", new List<string>());
            //data.Add("rate", new List<string>());
            data.Add("content", new List<string>());
            string queryString = "select fname, lname , user_id,rate,content from feedback INNER JOIN (SELECT fname, lname , user_id FROM JDs INNER JOIN user ON JDs.user_id = user.id;) as table1 ON feedback.JD_id = table1.user_id where PM_id =" + project_Manager_id + " and project_id = " + project_id + ";";
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
        [Route("ProjectManger/AddComment/{project_id}/{user_id}/{text}")]
        public void AddComment(int project_id, int user_id, string text)
        {
            Models.comment c = new Models.comment()
            {
                project_id = project_id,
                user_id = user_id,
                text = text
            };
            db.comments.Add(c);
            db.SaveChanges();
           // return View();
        }

        public ActionResult EditComment(int id, string text)
        {
            var comment = db.comments.Find(id);
            if (comment != null)
            {

                comment.text = text;
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult DeletComment(int id)
        {
            var chickcomment = db.comments.Find(id);
            if (chickcomment != null)
            {
                db.comments.Remove(chickcomment);
                db.SaveChanges();

            }
            return View();
        }

        public ActionResult select_qualification(int id)
        {
            var pminstance = db.comments.Find(id);
            if (pminstance != null)
            {
                var value1 = from p in db.PMS
                             where p.user_id == id
                             select p.analytical;
                int val1 = Convert.ToInt32(value1);

                var value2 = from p in db.PMS
                             where p.user_id == id
                             select p.highly_organized;
                int val2 = Convert.ToInt32(value2);
            }
            return View();
        }

        [Route("ProjectManager/DisapproveLeaving/{project_id}/{user_id}")]
        public void DisapproveLeaving(int project_id, int user_id)
        {
            //PM pm = new PM();
            //String project_id = Convert.ToString(Request.Form["Project_id"]);
            //String user_id = Convert.ToString(Request.Form["user_id"]);
            //bool accept = Convert.ToBoolean(Request.Form["accept"]);

            var usr_team = db.teams.ToList().Where(t => t.project_id == project_id).Where(t => t.user_id == user_id);
            int id = -1;
            foreach (var m in usr_team)
            {
                id = m.id;
            }
            var member = db.teams.Single(t => t.id == id);
            member.user_remove = 0;
            db.SaveChanges();
        }

        [Route("ProjectManager/ApproveLeaving/{project_id}/{user_id}")]
        public void ApproveLeaving(int project_id, int user_id)
        {
            //return "hhh";

            var usr_team = db.teams.ToList().Where(t => t.project_id == project_id).Where(t => t.user_id == user_id);

            int id = -1;
            foreach (var m in usr_team)
            {
                id = m.id;
            }
            var member = db.teams.Single(t => t.id == id);
            db.teams.Remove(member);
            db.SaveChanges();

        }

        // GET: ProjectManger
        public ActionResult Index()
        {
            return View();
        }


        [Route("ProjectManger/approve/{project_id}/{user_id}")]
        public void approve(int project_id, int user_id = 14)
        {
            //public ActionResult ApproveLeaving()
            //{

            var usr_team = db.teams.ToList().Where(t => t.project_id == project_id).Where(t => t.user_id == user_id);

            int id = -1;
            foreach (var m in usr_team)
            {
                id = m.id;
            }
            if (id != -1)
            {
                var member = db.teams.Single(t => t.id == id);
                db.teams.Remove(member);
                db.SaveChanges();

            }



            //}
            //	return View();
        }

        [HttpPost]
        public void delivery(string url, int project_id)
        {
            int PM_id = Convert.ToInt32(Session["pm_id"]);
            int customer_id = db.requests.Single(req => req.project_id == project_id).user_id;
            request r = new request()
            {
                PM_id = PM_id,
                user_id = customer_id,
                project_id = project_id,
                url = url,
                request_state = 1,
            };
            db.requests.Add(r);
            db.SaveChanges();


            var p = db.projects.Single(t => t.id == project_id);
            p.state = 1;
            db.SaveChanges();

            //url+"  |||  "+project_id;
        }

        [Route("ProjectManger/Disapprove/{project_id}/{user_id}")]
        public void Disapprove(int project_id, int user_id)
        {
            //PM pm = new PM();
            //String project_id = Convert.ToString(Request.Form["Project_id"]);
            //String user_id = Convert.ToString(Request.Form["user_id"]);
            //bool accept = Convert.ToBoolean(Request.Form["accept"]);

            var usr_team = db.teams.ToList().Where(t => t.project_id == project_id).Where(t => t.user_id == user_id);
            int id = -1;
            foreach (var m in usr_team)
            {
                id = m.id;
            }
            if (id == -1)
            {
                var member = db.teams.Single(t => t.id == id);
                member.user_remove = 0;
                db.SaveChanges();
            }

        }

    }
}