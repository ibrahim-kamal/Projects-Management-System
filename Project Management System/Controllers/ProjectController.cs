using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_Management_System.Models;

namespace ProjectManagementSystem.Controllers
{
    public class projectController : Controller
    {
		public ActionResult requestProject() {
			return View();
		}
		[HttpPost]
		public ActionResult requestProject(requestProject p)
		{
			Project_ManagementEntities6 db = new Project_ManagementEntities6();
			int pm_id = Convert.ToInt32(Session["type_user_id"]);

			if (ModelState.IsValid)
			{
				p.post_id = 10;
				p.PMs_id = pm_id;
				db.requestProjects.Add(p);
				db.SaveChanges();


				return View();
			}
			return View(p);
		}



		private Project_ManagementEntities6 db = new Project_ManagementEntities6();
		// GET: project
		public ActionResult project(int id)
		{
			int Project_id = id;

			
			String userid = Session["id"].ToString();
			ViewBag.type = Session["type_id"].ToString();
			Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
			data.Add("member_name", new List<string>());
			data.Add("member_id", new List<string>());
			data.Add("member_type_id", new List<string>());
			data.Add("member_state", new List<string>());
			data.Add("member_remove", new List<string>());
			data.Add("comment_id", new List<string>());
			data.Add("comment_photo", new List<string>());
			data.Add("comment_text", new List<string>());
			string title = "";
			string description = "";
			int state = -1;
			ViewBag.showcomment = false;
			var project_info = db.projects.ToList().Where(p => p.id == Project_id);
			var project_team = db.teams.Include(p => p.user);
			project_team = project_team.Where(t => t.project_id == Project_id).Where(t => t.user_state == 0);
			var project_comment = db.comments.ToList().Where(c => c.project_id == Project_id);
			var customer = db.requests.Include(u => u.user).ToList().Where(c => c.project_id == Project_id);
			var usr = project_team.Where(t => t.user_id.ToString() == (userid));

			ViewBag.project_id = Project_id;
			foreach (var m in usr) {
				ViewBag.user_remove = m.user_remove;
				ViewBag.user_state  = m.user_state;
				ViewBag.user_ID = m.user_id;
				ViewBag.user_photo = m.user.photo;
			}

			foreach (var m in customer) {
				ViewBag.customername = m.user.fname + " " + m.user.lname;
			}
			
			foreach (var m in project_info)
			{
				title = m.title;
				description = m.description;
				state = m.state;
			}


			foreach (var m in project_team)
			{
				data["member_name"].Add(m.user.fname + " " + m.user.lname);
				data["member_id"].Add(m.user_id.ToString());
				if (m.user_id.ToString() == userid) {
					ViewBag.showcomment = true;
				}
				data["member_type_id"].Add(m.user.type_id.ToString());
				data["member_state"].Add(m.user_state.ToString());
				data["member_remove"].Add(m.user_remove.ToString());
			}

			ViewBag.member_count = project_team.Count();

			foreach (var m in project_comment)
			{

				data["comment_id"].Add(m.id.ToString());
				data["comment_photo"].Add(m.user.photo);
				data["comment_text"].Add(m.text);
				
			}


			ViewBag.comment_count = project_comment.Count();


			ViewBag.title = title;
			ViewBag.description = description;
			ViewBag.state = state;


			data.Add("teamlaeder_id", new List<string>());
			data.Add("teamleader_name", new List<string>());
			data.Add("teamleader_photo", new List<string>());
			data.Add("jounior_id", new List<string>());
			data.Add("jounior_name", new List<string>());
			data.Add("jounior_photo", new List<string>());

			var teamleader = db.TLs.Include(u => u.user).ToList();
			var jounior = db.JDs.Include(u => u.user).ToList();
            Console.WriteLine(teamleader);
            Console.WriteLine(jounior);

            int jounior_count = 0; 
			foreach (var m in jounior)
			{
                var check = db.teams.ToList().Where(t => t.project_id == id).Where(t => t.user_id == m.user_id).Count();
                var check2 = db.teams.ToList().Where(t => t.project_id == id).Where(t => t.user_id == m.user_id);
                Console.WriteLine(check2);
                if (check == 0) {
					data["jounior_id"].Add(m.user_id.ToString());
					data["jounior_name"].Add(m.user.fname + " " + m.user.lname);
					data["jounior_photo"].Add(m.user.photo);
					jounior_count++;
				}
			}
			
				ViewBag.jounior_count = jounior_count;

			int teamleader_count = 0;
			foreach (var m in teamleader)
			{
                var check = db.teams.ToList().Where(t => t.project_id == id).Where(t => t.user_id == m.user_id).Count();
                var check3 = db.teams.ToList().Where(t => t.project_id == id).Where(t => t.user_id == m.user_id);
                Console.WriteLine(check3);

                if (check == 0)
				{
					data["teamlaeder_id"].Add(m.user_id.ToString());
					data["teamleader_name"].Add(m.user.fname + " " + m.user.lname);
					data["teamleader_photo"].Add(m.user.photo);
					teamleader_count++;
				}
			}


			
			ViewBag.teamleader_count = teamleader_count;
			

;

			ViewBag.data = data;
			return View();


		}

	}
}