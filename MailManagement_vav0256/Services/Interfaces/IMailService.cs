using MailManagement_vav0256.DTOs.Mail;

namespace MailManagement_vav0256.Services.Interfaces
{
    public interface IMailService
    {
        IEnumerable<MailReadDto> GetAllMails();
        MailReadDto GetMailById(Guid id);
        MailReadDto CreateMail(MailCreateDto mailDto, Guid receptionistId);
        bool ClaimMail(MailClaimDto claimDto, Guid mailId);
        bool UpdateMail(Guid id, MailUpdateDto mailDto);
        bool DeleteMail(Guid id);
        IEnumerable<MailReadDto> GetMailsByRecipientId(Guid recipientId);
    }
}