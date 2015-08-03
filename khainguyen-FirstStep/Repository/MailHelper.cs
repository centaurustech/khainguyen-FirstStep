using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;

namespace MvcLibrary.Repository
{
    public class MailHelper
    {
        /// <summary>
        /// Sends an mail message
        /// </summary>
        /// <param name="from">Sender address</param>
        /// <param name="to">Recepient address</param>
        /// <param name="bcc">Bcc recepient</param>
        /// <param name="cc">Cc recepient</param>
        /// <param name="subject">Subject of mail message</param>
        /// <param name="body">Body of mail message</param>
        public static void SendMailMessage(string fromEmail, string toEmail, string bcc, string cc, string subject, string body, string smtp, bool ssl, string username, string password)
        {
            MailMessage message = new MailMessage();

            SmtpClient smtpClient = new SmtpClient();

            string msg = string.Empty;

            MailAddress fromAddress = new MailAddress(fromEmail);
            message.From = fromAddress;
            message.To.Add(toEmail);
            if (bcc != null)
                message.Bcc.Add(bcc);
            if (cc != null)
                message.CC.Add(cc);

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            //smtpClient.Host = "smtp.mail.yahoo.com"; // We use yahoo as our smtp client
            smtpClient.Host = smtp; // We use yahoo as our smtp client
            smtpClient.Port = 587;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(username, password);

            smtpClient.Send(message);
        }        
    }
}