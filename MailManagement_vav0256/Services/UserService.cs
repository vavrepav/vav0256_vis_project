using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.User;
using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserReadDto Login(string email, string password)
        {
            var user = _userRepository.GetByEmailAndPassword(email, password);
            if (user == null)
                return null;

            return new UserReadDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };
        }
        
        public UserReadDto GetUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
                return null;

            return new UserReadDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };
        }

        public IEnumerable<UserReadDto> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return users.Select(user => new UserReadDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            });
        }

        public UserReadDto GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return null;

            return new UserReadDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };
        }

        public UserReadDto CreateUser(UserCreateDto userDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role
            };

            var createdUser = _userRepository.Create(user);

            return new UserReadDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                Role = createdUser.Role
            };
        }

        public bool UpdateUser(Guid id, UserUpdateDto userDto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return false;

            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Role = userDto.Role;

            _userRepository.Update(user);

            return true;
        }

        public bool DeleteUser(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return false;

            _userRepository.Delete(id);

            return true;
        }
    }
}