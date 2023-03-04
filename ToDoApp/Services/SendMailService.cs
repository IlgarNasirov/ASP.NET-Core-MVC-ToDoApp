using System.Net.Mail;
using System.Net;
using ToDoApp.Data.Models;

namespace ToDoApp.Services
{
    public class SendMailService:ISendMailService
    {
        private readonly IConfiguration _configuration;

        public SendMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMail(Mail mail)
        {
            using (MailMessage mm = new MailMessage(_configuration.GetValue<string>("Mail:Username")!, mail.From))
            {
                mm.Subject = mail.Subject;
                mm.Body = mail.Body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                NetworkCredential NetworkCred = new NetworkCredential(_configuration.GetValue<string>("Mail:Username")!, _configuration.GetValue<string>("Mail:Password")!);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}
