using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BreadCrumbs.Models
{
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

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public int CreatedByUser { get; set; }
    }
}
