using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeliveryApp.Models
{

    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; internal set;}

        [Required(ErrorMessage = "{0} must not be empty")]
        [EmailAddress(ErrorMessage = "Invalid e-mail")]
        [Column("email")]
        public string Email {get; set;}

        [Required(ErrorMessage = "{0} must not be empty")]
        [StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        [Column("name")]
        public string Name {get; set;}

        [Required(ErrorMessage = "{0} must not be empty")]
        [Column("password")]
        public string Password {get; set;}
        
        [Column("telephone")]
        public string Telephone {get; set;}

        [Required(ErrorMessage = "{0} must not be empty")]
        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        [Column("role")]
        public Role Role {get; set;}

        public User()
        {
        }

        public User(string email, string name, string password, string telephone, Role role)
        {
            Email = email;
            Name = name;
            Password = password;
            Telephone = telephone;
            Role = role;
        }

        public bool IsCustomer() => this.Role == Role.Customer;

        public bool IsDeliveryman() => this.Role == Role.Deliveryman;

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id.Equals(user.Id);
        }

        public override int GetHashCode() => HashCode.Combine(Email);
    }
}