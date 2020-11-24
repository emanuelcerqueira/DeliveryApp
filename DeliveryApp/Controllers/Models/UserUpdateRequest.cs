using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Controller.Models
{
    public class UserUpdateRequest
    {

        [EmailAddress(ErrorMessage = "Invalid e-mail")]
        public string Email {get; set;}

        [Required(ErrorMessage = "{0} must not be empty")]
        [StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string Name {get; set;}

        [Required(ErrorMessage = "{0} must not be empty")]
        public string Password {get; set;}
        
        public string Telephone {get; set;}

    }
}