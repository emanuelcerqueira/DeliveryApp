using System.Collections.Generic;
using DeliveryApp.Models;
using System.Linq;
using Newtonsoft.Json;

namespace DeliveryApp.Services.Models
{
    public class DeliveryInfoDTO
    {
        public decimal Price {get; set;}
        
        [JsonProperty("routeInfo")]
        public RoutingInfoDTO RoutingInfoDTO {get; set;}

        public DeliveryInfoDTO(RoutingInfoDTO routingInfoDTO, decimal Price)
        {
            this.RoutingInfoDTO = routingInfoDTO;
            this.Price = Price;
        }
    }
}