﻿using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;


namespace ProjectMvcGroup2.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                //From Address    
                string FromAddress = "Test.BrBart@gmail.com";
                string FromAddressTitle = "Your new reservation has been submitted";
                //To Address    
                string ToAddress = email;
                string ToAddressTitle = "User";
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
                        "Test.BrBart@gmail.com",
                        " fjqk edtz rlup jhyc "
                        );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                string emailException = ex.Message;
            }

        }//end SendEmailAsync method

    }//end EmailSender class

}//end namespace