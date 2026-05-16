using InstantMessenger.Application.DTOs.LoginRegister;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Application.Validators;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace InstantMessenger.Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RegisterValidator _registerValidator;

        public UserService(UserRepository userRepository, JwtService jwtService, IPasswordHasher<User> passwordHasher, RegisterValidator registerValidator)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _registerValidator = registerValidator;
        }

        public async Task<RegisterResponse> AddUserAsync(string username, string password)
        {
            List<string> messages = new List<string>();
            RegisterResponse registerResponse;
            if (!await _userRepository.IsUsernameAvailable(username))
            {
                messages.Add("Username is already taken");
                registerResponse = new(1, messages.ToArray(), string.Empty);
                return registerResponse;
            }
            RegisterRequest registerRequest = new(username, password);
            var result = await _registerValidator.ValidateAsync(registerRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                registerResponse = new(1, messages.ToArray(), string.Empty);
                return registerResponse;
            }
            var passwordHash = _passwordHasher.HashPassword(null!, password);
            var user = UserMapper.createUser(registerRequest, passwordHash);
            await _userRepository.AddUserAsync(user);
            registerResponse = new(0, messages.ToArray(), _jwtService.GenerateToken(username));
            return registerResponse;
        }
    }
}