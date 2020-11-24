
using System;
using System.ComponentModel.DataAnnotations;
using DeliveryApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeliveryApp.Controller.Models
{
    public class UserResponse
    {

        public Guid Id {get; internal set;}
        public string Email {get; set;}
        public string Name {get; set;}
        public string Telephone {get; set;}
        [JsonProperty("role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role {get; set;}

        public UserResponse(User user)
        {
            if (user != null)
            {
                Id = (Guid)(user?.Id);
                Email = user?.Email;
                Name = user?.Name;
                Telephone = user?.Telephone;
                Role = (Role)(user?.Role);

            }
        }

    }
}