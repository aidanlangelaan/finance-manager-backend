namespace FinanceManager.Business.Interfaces;

public interface ISmtpEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}