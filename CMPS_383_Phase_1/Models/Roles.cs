using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMPS_383_Phase_1.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}