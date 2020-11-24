using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryApp.Controllers.Model
{
    public class DeliveryParams
    {
        [BindRequired]
        [FromQuery(Name = "latInitial")]
        [Range(-90, 90, ErrorMessage="Invalid latitude")]
        public double InitialLat {get; set;}

        [BindRequired]
        [FromQuery(Name = "lonInitial")]
        [Range(-180, 180, ErrorMessage="Invalid longitude")]
        public double InitialLon {get; set;}

        [BindRequired]
        [FromQuery(Name = "latDelivery")]
        [Range(-90, 90, ErrorMessage="Invalid latitude")]
        public double DeliveryLat {get; set;}
     
        [BindRequired]
        [FromQuery(Name = "lonDelivery")]
        [Range(-180, 180, ErrorMessage="Invalid longitude")]
        public double DeliveryLon {get; set;}
    }

}