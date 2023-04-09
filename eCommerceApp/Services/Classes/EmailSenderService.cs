using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Services.Classes
{
    public static class EmailSenderService
    {
       public static void SendEmail(string email,string check)
        {
            string fromMail = "medina.abasova03@gmail.com";
            string fromPassword = "xliqwbhumlzcbxmp";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail, "BookShop");
            message.Subject = "Check from BookShop";
            message.To.Add(new MailAddress(email));
            message.Body = check;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
