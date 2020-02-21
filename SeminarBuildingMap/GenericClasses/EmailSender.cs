using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeminarBuildingMap.GenericClasses
{
    public class EmailSender: IEmailSender
    {

        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;

        public EmailSender(string host, int port, bool enableSSL, string userName, string password) {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.password = password;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var newMessage = new MailMessage())
            {
                newMessage.To.Add(new MailAddress(email));
                newMessage.From = new MailAddress(userName);
                newMessage.Subject = subject;
                newMessage.Body = message;
                newMessage.IsBodyHtml = true;
                using (var client = new SmtpClient(host))
                {
                    client.Port = port;
                    client.Credentials = new NetworkCredential(userName, password);
                    client.EnableSsl = enableSSL;
                    //await Task.Run(() => client.SendAsync(newMessage, "successfully sent email"));
                    client.Send(newMessage);
                }
            }
        }
    }
}
