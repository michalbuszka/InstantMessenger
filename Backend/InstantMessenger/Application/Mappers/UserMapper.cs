using InstantMessenger.Domain.Entities;
using RegisterRequest = InstantMessenger.Application.DTOs.LoginRegister.RegisterRequest;

namespace InstantMessenger.Application.Mappers
{
    public static class UserMapper
    {
        public static User createUser (RegisterRequest registerRequest, string passwordHash)
        {
            var user = new User(registerRequest.Username, passwordHash);
            return user;
        }
    }
}
