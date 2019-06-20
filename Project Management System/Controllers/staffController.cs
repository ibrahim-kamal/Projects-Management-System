using Project_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Project_Management_System.Controllers
{
    public class staffController : Controller
    {
        Project_ManagementEntities6 db = new Project_ManagementEntities6();
        // TL and PM
        /*public ActionResult leaveProject(int userid, int projectid)
		{
			var firedperson = from d in db.teams
							  where d.user_id == userid && d.project_id == projectid
							  select d.user_remove;
			var usrfire = Convert.ToInt32(firedperson.ToList());
			usrfire = 1;
			// it will be removed from ui in the project
			return View();
		}*/

        [Route("staff/leaveProject/{userid}/{projectid}")]
        public void leaveProject(int userid, int projectid)
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
                U.user_remove = 1;
                db.SaveChanges();
            }
        }


        public ActionResult showFeedback(int userid) // in Home Page
        {
            /*var jdid = from d in db.JDs
                       where (d.user_id == userid_jd)
                       select d.JD_id;
 int jd_id = Convert.ToInt32(jdid);*/
            int jd_id = db.JDs.Single(j => j.user_id == userid).id;
            /*var fbk = from f in db.feedbacks
                      where (f.JD_id == jd_id)
                      select f;*/
            var fbk = db.feedbacks.ToList().Where(f => f.JD_id == jd_id);
            return View(fbk);
        }
        public ActionResult show_request()
        {
            int user_id = Convert.ToInt32(Session["user_id"].ToString());
            var team = db.teams.Include(j => j.project);
            var data = team.ToList().Where(u => u.user_id == user_id).Where(u => u.user_state == 1);
            return View(data);
        }

        public ActionResult setFeedback(int projectid, int jd_id, String feedbackcontent, int rating) // in Home Page
        {
            // tl id get by session & pm_id will get from project_id -> userid-> pmtable -> pm id
            if (Session["type_id"].ToString() == "3") // tl
            {
                int tlid = Convert.ToInt32(Session["id"]);
                /*  var tl_id = from d in db.TLs
                             where (d.user_id == tlid)
                             select d.TL_id;*/
                int tl_id = db.TLs.Single(t => t.user_id == tlid).id;
                /* var pm_id = from d in db.projects
                             where (d.project_id == projectid)
                             select d.user_id;*/
                int pm_id = db.projects.Single(t => t.id == projectid).user_id;
                /*  var pmid = from d in db.PMS
                               where (d.user_id.Equals(pm_id))
                               select d.PM_id;*/
                int pmid = db.PMS.Single(t => t.user_id == pm_id).user_id;

                Models.feedback f = new Models.feedback()
                {
                    rate = rating,
                    content = feedbackcontent,
                    JD_id = jd_id,
                    TL_id = tl_id,
                    PM_id = pmid,
                    project_id = projectid,
                    date = DateTime.Now
                };
                db.feedbacks.Add(f);
                db.SaveChanges();
            }

            return View();
        }
    }
}