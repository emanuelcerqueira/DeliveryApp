using System.Threading.Tasks;
using DeliveryApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DeliveryApp.Data;
using System;

namespace DeliveryApp.Repository
{
    public interface IUserRepository
    {
        Task<User> Save(User user);
        Task<User> FindUserByEmail(string email);
        Task<User> FindUserById(Guid id);
        Task<User> UpdateUser(User user);
        Task<int> GetNumberOfCustomers();
        Task<int> GetNumberOfDeliverymen();
    }

    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        
        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> FindUserById(Guid id)
        {
            var user = await _context.Users.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> Save(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

                public async Task<int> GetNumberOfCustomers()
        {
            int numberOfCustomers = await _context.Users
                .Where(user => user.Role == Role.Customer)
                .CountAsync();
            return numberOfCustomers;
        }

        public async Task<int> GetNumberOfDeliverymen()
        {
            int numberOfDeliverymen = await _context.Users
                .Where(user => user.Role == Role.Deliveryman)
                .CountAsync();
            return numberOfDeliverymen;
        }

    }

}