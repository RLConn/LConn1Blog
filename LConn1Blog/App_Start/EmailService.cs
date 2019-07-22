using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;

namespace LConnBlog
{
    public class PersonalEmail
    {
        public async Task SendAsync(MailMessage message)
        {
            var GmailUsername = WebConfigurationManager.AppSettings["username"];
            var GmailPassword = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);

            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            })
            {
                try
                {
                    await smtp.SendMailAsync(message);
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await Task.FromResult(0);
                }
            };
        }

    }

    internal class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}