using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Trojantrading.Service
{
    public class Share : IShare
    {
        private readonly ILogger _logger;

        public Share(ILogger logger)
        {
            _logger = logger;
        }

        public bool SendEmail(string from, string to, string subject, string message, string cc = null, string bcc = null, bool isHTML = false, List<Attachment> attachments = null, string fromAddressDisplayName = "", string sendingApplication = "Email Service Application")
        {
            bool isSent = false;

            try
            {
                SmtpClient client = new SmtpClient("bulk-email.dickerdata.int.au");
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
    }
}
