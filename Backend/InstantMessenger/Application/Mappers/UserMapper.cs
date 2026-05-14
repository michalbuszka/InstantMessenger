using InstantMessenger.Application.DTOs;
using InstantMessenger.Domain.Entities;

namespace InstantMessenger.Application.Mappers
{
    public static class UserMapper
    {
        public static User createUser (RegisterRequestDTO registerRequestDto)
        {
            User user = new User(registerRequestDto.Username, registerRequestDto.Password);
            return user;
        }
    }
}
