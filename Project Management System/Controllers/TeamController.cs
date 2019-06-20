using Project_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Management_System.Controllers
{
    public class TeamController : Controller
    {
        private Project_ManagementEntities6 db = new Project_ManagementEntities6();
        // GET: Staff
        public ActionResult insert_qualification(int id, int type_id, int num1, int num2)
        {
            if (type_id == 2)
            {
                Models.PM pminstance = new Models.PM()
                {
                    user_id = id,
                    analytical = num1,
                    highly_organized = num2


                };
                db.PMS.Add(pminstance);
                db.SaveChanges();
            }
            else if (type_id == 3)
            {
                Models.TL tlinstance = new Models.TL()
                {
                    user_id = id,
                    decision_making = num1,
                    excellent_communication = num2


                };
                db.TLs.Add(tlinstance);
                db.SaveChanges();

            }
            else if (type_id == 4)
            {
                Models.JD jdinstance = new Models.JD()
                {
                    user_id = id,
                    coding_skills = num1,
                    system_design = num2


                };
                db.JDs.Add(jdinstance);
                db.SaveChanges();

            }


            return View();
        }
   
 }
}

