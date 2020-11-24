using System;
using DeliveryApp.Models;

namespace DeliveryApp.Services.Models
{
    public class Token
    {
        public string JwtToken { get; set; }
        public User User {get; set;}
        public DateTime Expires { get; set; }

        public Token(string jwtToken, User user, DateTime expirationDateTime)
        {
            JwtToken = jwtToken;
            User = user;
            Expires = expirationDateTime;
        }
    }    
}