using System;
using DeliveryApp.Models;
using DeliveryApp.Util;

namespace DeliveryApp.Controllers.Model
{
    public class DeliveryResponseDeliverymanQuery
    {
        public Guid Id {get; set;}

        public double RadiusDistance {get; set;}

        public DateTime RequestDate {get; set;}
 
        public string CustomerName {get; set;}

        public string CustomerTelephone {get; set;}

        public DeliveryStatus Status {get;}

        public string Notes {get; set;}

        public string InitialLocation {get; set;}

        public string DeliveryLocation {get; set;}
        
        public decimal DeliverymanEarnings {get; set;}
        public long DeliveryDistance {get; set;}
        
        public DeliveryResponseDeliverymanQuery(Delivery delivery, Location deliverymanCurrentLocation)
        {
            Id = delivery.Id;
            RadiusDistance = Haversine
                                .Distance(deliverymanCurrentLocation.Latitude,
                                        delivery.InitialLocation.Latitude,
                                        deliverymanCurrentLocation.Longitude,
                                        delivery.InitialLocation.Longitude);

            RequestDate = delivery.RequestDate;
            CustomerName = delivery.Customer.Name;
            CustomerTelephone = delivery.Customer.Telephone;
            Notes = delivery.Notes;
            DeliverymanEarnings = delivery.DeliverymanEarnings;
            DeliveryDistance = delivery.Distance;
            Status = delivery.Status;
            InitialLocation = delivery.InitialLocation.Name;
            DeliveryLocation = delivery.DeliveryLocation.Name;
        }


    }
}