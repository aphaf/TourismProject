using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace MvcTourismApp.Services
{
    //email sender code is included but not implemented
    /*public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            try
            {
                //From Address    
                string FromAddress = "exampleEmail@test.com";
                string FromAddressTitle = "Tourist Planner Administrator";
                //To Address    
                string ToAddress = email;
                string ToAddressTitle = "Tourist Planner User";
                string Subject = subject;
                string BodyContent = htmlMessage;

                //Smtp Server    
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number    
                int SmtpPortNumber = 587;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress
                                        (FromAddressTitle,
                                         FromAddress
                                         ));
                mimeMessage.To.Add(new MailboxAddress
                                         (ToAddressTitle,
                                         ToAddress
                                         ));
                mimeMessage.Subject = Subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    client.Authenticate(
                        "exampleEmail@test.com",
                        "exampleEmailAppPassword"
                        );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                string exceptionCaught = ex.Message;
            }

        }//end of the SendMailAsync method
    }//end of EmailSender class
    */
}
