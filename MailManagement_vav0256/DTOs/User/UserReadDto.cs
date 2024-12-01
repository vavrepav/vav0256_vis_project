using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.DTOs.User
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}