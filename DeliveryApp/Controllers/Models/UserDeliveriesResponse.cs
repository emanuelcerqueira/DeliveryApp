using System;
using DeliveryApp.Models;
using DeliveryApp.Util;

namespace DeliveryApp.Controller.Models
{
    public class UserDeliveriesResponse
    {
        
        public Guid Id {get; set;}

        public DateTime RequestDate {get; set;}

        public DateTime? DeliveryDate {get; set;}

        //public User Deliveryman {get; set;}

        public decimal Price {get; set;}

        public decimal DeliverymanEarnings {get; set;}

        public long Distance {get; set;}

        public DeliveryStatus Status {get; set;}

        public string InitalLocation {get; set;}

        public string DeliveryLocation {get; set;}

        public UserDeliveriesResponse(Delivery delivery)
        {
            Id = delivery.Id;
            RequestDate = delivery.RequestDate;
            DeliveryDate = delivery.DeliveryDate;
            Price = delivery.Price;
            DeliverymanEarnings = delivery.DeliverymanEarnings;
            Distance = delivery.Distance;
            Status = delivery.Status;
            InitalLocation = delivery.InitialLocation.Name;
            DeliveryLocation = delivery.DeliveryLocation.Name;
        }

    }
}