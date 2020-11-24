using System.ComponentModel.DataAnnotations;
using DeliveryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace DeliveryApp.Controller.Models
{
    public class DeliveryRequest
    {

        [Required]
        [JsonProperty("initialLocation")]
        public Location InitialLocation {get; set;}

        [Required]
        [JsonProperty("deliveryLocation")]
        public Location DeliveryLocation {get; set;}

        [Required]
        [JsonProperty("object")]
        public TransportedObject Object {get; set;}

        public string Notes {get; set;}

    }
}