using DeliveryApp.Models;
using DeliveryApp.Data;
using System.Threading.Tasks;
using DeliveryApp.Repository;
using DeliveryApp.Services.Models;

namespace DeliveryApp.Services 
{
    public interface IAuthService 
    {
        Task<Token> Login(string email, string password);
        Token RefreshToken();
    }

    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        private readonly ISecurityUtil _securityUtil;

        public AuthService(ITokenService tokenService, IUserRepository userRepository, ISecurityUtil securityUtil)
        {
             _tokenService = tokenService;
             _userRepository = userRepository;
             _securityUtil = securityUtil;
        }

        public async Task<Token> Login(string email, string password)
        {
            var user = await _userRepository.FindUserByEmail(email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;
                
            var generatedToken = _tokenService.GenerateToken(user);
            return generatedToken;
        }

        public Token RefreshToken()
        {
            var currentUser = _securityUtil.CurrentUser;

            if (currentUser == null)
            {
                return null;
            }

            var generatedToken = _tokenService.GenerateToken(currentUser);
            return generatedToken;
        }
    }


}