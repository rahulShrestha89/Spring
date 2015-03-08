using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;


namespace CMPS_383_Phase_1.Models
{
        public class AuthorizeUser : AuthorizeAttribute
        {
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new System.Web.Mvc.RedirectResult("/User/Login");
            }

        }
    
}
   