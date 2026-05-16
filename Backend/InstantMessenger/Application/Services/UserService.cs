using InstantMessenger.Application.DTOs.LoginRegister;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;

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
            string[] messages = { };
            RegisterResponse registerResponse;
            if (!await _userRepository.IsUsernameAvailable(username))
            {
                messages = new[] { "Username is already taken." };
                registerResponse = new(1, messages, string.Empty);
                return registerResponse;
            }

            RegisterRequest registerRequest = new(username, password);
            await _userRepository.AddUserAsync(UserMapper.createUser(registerRequest));
            registerResponse = new(0, messages, _jwtService.GenerateToken(username));
            return registerResponse;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}