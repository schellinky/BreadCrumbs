using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BreadCrumbs.Models
{
    public enum TicketStatus
    {
        [Display(Name = "New")]
        New,
        [Display(Name = "Closed")]
        Closed,
        [Display(Name = "In Progress")]
        InProgress
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
        public string Description { get; set; }

        [Column(TypeName = "int")]
        [Required]
        public int CreatedByUser { get; set; }

        [Column(TypeName = "int")]
        [Required]
        public TicketStatus TicketStatus { get; set; }
    }
}
