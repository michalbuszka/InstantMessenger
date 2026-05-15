using System.Security.Claims;
using System.Text;
using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace InstantMessenger.Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(UserRepository userRepository,  IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task AddUserAsync(string username, string password)
        {
            RegisterRequestDTO registerRequestDto = new RegisterRequestDTO(username, password);
            var issuer = _configuration["JwtConfig:Issuer"]!;
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration["JwtConfig:TokenValidityMins"];
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(Convert.ToDouble(tokenValidityMins));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = tokenExpiryTimeStamp,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            await _userRepository.AddUserAsync(UserMapper.createUser(registerRequestDto));
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}