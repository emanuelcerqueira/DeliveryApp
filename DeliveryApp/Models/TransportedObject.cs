using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DeliveryApp.Models
{
    [Table("transported_object")]
    public class TransportedObject
    {
        [JsonIgnore]
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; internal set;}
        [Column("description")]
        public string Description {get; set;}

        [Required]
        public Dimensions Dimensions {get; set;}

    }
}