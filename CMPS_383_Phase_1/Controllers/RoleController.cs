using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMPS_383_Phase_1.Controllers
{
    public class RoleController : Controller
    {
        // This controller work to define the role of users
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }
    }
}