
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeliveryApp.Controller.Models
{
    public class LocationAndRadiusQuery
    {
        [BindRequired]
        [FromQuery(Name = "lat")]
        [Range(-90, 90, ErrorMessage="Invalid latitude")]
        public double Lat {get; set;}

        [BindRequired]
        [FromQuery(Name = "lon")]
        [Range(-180, 180, ErrorMessage="Invalid longitude")]
        public double Lon {get; set;}

        [BindRequired]
        [FromQuery(Name = "radius")]
        [Range(1, 30)]
        public int Radius {get; set;}


    }
}       
       
