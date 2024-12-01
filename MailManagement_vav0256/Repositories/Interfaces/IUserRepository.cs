using MailManagement_vav0256.Entities;
using System;
using System.Collections.Generic;

namespace MailManagement_vav0256.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetByEmailAndPassword(string email, string password);
        User GetByEmail(string email);
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        User Create(User user);
        void Update(User user);
        void Delete(Guid id);
    }
}