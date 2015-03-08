using CMPS_383_Phase_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMPS_383_Phase_1.Helpers
{
    public class AccountHelper
    {
        private TimeClockApplicationDbContext db = new TimeClockApplicationDbContext();

        public AccountHelper()
        {
        }

        public int getUserId(string name)
        {
            var userId = (from p in db.User
                          where p.UserName == name
                          select p.UserId).FirstOrDefault();
            return userId;
        }

    }
}
