using MailManagement_vav0256.DTOs.User;

namespace MailManagement_vav0256.Services.Interfaces
{
    public interface IUserService
    {
        UserReadDto Login(string email, string password);
        UserReadDto GetUserByEmail(string email);
        IEnumerable<UserReadDto> GetAllUsers();
        UserReadDto GetUserById(Guid id);
        UserReadDto CreateUser(UserCreateDto userDto);
        bool UpdateUser(Guid id, UserUpdateDto userDto);
        bool DeleteUser(Guid id);
    }
}