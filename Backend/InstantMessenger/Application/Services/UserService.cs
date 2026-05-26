using InstantMessenger.Application.DTOs;
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
        private readonly LoginValidator _loginValidator;

        public UserService(UserRepository userRepository, JwtService jwtService, IPasswordHasher<User> passwordHasher,
            RegisterValidator registerValidator, LoginValidator loginValidator)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        public async Task<LoginRegisterResponse> AddUserAsync(RegisterRequest registerRequest)
        {
            List<string> messages = new List<string>();
            LoginRegisterResponse loginRegisterResponse;
            var result = await _registerValidator.ValidateAsync(registerRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return loginRegisterResponse;
            }

            if (!await _userRepository.IsUsernameAvailableAsync(registerRequest.Username))
            {
                messages.Add("Username is already taken");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return loginRegisterResponse;
            }

            var user = UserMapper.CreateUser(registerRequest, string.Empty);
            var passwordHash = _passwordHasher.HashPassword(user, registerRequest.Password);
            user.PasswordHash = passwordHash;
            await _userRepository.AddUserAsync(user);
            loginRegisterResponse = new(0, messages.ToArray(), _jwtService.GenerateToken(registerRequest.Username));
            return loginRegisterResponse;
        }

        public async Task<LoginRegisterResponse> LoginUserAsync(LoginRequest loginRequest)
        {
            List<string> messages = new List<string>();
            LoginRegisterResponse loginRegisterResponse;
            var result = await _loginValidator.ValidateAsync(loginRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return loginRegisterResponse;
            }

            if (await _userRepository.IsUsernameAvailableAsync(loginRequest.Username))
            {
                messages.Add("Invalid username or password.");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return loginRegisterResponse;
            }

            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) !=
                PasswordVerificationResult.Success)
            {
                messages.Add("Invalid username or password.");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return loginRegisterResponse;
            }

            loginRegisterResponse = new(0, messages.ToArray(), _jwtService.GenerateToken(loginRequest.Username));
            return loginRegisterResponse;
        }

        public async Task<bool> UpdateUserDataAsync(string? username, UserDTO.UserSettingsDTO userSettings)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return false;
            UserMapper.UpdateUser(user, userSettings);
            await _userRepository.SaveUserAsync();
            return true;
        }

        public async Task<UserDTO.UserSettingsDTO?> GetUserDataAsync(string? username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return null;
            return new UserDTO.UserSettingsDTO(user.Email, user.FirstName, user.LastName, user.Nick, user.Avatar);
        }

        public async Task<List<UserDTO.ContactDTO>?> GetUsersByNickQuery(string? nickQuery)
        {
            if (nickQuery == null)
                return null;
            List<UserDTO.ContactDTO> contacts = new();
            var users = await _userRepository.GetUserByNickQuery(nickQuery);
            contacts = users.Select(u => new UserDTO.ContactDTO(u.Id.ToString(), u.Nick, u.Username, u.Avatar!)).ToList();
            return contacts;
        }
    }
}