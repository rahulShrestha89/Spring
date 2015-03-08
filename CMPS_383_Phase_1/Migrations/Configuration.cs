namespace CMPS_383_Phase_1.Migrations
{
    using CMPS_383_Phase_1.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<CMPS_383_Phase_1.Models.TimeClockApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CMPS_383_Phase_1.Models.TimeClockApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.


            string HashedAdminPassword = Crypto.HashPassword("selu2015");
            string HashedUserPassword = Crypto.HashPassword("password");

            context.Role.AddOrUpdate(
                new Roles { RoleId = 01, RoleName = "Admin" },
                new Roles { RoleId = 02, RoleName = "User" });

            context.User.AddOrUpdate(
                x => x.UserId,
                new Users { UserName = "383Admin", FirstName = "Admin", LastName = "Istrator", RoleId = 01, Password = HashedAdminPassword },
                new Users { UserName = "383dev", FirstName = "Shane", LastName = "Cao", RoleId = 02, Password = HashedUserPassword }     
            );
            //if (!System.Web.Security.Roles.RoleExists("1"))
            //    System.Web.Security.Roles.CreateRole("1");
            //if (!System.Web.Security.Roles.RoleExists("2"))
            //    System.Web.Security.Roles.CreateRole("2");
            //System.Web.Security.Roles.AddUserToRole("383Admin", "1");
            //System.Web.Security.Roles.AddUserToRole("383dev", "2");
        }
    }
}
