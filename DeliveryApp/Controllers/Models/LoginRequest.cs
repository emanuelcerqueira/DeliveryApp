
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Controller.Models
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = "Invalid e-mail")]
        public string Email {get; set;}
        
        [Required(ErrorMessage = "Must not be empty")]
        public string Password {get; set;}
    }
}