
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DeliveryApp.Models
{
    [Table("location")]
    public class Location
    {

        [JsonIgnore]
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; internal set;}

        [Required]
        [Range(-90, 90, ErrorMessage="Invalid latitude")]
        [Column("latitude")]
        public double Latitude{get; set;}
        [Required]
        [Range(-180, 180, ErrorMessage="Invalid longitude")]
        [Column("longitude")]
        public double Longitude{get; set;}

        [Column("name")]
        public string Name{get; internal set;}


        public Location()
        {
            
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location(LocationDTO locationDTO)
        {
            Latitude = locationDTO.Latitude;
            Longitude = locationDTO.Longitude;
        }
    }
}