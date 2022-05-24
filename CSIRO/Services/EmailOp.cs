using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace CSIRO.Services
{
    public class EmailOp
    {
        public EmailOp(string to, string from, string title, string message, string strKey)
        {
            try
            {
                var aipKey = strKey;
                var client = new SendGridClient(aipKey);
                var from_1 = new EmailAddress(from, "CSIRO");
                var to_1 = new EmailAddress(to, "To email");
                var plainTextContent = message;
                var htmlContent = "<strong> " + message + "</strong>";
                var msg = MailHelper.CreateSingleEmail(from_1, to_1, title, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
