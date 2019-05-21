using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Trojantrading.Service
{
    public class Share : IShare
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public Share(ILogger<Share> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool SendEmail(string from, string to, string subject, string message, string cc = null, string bcc = null, bool isHTML = false, List<Attachment> attachments = null, string fromAddressDisplayName = "", string sendingApplication = "Email Service Application")
        {
            bool isSent = false;

            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("testprojectemail2019@gmail.com", "123456google"),
                    EnableSsl = true
                };
                //MailMessage emailMessage = new MailMessage(from, to);

                MailMessage emailMessage = new MailMessage();

                emailMessage.From = new MailAddress(from, fromAddressDisplayName);

                // to address, it can be more than one
                if (to != null)
                {
                    List<string> toList = to.Trim().Split(';', ',').ToList();

                    foreach (string toAddress in toList)
                    {
                        if (!string.IsNullOrWhiteSpace(toAddress))
                            emailMessage.To.Add(new MailAddress(toAddress));
                    }
                }

                if (emailMessage.To.Count <= 0)
                {
                    // there is no to email address specified
                    return isSent;
                }

                emailMessage.Subject = subject;
                emailMessage.Body = message;
                emailMessage.IsBodyHtml = isHTML;

                // adding attachments
                foreach (Attachment attachment in attachments ?? new List<Attachment>())
                {
                    emailMessage.Attachments.Add(attachment);
                }

                // add cc list

                if (cc != null)
                {
                    List<string> ccList = cc.Trim().Split(';', ',').ToList();

                    foreach (string ccAddress in ccList)
                    {
                        if (!string.IsNullOrWhiteSpace(ccAddress))
                            emailMessage.CC.Add(ccAddress.Trim());
                    }
                }

                // add bcc list

                if (bcc != null)
                {
                    List<string> bccList = bcc.Trim().Split(';', ',').ToList();

                    foreach (string bccAddress in bccList)
                    {
                        if (!string.IsNullOrWhiteSpace(bccAddress))
                            emailMessage.Bcc.Add(bccAddress.Trim());
                    }
                }


                client.Send(emailMessage);
                isSent = true;
            }
            
            catch (Exception ex)
            {
                string log = "Error in Sending Email";
                string sEvent = string.Format("Unable to send email. Subject : {0}, To Address : {1}. Details of Error: {2}\n\n Email Message: {3}", subject, to, ex.ToString(), message);
                _logger.LogError(sEvent);
            }

            return isSent;
        }

        public string GetConfigKey(string keyName, string categoryName = "AppConfig")
        {
            string keyValue = null;
            try
            {
                keyValue = _configuration.GetSection(categoryName)[keyName].ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return keyValue;
        }

        public void EncodePassWord(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
