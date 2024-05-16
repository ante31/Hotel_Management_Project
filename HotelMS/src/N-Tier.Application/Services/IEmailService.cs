using HotelMS.Application.Common.Email;

namespace HotelMS.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
