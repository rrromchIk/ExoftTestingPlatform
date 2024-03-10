using Security.Models;

namespace Security.Service.Abstractions;

public interface IEmailService
{
    Task<bool> SendEmail(string receiverEmail, string userId, string token);
}