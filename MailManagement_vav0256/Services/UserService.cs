using AutoMapper;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.User;
using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public UserReadDto Login(string email, string password)
        {
            var user = _userRepository.GetByEmailAndPassword(email, password);
            return _mapper.Map<UserReadDto>(user);
        }

        public UserReadDto GetUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            return _mapper.Map<UserReadDto>(user);
        }

        public IEnumerable<UserReadDto> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public UserReadDto GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);
            return _mapper.Map<UserReadDto>(user);
        }

        public UserReadDto CreateUser(UserCreateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();

            _userRepository.Create(user);

            return _mapper.Map<UserReadDto>(user);
        }

        public bool UpdateUser(Guid id, UserUpdateDto userDto)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return false;

            _mapper.Map(userDto, user);

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