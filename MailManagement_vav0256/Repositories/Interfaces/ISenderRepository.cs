using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Repositories.Interfaces
{
    public interface ISenderRepository
    {
        IEnumerable<Sender> GetAll();
        Sender GetById(Guid id);
        Sender Create(Sender sender);
        void Update(Sender sender);
        void Delete(Guid id);
    }
}