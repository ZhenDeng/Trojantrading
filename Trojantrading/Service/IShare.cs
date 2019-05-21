using System.Collections.Generic;
using System.Net.Mail;

namespace Trojantrading.Service
{
    public interface IShare
    {
        bool SendEmail(string from, string to, string subject, string message, string cc = null, string bcc = null, bool isHTML = false, List<Attachment> attachments = null, string fromAddressDisplayName = "", string sendingApplication = "Email Service Application");
        string GetConfigKey(string keyName, string categoryName = "AppConfig");
        void EncodePassWord(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string RandomString(int size, bool lowerCase);
        int RandomNumber(int min, int max);
    }
}