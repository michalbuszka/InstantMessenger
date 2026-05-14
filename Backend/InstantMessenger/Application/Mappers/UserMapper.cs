using InstantMessenger.Application.DTOs;
using InstantMessenger.Domain.Entities;

namespace InstantMessenger.Application.Mappers
{
    public static class UserMapper
    {
        public static User createUser (AddUserDTO addUserDTO)
        {
            User user = new User { Username = addUserDTO.Username };
            return user;
        }
    }
}
