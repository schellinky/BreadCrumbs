using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace BreadCrumbs.Models
{
    public enum TicketStatus
    {
        [Display(Name = "New")]
        New,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Closed")]
        Closed
        
    }

    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Column(TypeName ="nvarchar(250)")]
        [Required]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(4000)")]
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Created By User")]
        [Column(TypeName = "int")]
        [Required]
        public int CreatedByUser { get; set; }
          
        [Display(Name = "Ticket Status")]
        [Column(TypeName = "int")]
        [Required]
        public TicketStatus TicketStatus { get; set; }
    }
}
