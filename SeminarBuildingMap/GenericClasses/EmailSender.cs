using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeminarBuildingMap.GenericClasses
{
    //This class holds the functionality to send an email
    //It is used to send a confirmation email, to allow a registered user to set their password
    public class EmailSender: IEmailSender
    {

        private string host; //holds the email host
        private int port; //port
        private bool enableSSL; //enable for security
        private string userName; //the gmail user
        private string password; //gmail password

        //constructor, simply initializes values
        public EmailSender(string host, int port, bool enableSSL, string userName, string password) {
            this.host = host; 
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.password = password;
        }

        //actually sends email. this is an async method but it runs synchronously. For some reason sendAsync is broken
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var newMessage = new MailMessage())
            {
                //set the email parameters
                newMessage.To.Add(new MailAddress(email));
                newMessage.From = new MailAddress(userName);
                newMessage.Subject = subject;
                newMessage.Body = message;
                newMessage.IsBodyHtml = true;
                using (var client = new SmtpClient(host)) //send the message
                {
                    client.Port = port;
                    client.Credentials = new NetworkCredential(userName, password);
                    client.EnableSsl = enableSSL;
                    //await Task.Run(() => client.SendAsync(newMessage, "successfully sent email")); //disabled since this function wouldn't work.
                    client.Send(newMessage);
                }
            }
        }
    }
}
