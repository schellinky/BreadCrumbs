using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BreadCrumbs.Models
{
    public class TicketViewModel
    {
        [Display(Name = "Ticket ID")]
        public int id { get; set; }
        [Display(Name = "Title")]
        public String title { get; set; }
        [Display(Name = "Description")]
        public String description { get; set; }
        [Display(Name = "User")]
        public String user { get; set; }
    }
}
