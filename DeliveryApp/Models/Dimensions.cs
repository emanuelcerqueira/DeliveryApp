using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Models
{
    [Owned]
    public class Dimensions
    {
        [Required]
        [Range(0.1, 40, ErrorMessage = "{0} must be between {1} cm and {2} cm")]
        [Column("height")]
        public double Height {get; set;}
        
        [Range(0.1, 40, ErrorMessage = "{0} must be between {1} cm and {2} cm")]
        [Required]
        [Column("width")]
        public double Width {get; set;}

        [Range(0.1, 40, ErrorMessage = "{0} must be between {1} cm and {2} cm")]
        [Required]
        [Column("depth")]
        public double Depth {get; set;}

        [Range(0.05, 20, ErrorMessage = "{0} must be between {1} kg and {2} kg")]
        [Required]
        [Column("weight")]
        public double Weight {get; set;}
    }
}