using MailManagement_vav0256.Entities;
using System;
using System.Collections.Generic;

namespace MailManagement_vav0256.Repositories.Interfaces
{
    public interface IMailRepository
    {
        IEnumerable<Mail> GetAll();
        Mail GetById(Guid id);
        Mail Create(Mail mail);
        void Update(Mail mail);
        void Delete(Guid id);
    }
}