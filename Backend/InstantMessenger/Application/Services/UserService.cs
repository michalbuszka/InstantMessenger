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

        public async Task<LoginRegisterResponseWithRefreshToken> AddUserAsync(RegisterRequest registerRequest)
        {
            List<string> messages = new List<string>();
            LoginRegisterResponse loginRegisterResponse;
            var result = await _registerValidator.ValidateAsync(registerRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }

            if (!await _userRepository.IsUsernameAvailableAsync(registerRequest.Username))
            {
                messages.Add("Username is already taken");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse,string.Empty);
            }
            
            var user = UserMapper.CreateUser(registerRequest, string.Empty);
            var passwordHash = _passwordHasher.HashPassword(user, registerRequest.Password);
            user.PasswordHash = passwordHash;
            await _userRepository.AddUserAsync(user);
            var token = _jwtService.GenerateToken(user.Username);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Username);
            user.RefreshToken = refreshToken;
            await _userRepository.SaveUserAsync();
            loginRegisterResponse = new LoginRegisterResponse(0, messages.ToArray(), token);
            return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, refreshToken);
        }

        public async Task<LoginRegisterResponseWithRefreshToken> LoginUserAsync(LoginRequest loginRequest)
        {
            List<string> messages = new List<string>();
            LoginRegisterResponse loginRegisterResponse;
            var result = await _loginValidator.ValidateAsync(loginRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }

            if (await _userRepository.IsUsernameAvailableAsync(loginRequest.Username))
            {
                messages.Add("Invalid username or password.");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }

            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) !=
                PasswordVerificationResult.Success)
            {
                messages.Add("Invalid username or password.");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }
            var token = _jwtService.GenerateToken(user.Id.ToString());
            var refreshToken = _jwtService.GenerateRefreshToken(loginRequest.Username);
            user.RefreshToken = refreshToken;
            await _userRepository.SaveUserAsync();
            loginRegisterResponse = new(0, messages.ToArray(), token);
            return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, refreshToken);;
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
            List<UserDTO.ContactDTO> contacts = new();
            var users = await _userRepository.GetUserByNickQuery(nickQuery);
            contacts = users.Select(u => new UserDTO.ContactDTO(u.Id.ToString(), u.Nick, u.Avatar!)).ToList();
            return contacts;
        }

        public async Task<Tokens?> RefreshUserAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
                return null;
            var newToken = _jwtService.GenerateToken(user.Username);
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Username);
            user.RefreshToken = newRefreshToken;
            await _userRepository.SaveUserAsync();
            return new Tokens(newToken, newRefreshToken);
        }
        
        public async Task LogoutUserAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
                return;
            user.RefreshToken = string.Empty;
            await _userRepository.SaveUserAsync();
        }

        public async Task<UserDTO.ContactDTO> GetUserById(string Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(Id));
            if (user is null)
                return null;
            return new UserDTO.ContactDTO(user.Id.ToString(), user.Nick, user.Avatar!);
        }
    }
}