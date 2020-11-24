using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeliveryApp.Services.Exceptions;

namespace DeliveryApp.Models
{
    [Table("delivery")]
    public class Delivery
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; internal set;}

        [Required]
        [Column("request_date")]
        public DateTime RequestDate {get; set;}

        [Column("delivery_date")]
        public DateTime? DeliveryDate {get; set;}

        [Required]
        [ForeignKey("customer_id")]
        public User Customer {get; set;}
        
        [ForeignKey("deliveryman_id")]
        public User Deliveryman {get; set;}

        [Required]
        [ForeignKey("object_id")]
        public TransportedObject TransportedObject {get; set;}

        [Required]
        [Column("status")]
        public DeliveryStatus Status {get; private set;}

        [Column("notes")]
        public string Notes {get; set;}

        [Required]
        [ForeignKey("initial_location_id")]
        public Location InitialLocation {get; set;}

        [Required]
        [ForeignKey("delivery_location_id")]
        public Location DeliveryLocation {get; set;}
        
        [Required]
        [Column("price")]
        public decimal Price {get; set;}

        [Required]
        [Column("deliveryman_earnings")]
        public Decimal DeliverymanEarnings { get; set; }

        [Column("distance")]
        public long Distance {get; set;}

        [NotMapped]
        public List<LocationDTO> Route {get; set;}


        public Delivery()
        {
            Status = DeliveryStatus.Requested;
            RequestDate = DateTime.Now;
        }

        public Delivery(
            User customer, 
            TransportedObject transportedObject, 
            Location initialLocation, 
            Location deliveryLocation, 
            decimal price,
            decimal deliverymanEarnings,
            long distance, 
            string notes,
            List<LocationDTO> route) : this()
        {
            Customer = customer;
            TransportedObject = transportedObject;
            InitialLocation = initialLocation;
            DeliveryLocation = deliveryLocation;
            Price = price;
            DeliverymanEarnings = deliverymanEarnings;
            Distance = distance;
            Notes = notes;
            this.Route = route;
        }

        public bool IsStatusRequested() => Status == DeliveryStatus.Requested;

        public bool IsStatusAccepted() => Status == DeliveryStatus.Accepted;

        public bool IsStatusCanceled() => Status == DeliveryStatus.Canceled;

        public bool IsStatusOnCarriage() => Status == DeliveryStatus.OnCarriage;

        public bool IsStatusDelivered() => Status == DeliveryStatus.Delivered;

        public bool CanDelete() => IsStatusRequested() || IsStatusCanceled();

        public bool CanCancel() => IsStatusRequested() || IsStatusAccepted();

        public void Accept(User deliveryman)
        {
            if (!IsStatusRequested())
                throw new BussinessException($"Is not possible to accept a delivery that it status is '{Status.ToString().ToLower()}'");
            
            Status = DeliveryStatus.Accepted;
            Deliveryman = deliveryman;
        }

        public void OnCarriage(User currentDeliveryman)
        {
            if (!IsStatusAccepted())
                throw new BussinessException($"Is not possible to carriage a delivery that it status is '{Status.ToString().ToLower()}'");

            if (!isDeliveryAssociatedWith(currentDeliveryman))
                throw new BussinessException("Is not possible to interact with a delivery that is not associated with you.");

            Status = DeliveryStatus.OnCarriage;
        }


        public void Cancel(User currentUser)
        {
            if (!CanCancel())
                throw new BussinessException($"Is not possible to cancel a delivery that it status is '{Status.ToString().ToLower()}'");
                
            if (!isDeliveryAssociatedWith(currentUser))
                throw new BussinessException("Is not possible to cancel a delivery that is not associated with you.");

            Status = DeliveryStatus.Canceled;
        }

        public void Deliver(User currentDeliveryman)
        {   
            if (!IsStatusOnCarriage())
                throw new BussinessException($"Is not possible to deliver a delivery that it status is '{Status.ToString().ToLower()}'");
                
            if (!isDeliveryAssociatedWith(currentDeliveryman))
                throw new BussinessException("Is not possible to deliver a delivery that is not associated with you.");

            Status = DeliveryStatus.Delivered;
            DeliveryDate = DateTime.Now;
        }

        public bool isDeliveryAssociatedWith(User user)
        {
            if (user is null)
                return false;

            return user.Equals(Deliveryman) || user.Equals(Customer);
        }
        
    }
}