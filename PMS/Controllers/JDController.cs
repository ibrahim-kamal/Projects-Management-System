using System;
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
    public class JDController : Controller
    {
        //
        // GET: /JD/

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
	}
}