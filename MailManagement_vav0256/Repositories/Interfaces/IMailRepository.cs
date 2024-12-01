using MailManagement_vav0256.Entities;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories.Interfaces
{
    public interface IMailRepository
    {
        IEnumerable<Mail> GetAll();
        IEnumerable<Mail> GetByRecipientId(Guid recipientId);
        Mail GetById(Guid id);
        public Mail Create(Mail mail, SqlConnection connection = null, SqlTransaction transaction = null);
        public void Update(Mail mail, SqlConnection connection = null, SqlTransaction transaction = null);
        void Delete(Guid id);
        IEnumerable<Mail> GetByStatus(string status);
        IEnumerable<Mail> GetByRecipientIdAndStatus(Guid recipientId, string status);
    }
}