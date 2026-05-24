using InstantMessenger.Application.DTOs;
using InstantMessenger.Domain.Entities;
using RegisterRequest = InstantMessenger.Application.DTOs.LoginRegister.RegisterRequest;

namespace InstantMessenger.Application.Mappers
{
    public static class UserMapper
    {
        public static User CreateUser(RegisterRequest registerRequest, string passwordHash)
        {
            var user = new User()
            {
                Username = registerRequest.Username,
                PasswordHash = passwordHash
            };
            return user;
        }

        public static void UpdateUser(User user, UserDTO.UserSettingsDTO userSettings)
        {
            if (userSettings.Email is not null)
                user.Email = userSettings.Email;
            if (userSettings.FirstName is not null)
                user.FirstName = userSettings.FirstName;
            if (userSettings.LastName is not null)
                user.LastName = userSettings.LastName;
            if (userSettings.Nick is not null)
                user.Nick = userSettings.Nick;
            if (userSettings.Avatar is not null)
                user.Avatar = userSettings.Avatar;
        }
    }
}