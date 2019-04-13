using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using PMS.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace PMS.Controllers
{
    public class TLController : Controller
    {
        //
        // GET: /TL/
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
        
	}
}