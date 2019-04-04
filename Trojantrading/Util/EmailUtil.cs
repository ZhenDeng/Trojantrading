using Trojantrading.Repositories;
using System;
using System.Net.Mail;
using System.Net;

namespace Trojantrading.Util
{
    public class EmailUtil
    {
        private readonly IEmailRepository _emailRepository;
        public EmailUtil(EmailRepository emailRepository){
            this._emailRepository = emailRepository;
        }

        public bool sendEmail(string toEmail, string subject, string body){
            string account = _emailRepository.GetAccount();
            string passWord = _emailRepository.GetPassWord();
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            try{
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("");
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                client.Send(mailMessage);
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }
    }
}