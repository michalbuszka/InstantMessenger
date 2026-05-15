using System.Security.Claims;
using System.Text;
using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using InstantMessenger.Application.DTOs.LoginRegister;

namespace InstantMessenger.Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserService(UserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<RegisterResponse> AddUserAsync(string username, string password)
        {
            RegisterRequest registerRequest = new (username, password);
            string[] messages = new string[10];
            await _userRepository.AddUserAsync(UserMapper.createUser(registerRequest));
            RegisterResponse registerResponse = new (0, messages, _jwtService.GenerateToken(username));
            return registerResponse;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}