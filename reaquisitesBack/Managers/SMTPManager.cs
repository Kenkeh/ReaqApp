using System.Net;
using System.Net.Mail;
namespace reaquisites.Managers
{
    public static class SMTPManager
    {
        static SmtpClient smtpClient;

        /*
        We are using GMail SMTP, but we should use own SMTP and configure it to do a fallback in case email doesnt exist
        */

        static SMTPManager()
        {
            smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("reaquisites@gmail.com", "nqkbbyijmeseeonc");
            smtpClient.EnableSsl = true;
        }

        internal static void sendMailMessage(MailMessage mail){
            smtpClient.Send(mail);
        }
    }
}