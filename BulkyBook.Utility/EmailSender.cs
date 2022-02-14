using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("contact@bookstore.com")); // adresa de la from  asa va arata
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text=htmlMessage};// aici in htmlMessage poti defini un template html

            // send email
            using (var emailClient = new SmtpClient())
			{
                emailClient.Connect("smtp.gmail.com",587,MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("scortarudaniel1@gmail.com","Admin.1234");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
			}

            return Task.CompletedTask;
        }
    }
}
