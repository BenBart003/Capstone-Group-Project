﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProjectMvcGroup2.Services
{
    public class LiftTicketEmailSender : ILiftTicketEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;

        public LiftTicketEmailSender(IEmailSender emailSender, IHttpContextAccessor contextAccessor)
        {
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
        }

        public void SendLiftTicketEmail(string controllerAndMethod, string email, string subject, string inputMessage)
        {
            string callbackUrl = "http://" + _contextAccessor.HttpContext.Request.Host + "/" + controllerAndMethod;
            string htmlMessage = inputMessage + $" View by clicking <a href='{callbackUrl}'>here</a>.";

            _emailSender.SendEmailAsync(email, subject, htmlMessage).Wait();
        }
    }
}
