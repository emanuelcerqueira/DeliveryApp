using System;
using System.Collections.Generic;
using DeliveryApp.Controller.Models;
using DeliveryApp.Models;

namespace DeliveryApp.Controllers.Model
{
    public class DeliveryResponse
    {
        public Guid Id { get; set; }
        public UserResponse Customer { get; set; }
        public UserResponse Deliveryman { get; set; }
        public TransportedObject Object { get; set; }
        public DeliveryStatus Status {get; set;}
        public string Notes {get; set;}
        public Location InitialLocation {get; set;}
        public Location DeliveryLocation {get; set;}
        public decimal Price {get; set;}

        public decimal DeliverymanEarnings {get; set;}

        public long Distance {get; set;}
        public List<LocationDTO> Route {get; set;}

        public DeliveryResponse(Delivery delivery)
        {
            Id = delivery.Id;
            Customer = new UserResponse(delivery.Customer);
            if (delivery.Deliveryman != null)
                Deliveryman = new UserResponse(delivery.Deliveryman);
            Object = delivery.TransportedObject;
            Status = delivery.Status;
            Notes = delivery.Notes;
            InitialLocation = delivery.InitialLocation;
            DeliveryLocation = delivery.DeliveryLocation;
            Price = delivery.Price;
            DeliverymanEarnings = delivery.DeliverymanEarnings;
            Distance = delivery.Distance;
            Route = delivery.Route;
        }
        
    }
}