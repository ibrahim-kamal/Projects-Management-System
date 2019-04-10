using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using PMS.Models;


namespace PMS.Controllers
{
    public class PMController : Controller
    {
        //
        // GET: /PM/
        public void send_feedback(object sender , EventArgs e)
        {
            PM pm = new PM();
            String Project_id =  Convert.ToString(Request.Form["Project_id"]);
            String Junior_id = Convert.ToString(Request.Form["Junior_id"]);
            String rate = Convert.ToString(Request.Form["rate"]);
            String content = Convert.ToString(Request.Form["content"]);
            String TeamLeader_id = Session["type_id"].ToString();
            pm.send_feedback(Project_id, Junior_id, rate, content, TeamLeader_id);

        }
        public void approve_disapprove_leaving(object sender, EventArgs e)
        {

            PM pm = new PM();
            String Project_id =  Convert.ToString(Request.Form["Project_id"]);
            String user_id = Convert.ToString(Request.Form["user_id"]);
            bool accept = Convert.ToBoolean(Request.Form["accept"]);
            pm.approve_disapprove_leaving(accept,user_id,Project_id);
        }
        
	}
}