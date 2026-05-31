using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Polly.Retry;
using SmartBus.Application.IExternalServices;
using SmartBus.Domain.Models;

namespace SmartBus.Infrasturcture.ExternalServices.MailExternalServices
{
    public class SendingEmailService : ISendingEmailService
    {
        private readonly MailkitSetting _setting;

        // Retry Policy
        private readonly AsyncRetryPolicy _retryPolicy;

        public SendingEmailService(IOptions<MailkitSetting> options)
        {
            _setting = options.Value;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(retryAttempt * 2),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine(
                            $"Retry {retryCount} after {timeSpan.TotalSeconds}s " +
                            $"because of: {exception.Message}");
                    });
        }

        public async Task<bool> SendingEmail(string to, string subject, string body)
        {
            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var email = new MimeMessage();

                    email.From.Add(
                        new MailboxAddress(_setting.UserName, _setting.From));

                    email.To.Add(MailboxAddress.Parse(to));

                    email.Subject = subject;

                    email.Body = new TextPart("html")
                    {
                        Text = body
                    };

                    using var smtp = new SmtpClient();

                    await smtp.ConnectAsync(
                        _setting.Host,
                        _setting.Port,
                        SecureSocketOptions.StartTls);

                    await smtp.AuthenticateAsync(
                        _setting.From,
                        _setting.apiKey);

                    await smtp.SendAsync(email);

                    await smtp.DisconnectAsync(true);
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SendingEmailWithAttachment(string to, string subject, string body, byte[] attachment, string attachmentName)
        {
            try
            {
                Console.WriteLine($"Sending Email ticket for booking ID");
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var email = new MimeMessage();

                    email.From.Add(
                        new MailboxAddress(_setting.UserName, _setting.From));

                    email.To.Add(MailboxAddress.Parse(to));

                    email.Subject = subject;

                    var builder = new BodyBuilder
                    {
                        HtmlBody = body
                    };

                    if (attachment != null)
                    {
                        builder.Attachments.Add(attachmentName, attachment);
                    }

                    email.Body = builder.ToMessageBody();

                    using var smtp = new SmtpClient();

                    await smtp.ConnectAsync(
                        _setting.Host,
                        _setting.Port,
                        SecureSocketOptions.StartTls);

                    await smtp.AuthenticateAsync(
                        _setting.From,
                        _setting.apiKey);

                    await smtp.SendAsync(email);

                    await smtp.DisconnectAsync(true);
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}