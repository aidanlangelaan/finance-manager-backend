using System.Net;
using System.Net.Mail;
using FinanceManager.Business.Exceptions;
using FinanceManager.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Business.Services;

public class SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    : ISmtpEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient
            {
                Host = configuration["EmailSettings:SmtpHost"] ?? throw new ConfigurationException("SmtpHost cannot be null"),
                Port = int.Parse(configuration["EmailSettings:SmtpPort"] ?? throw new ConfigurationException("SmtpPort cannot be null")),
                EnableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"] ?? throw new ConfigurationException("EnableSsl cannot be null")),
                Credentials = new NetworkCredential(
                    configuration["EmailSettings:SmtpUsername"] ?? throw new ConfigurationException("SmtpUsername cannot be null"),
                    configuration["EmailSettings:SmtpPassword"] ?? throw new ConfigurationException("SmtpPassword cannot be null")
                )
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(configuration["EmailSettings:FromEmail"] ?? throw new ConfigurationException("FromEmail cannot be null")),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
            logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw; // Re-throw the exception to be handled by global error handling middleware if needed
        }
    }
}