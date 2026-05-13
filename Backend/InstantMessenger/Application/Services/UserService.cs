using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Domain.Entities;
using InstantMessenger.Infrastructure;
using InstantMessenger.Infrastructure.Repositories;

namespace InstantMessenger.Application.Services
{
    public class UserService
    {
        public readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUserAsync(string nick)
        {
            AddUserDTO addUserDTO = new AddUserDTO(nick);
            await _userRepository.AddUserAsync(UserMapper.createUser(addUserDTO));
        }
        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
