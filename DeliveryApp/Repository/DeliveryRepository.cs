using System.Threading.Tasks;
using DeliveryApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DeliveryApp.Data;
using System;
using System.Collections.Generic;

namespace DeliveryApp.Repository
{
    public interface IDeliveryRepository
    {
        Task<Delivery> Save(Delivery delivery);
        Task<Delivery> FindDeliveryById(Guid id);

        Task<List<Delivery>> FindDeliveriesByCustomer(User currentUser);
        Task<List<Delivery>> FindDeliveriesByDeliveryman(User currentUser);
        List<Delivery> FindRequestedDeliveriesByLocationAndRadius(Location location, int radius);
        Task<Delivery> Update(Delivery delivery);
        Task Remove(Delivery delivery);
        
        Task<int> GetNumberOfSuccessfulDeliveries();

    }

    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DatabaseContext _context;
        
        public DeliveryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Delivery>> FindDeliveriesByCustomer(User customer)
        {
            return await _context.Deliveries
                .Include(delivery => delivery.InitialLocation)
                .Include(delivery => delivery.DeliveryLocation)
                .Include(delivery => delivery.Customer)
                .Where(delivery => delivery.Customer.Equals(customer))
                .OrderByDescending(delivery => delivery.RequestDate)
                .ToListAsync();
        }

        public async Task<List<Delivery>> FindDeliveriesByDeliveryman(User deliveryman)
        {
            return await _context.Deliveries
                .Include(delivery => delivery.InitialLocation)
                .Include(delivery => delivery.DeliveryLocation)
                .Include(delivery => delivery.Deliveryman)
                .Where(delivery => delivery.Deliveryman.Equals(deliveryman))
                .OrderByDescending(delivery => delivery.RequestDate)
                .ToListAsync();
        }

        public async Task<Delivery> FindDeliveryById(Guid id)
        {
            return await _context.Deliveries
                .Include(delivery => delivery.InitialLocation)
                .Include(delivery => delivery.DeliveryLocation)
                .Include(delivery => delivery.Customer)
                .Include(delivery => delivery.Deliveryman)
                .Include(delivery => delivery.TransportedObject)
                .Where(delivery => delivery.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Delivery> Save(Delivery delivery)
        {
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();  
            return delivery;
        }
        public async Task<Delivery> Update(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();  
            return delivery;
        }

        public List<Delivery> FindRequestedDeliveriesByLocationAndRadius(Location location, int radius)
        {
            // https://gis.stackexchange.com/questions/31628/find-features-within-given-coordinates-and-distance-using-mysql
            var sql = $@"SELECT d.id, d.request_date, d.delivery_date, d.customer_id, d.deliveryman_id, d.object_id, d.status, d.notes, d.initial_location_id, d.delivery_location_id, d.price, d.deliveryman_earnings, d.distance
                    FROM delivery d
                    INNER JOIN location l ON (d.initial_location_id = l.id)
                    WHERE (6371 * Acos (Cos (Radians({location.Latitude})) * Cos(Radians(latitude)) * Cos(Radians(longitude) - Radians({location.Longitude})) + SIN (Radians({location.Latitude})) * Sin(Radians(latitude)))) < {radius}
                    ORDER  BY distance";

            var deliveries = _context.Deliveries
                                        .FromSqlRaw(sql)
                                        .Where(d => d.Status == DeliveryStatus.Requested)
                                        .Include(d => d.InitialLocation)
                                        .Include(d => d.DeliveryLocation)
                                        .Include(d => d.Customer)
                                        .ToList();
            
            return deliveries;
        }

        public async Task Remove(Delivery delivery)
        {
            _context.Deliveries.Remove(delivery);
            _context.Locations.Remove(delivery.InitialLocation);
            _context.Locations.Remove(delivery.DeliveryLocation);
            _context.TransportedObjects.Remove(delivery.TransportedObject);
            await _context.SaveChangesAsync();  
        }

        public async Task<int> GetNumberOfSuccessfulDeliveries()
        {
            int numberOfSuccessfulDeliveries = await _context.Deliveries
                .Where(delivery => delivery.Status == DeliveryStatus.Delivered)
                .CountAsync();

            return numberOfSuccessfulDeliveries;
        }
    }

}