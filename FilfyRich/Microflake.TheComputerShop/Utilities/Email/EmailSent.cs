using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Utilities.Email
{
  public  class EmailSent
    {
        public async static Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {

                var _email = "Ecommerce@elecqueue.com";
                var _pass = "Tms.123456";
                var _dispname = "E Shop";

                MailMessage myMessage = new MailMessage();
                myMessage.IsBodyHtml = true;
                myMessage.To.Add(email);
                myMessage.From = new MailAddress(_email, _dispname);
                myMessage.Subject = subject;
                myMessage.Body = message;


                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "207.180.192.81";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Timeout = 100000;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(_email, _pass);

                    // smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(myMessage);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
