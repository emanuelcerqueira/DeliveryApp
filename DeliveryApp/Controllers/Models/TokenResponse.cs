
using System;
using DeliveryApp.Services.Models;

namespace DeliveryApp.Controller.Models
{
    public class TokenResponse
    {
        public string JwtToken { get; set; }
        public UserResponse User {get; set;}
        public DateTime Expires { get; set; }

        public TokenResponse(Token token)
        {
            JwtToken = token.JwtToken;
            User = new UserResponse(token.User);
            Expires = token.Expires;
        }
    }    
}