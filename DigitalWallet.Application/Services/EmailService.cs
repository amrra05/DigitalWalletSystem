using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Application.Settings;
using DigitalWallet.Domain.Entities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;


namespace DigitalWallet.Infrastructure.Services;


public class EmailService : IEmailService
{

    private readonly EmailSettings _settings;


    public EmailService(
        IOptions<EmailSettings> settings)
    {

        _settings = settings.Value;

    }




    public async Task SendEmailAsync(
    EmailRequest request)
    {


        var email = new MimeMessage();


        email.From.Add(
            new MailboxAddress(
                "Digital Wallet",
                _settings.Email
            )
        );


        email.To.Add(
            MailboxAddress.Parse(request.To)
        );


        email.Subject = request.Subject;


        email.Body =
            new TextPart("html")
            {
                Text = request.Body
            };



        using var smtp = new MailKit.Net.Smtp.SmtpClient();


        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            MailKit.Security.SecureSocketOptions.StartTls
        );


        await smtp.AuthenticateAsync(
            _settings.Email,
            _settings.Password
        );


        await smtp.SendAsync(email);



        await smtp.DisconnectAsync(true);

    }


    public async Task SendVerificationEmailAsync(
    string email,
    string verificationLink)
    {
        EmailRequest request = new EmailRequest
        {
            To = email,
            Subject = "EmailVerification",
            Body = $@"
<h2>Verify Your Email</h2>

<p>
Thank you for registering with Digital Wallet.
</p>

<p>
Please click the button below to verify your email address.
</p>

<p>
<a href='{verificationLink}'
style='
background:#2563eb;
color:white;
padding:12px 20px;
text-decoration:none;
border-radius:8px;'>
Verify Email
</a>
</p>

<p>
This link expires in 24 hours.
</p>"
        };
        await SendEmailAsync(request);
    }

}