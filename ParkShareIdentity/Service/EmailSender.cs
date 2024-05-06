using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ParkShareIdentity.Service
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            #region mail code subject and message

            var expiretime = DateTime.Now.AddMinutes(30);
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("abpnarola@gmail.com"); //dynamic 
            message.Subject = subject;

            message.Body = htmlMessage; 
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "abpnarola@gmail.com",
                    Password = "uljshtzeaxczdcsw"
                   // Password = " ieoe ixva fryy tagt"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            #endregion 
        }
    }
}
