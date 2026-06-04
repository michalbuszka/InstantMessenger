using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.DTOs.LoginRegister;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Application.Validators;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace InstantMessenger.Application.Services
{
    public sealed class UserService(
        UserRepository userRepository,
        JwtService jwtService,
        IPasswordHasher<User> passwordHasher,
        RegisterValidator registerValidator,
        LoginValidator loginValidator)
    {
        public async Task<LoginRegisterResponseWithRefreshToken> AddUserAsync(RegisterRequest registerRequest)
        {
            List<string> messages = new List<string>();
            LoginRegisterResponse loginRegisterResponse;
            var result = await registerValidator.ValidateAsync(registerRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }

            if (!await userRepository.IsUsernameAvailableAsync(registerRequest.Username))
            {
                messages.Add("Username is already taken");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse,string.Empty);
            }
            
            var user = UserMapper.CreateUser(registerRequest, string.Empty);
            var passwordHash = passwordHasher.HashPassword(user, registerRequest.Password);
            user.PasswordHash = passwordHash;
            await userRepository.AddUserAsync(user);
            var token = jwtService.GenerateToken(user.Username);
            var refreshToken = jwtService.GenerateRefreshToken(user.Username);
            user.RefreshToken = refreshToken;
            await userRepository.SaveUserAsync();
            loginRegisterResponse = new LoginRegisterResponse(0, messages.ToArray(), token);
            return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, refreshToken);
        }

        public async Task<LoginRegisterResponseWithRefreshToken> LoginUserAsync(LoginRequest loginRequest)
        {
            List<string> messages = new List<string>();
            LoginRegisterResponse loginRegisterResponse;
            var result = await loginValidator.ValidateAsync(loginRequest);
            if (result.Errors.Any())
            {
                result.Errors.ToList().ForEach(error => messages.Add(error.ErrorMessage));
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }

            if (await userRepository.IsUsernameAvailableAsync(loginRequest.Username))
            {
                messages.Add("Invalid username or password.");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }

            var user = await userRepository.GetUserByUsernameAsync(loginRequest.Username);
            if (user == null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) !=
                PasswordVerificationResult.Success)
            {
                messages.Add("Invalid username or password.");
                loginRegisterResponse = new(1, messages.ToArray(), string.Empty);
                return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, string.Empty);
            }
            var token = jwtService.GenerateToken(user.Id.ToString());
            var refreshToken = jwtService.GenerateRefreshToken(loginRequest.Username);
            user.RefreshToken = refreshToken;
            await userRepository.SaveUserAsync();
            loginRegisterResponse = new(0, messages.ToArray(), token);
            return new LoginRegisterResponseWithRefreshToken(loginRegisterResponse, refreshToken);;
        }

        public async Task<bool> UpdateUserDataAsync(string? username, UserDto.UserSettingsDto userSettings)
        {
            var user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return false;
            UserMapper.UpdateUser(user, userSettings);
            await userRepository.SaveUserAsync();
            return true;
        }

        public async Task<UserDto.UserSettingsDto?> GetUserDataAsync(string? username)
        {
            var user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return null;
            return new UserDto.UserSettingsDto(user.Email, user.FirstName, user.LastName, user.Nick, user.Avatar);
        }

        public async Task<List<UserDto.ContactDto>?> GetUsersByNickQuery(string? nickQuery)
        {
            List<UserDto.ContactDto> contacts = new();
            var users = await userRepository.GetUserByNickQuery(nickQuery);
            contacts = users.Select(u => new UserDto.ContactDto(u.Id.ToString(), u.Nick, u.Avatar!)).ToList();
            return contacts;
        }

        public async Task<Tokens?> RefreshUserAsync(string refreshToken)
        {
            var user = await userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
                return null;
            var newToken = jwtService.GenerateToken(user.Username);
            var newRefreshToken = jwtService.GenerateRefreshToken(user.Username);
            user.RefreshToken = newRefreshToken;
            await userRepository.SaveUserAsync();
            return new Tokens(newToken, newRefreshToken);
        }
        
        public async Task LogoutUserAsync(string refreshToken)
        {
            var user = await userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
                return;
            user.RefreshToken = string.Empty;
            await userRepository.SaveUserAsync();
        }

        public async Task<UserDto.ContactDto> GetUserById(string Id)
        {
            var user = await userRepository.GetUserByIdAsync(Guid.Parse(Id));
            if (user is null)
                return null;
            return new UserDto.ContactDto(user.Id.ToString(), user.Nick, user.Avatar!);
        }
    }
}