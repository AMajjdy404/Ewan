using Ewan.Core.Models.Dtos.Mail;

namespace Ewan.Core.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Email email, CancellationToken cancellationToken = default);
    }
}
