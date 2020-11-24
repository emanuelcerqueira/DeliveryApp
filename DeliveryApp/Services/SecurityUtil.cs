using DeliveryApp.Data;
using DeliveryApp.Models;
using DeliveryApp.Repository;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DeliveryApp.Services 
{
    public interface ISecurityUtil
    {
        User CurrentUser {get;}
    }
    public class SecurityUtil : ISecurityUtil
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserRepository _userRepository;

        public SecurityUtil(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public User CurrentUser 
        { 
            get
            {
                var currentUserEmail = _contextAccessor.HttpContext.User.Identity.Name;
                return _userRepository.FindUserByEmail(currentUserEmail).Result;
            }
        }
        
    } 
}