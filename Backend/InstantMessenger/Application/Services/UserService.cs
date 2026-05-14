using InstantMessenger.Application.DTOs;
using InstantMessenger.Application.Mappers;
using InstantMessenger.Domain.Entities;
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
        public async Task AddUserAsync(string username, string password)
        {
            RegisterRequestDTO registerRequestDto = new RegisterRequestDTO(username, password);
            await _userRepository.AddUserAsync(UserMapper.createUser(registerRequestDto));
        }
        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
