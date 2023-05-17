using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace SportsPro.Models
{
    public class Incident
    {
            [Key]
            public int IncidentId { get; set; } 

            [Required(ErrorMessage = "Test the name")]
            public string? Title { get; set; }

            [ForeignKey("Customer")]
            public int CustomerId { get; set; }
            public Customer? Customer { get; set; }


 [Required(ErrorMessage = "Please choose product from dropdown")]
            [ForeignKey("Product")]
            public int ProductId { get; set; }
            public Product? Product { get; set; }

           

        
        [Required(ErrorMessage = "Test the open date")]
        public DateTime DateOpened { get; set; }
            
            public DateTime? DateClosed { get; set; }

        [ForeignKey("Technician")]
            public int? TechnicianId { get; set; }

            public Technician? Technician { get; set; }

            public string? Description { get; set; }
        }
    }
