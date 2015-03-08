using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMPS_383_Phase_1.Models
{
    public class TimeEntry
    {
        [Key]
        public int TimeEntryId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Time In")]
        public DateTime? TimeIn { get; set; }

        [Display(Name = "Time Out")]
        public DateTime? TimeOut { get; set; }

        public Boolean Flag { get; set; }

        public virtual Users User { get; set; }
    }
}