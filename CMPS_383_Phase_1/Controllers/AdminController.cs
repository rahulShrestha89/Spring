using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMPS_383_Phase_1.Controllers
{
    public class AdminController : Controller
    {
        // This controller is about what admin can do
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}