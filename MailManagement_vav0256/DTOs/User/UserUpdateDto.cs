using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.DTOs.User
{
    public class UserUpdateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}