using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace YourNamespace.Services
{
    public class UserService
    {
        // Simulate simple authentication (you can expand this as needed)
        public string Authenticate(string username, string password)
        {
            if (username == "admin" && password == "password") // Example check
            {
                return GenerateJwtToken(username);
            }
            return null;
        }

        // Generate JWT Token
        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("yoursecretkey"); // Symmetric key for signing
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddDays(7), // Expiration time of the token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Return the JWT token
        }
    }
}
