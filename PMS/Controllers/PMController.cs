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
    public class PMController : Controller
    {
        //
        // GET: /PM/
        
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

            data.Add("fname",new List<string>());
            data.Add("lname",new List<string>());
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

        
	}
}