using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMPS_383_Phase_1.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public Boolean ClockedIn { get; set; }

        public int TimeEntryId { get; set; }

        public int RoleId { get; set; }

        public virtual Roles Role { get; set; }
    }
}