using InstantMessenger.Domain.Entities;
using RegisterRequest = InstantMessenger.Application.DTOs.LoginRegister.RegisterRequest;

namespace InstantMessenger.Application.Mappers
{
    public static class UserMapper
    {
        public static User createUser (RegisterRequest registerRequest)
        {
            User user = new User(registerRequest.Username, registerRequest.Password);
            return user;
        }
    }
}
