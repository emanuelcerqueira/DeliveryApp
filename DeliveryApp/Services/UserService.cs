using DeliveryApp.Models;
using DeliveryApp.Data;
using System.Threading.Tasks;
using DeliveryApp.Repository;
using DeliveryApp.Controller.Models;
using System;
using DeliveryApp.Services.Exceptions;
using DeliveryApp.Service.Exception;

namespace DeliveryApp.Services 
{
    public interface IUserService 
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserById(Guid id);
        Task<User> UpdateUser(Guid id, UserUpdateRequest userUpdate);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityUtil _securityUtil;

        public UserService(IUserRepository userRepository, ISecurityUtil securityUtil)
        {
             _userRepository = userRepository;
             _securityUtil = securityUtil;
        }

        public async Task<User> CreateUser(User newUser)
        {
            await CheckDuplicatedEmailAsync(newUser);
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            newUser = await _userRepository.Save(newUser);
            return newUser;
        }

        private async Task CheckDuplicatedEmailAsync(User newUser)
        {
            var possibleUser = await _userRepository.FindUserByEmail(newUser.Email);

            if (possibleUser != null)
                throw new BussinessException("A user with this e-mail already exists");
        }

        public async Task<User> GetUserById(Guid id)
        {
            var currentUser =_securityUtil.CurrentUser;
            
            if (!id.Equals(currentUser.Id))
                throw new BussinessException("A user can only get data from itself.");
                
            var user = await _userRepository.FindUserById(id);;

            if (user == null)
                throw new ObjectNotFoundException("User not found");

            return user;
        }

        public async Task<User> UpdateUser(Guid id, UserUpdateRequest userUpdate)
        {
            var user = await GetUserById(id);

            user.Email = userUpdate.Email;
            user.Name = userUpdate.Name;
            user.Telephone = userUpdate.Telephone;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password);

            var updatedUser = await _userRepository.UpdateUser(user);

            return updatedUser;
        }
    }


}