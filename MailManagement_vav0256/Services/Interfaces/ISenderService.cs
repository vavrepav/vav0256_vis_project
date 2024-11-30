using MailManagement_vav0256.DTOs.Sender;

namespace MailManagement_vav0256.Services.Interfaces
{
    public interface ISenderService
    {
        IEnumerable<SenderReadDto> GetAllSenders();
        SenderReadDto GetSenderById(Guid id);
        SenderReadDto CreateSender(SenderCreateDto senderDto);
        bool UpdateSender(Guid id, SenderUpdateDto senderDto);
        bool DeleteSender(Guid id);
    }
}