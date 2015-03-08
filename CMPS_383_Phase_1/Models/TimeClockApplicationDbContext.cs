using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CMPS_383_Phase_1.Models
{
    public class TimeClockApplicationDbContext : DbContext
    {
        public DbSet<Roles> Role { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<TimeEntry> TimeEntry { get; set; }
    }
}