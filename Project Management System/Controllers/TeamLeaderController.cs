using Project_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace Project_Management_System.Controllers
{
    public class TeamLeaderController : Controller
    {
        private Project_ManagementEntities6 db = new Project_ManagementEntities6();
        public ActionResult SetQualificationTL()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SetQualificationTL(TL P)
        {
            //int x = 2;
            Project_ManagementEntities6 db = new Project_ManagementEntities6();
            if (ModelState.IsValid)
            {
                P.user_id = Convert.ToInt32(Session["id"]);

                db.TLs.Add(P);
                db.SaveChanges();
                return RedirectToAction("../Home/newHomePage");

            }

            return View(P);

        }
        public void delete_member(int userid, int projectid)
        {
            var teams = db.teams.ToList().Where(t => t.project_id == projectid).Where(t => t.user_id == userid);
            int id = -1;
            foreach (var t in teams)
            {
                id = t.id;
            }
            if (id != -1)
            {
                var U = db.teams.Single(u => u.id == id);
                U.user_remove = 2;
                db.SaveChanges();
            }
        }

        public ActionResult checkRequestsForJoiningProject()
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

        public ActionResult sendRequestForJoining() // in profile
        {
            return View();
        }
        public ActionResult evaluateJD() // in profile
                                         // evaluate jd , remove jd ,leave project
        {
            return View();
        }
        public ActionResult sendFeedback() // in profile
        {
            return View();
        }
        public void send_feedback(object sender, EventArgs e)
        {
            String projectId = Convert.ToString(Request.Form["Project_id"]);
            String juniorId = Convert.ToString(Request.Form["Junior_id"]);
            String rate = Convert.ToString(Request.Form["rate"]);
            String content = Convert.ToString(Request.Form["content"]);
            String TeamLeader_id = Session["type_id"].ToString();

            String date = DateTime.Now.ToString("MM/dd/yyyy");
            String ProjectManager_id = "select (PM_id) from request when project_id = " + projectId + "and request_state = 0";
            String queryString = "insert into feedback values((select (max(id)+1)  from feedback)," + rate + "," + content + "," + ProjectManager_id + "," + TeamLeader_id + "," + juniorId + "," + projectId + "," + date + ")";
            var connectionString = ConfigurationManager.ConnectionStrings["Database1Entities"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    connection.Close();
            }

        }
        // GET: TL



        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["type"] != null)
            {
                string type = Session["type"].ToString();
                if (type == "1")
                {
                    JD jD = db.JDs.Find(id);
                    db.JDs.Remove(jD);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return Redirect(Url.Content("~/"));
        }

        public ActionResult show_jds()
        {
            if (Session["type"] != null)
            {
                string type = Session["type"].ToString();
                if (type == "2" || type == "3")
                {
                    var jDs = db.JDs.Include(j => j.user);
                    return View(jDs.ToList());
                }
            }
            return Redirect(Url.Content("~/"));
        }
        public ActionResult show_jds(string name)
        {
            if (Session["type"] != null)
            {
                string type = Session["type"].ToString();
                if (type == "2" || type == "3")
                {

                    var jDs = db.JDs.Include(j => j.user);
                    return View(jDs.ToList().Where(j => j.user.fname == name));
                }
            }
            return Redirect(Url.Content("~/"));
        }

        public ActionResult RemoveJD(team JD_project)
        {
            if (Session["type"] != null)
            {
                string queryString = "delete from team where user_id =  " + JD_project.user_id + " and project_id =" + JD_project.project_id;
                string type = Session["type"].ToString();
                if (type == "1")
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["Database1Entities"].ConnectionString;
                    using (var connection = new SqlConnection(connectionString))
                    {
                        var command = new SqlCommand(queryString, connection);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                            connection.Close();
                    }
                }
            }
            return Redirect(Url.Content("~/"));
        }



        public ActionResult select_qualication(int id)
        {
            var tlinstance = db.comments.Find(id);
            if (tlinstance != null)
            {
                var value1 = from t in db.TLs
                             where t.user_id == id
                             select t.decision_making;
                int val1 = Convert.ToInt32(value1);

                var value2 = from t in db.TLs
                             where t.user_id == id
                             select t.excellent_communication;
                int val2 = Convert.ToInt32(value2);

            }
            return View();
        }
        public ActionResult Approve(int userid , int projectid)
        {
            var data = from d in db.teams
                       where d.user_id == userid && d.project_id == projectid
                       select d.user_state;
            var state = Convert.ToInt32(data);
            state = 0;

            return View();
        }
        public ActionResult Disapprove(int userid, int projectid)
        {
            var data = from d in db.teams
                       where d.user_id == userid && d.project_id == projectid
                       select d;
          //  db.teams.Remove(data);
            return View();

        }

    }
}