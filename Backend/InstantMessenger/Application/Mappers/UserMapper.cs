using InstantMessenger.Application.DTOs;
using InstantMessenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessenger.Application.Mappers
{
    public static class UserMapper
    {
        public static User createUser (AddUserDTO addUserDTO)
        {
            User user = new User();
            user.Nick = addUserDTO.Nick;
            return user;
        }
    }
}
