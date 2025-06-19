using Microsoft.AspNetCore.Identity.UI.Services;
using ProjectMvcGroup2.Models;
using Microsoft.AspNetCore.Identity;

namespace ProjectMvcGroup2.Services
{
    public class LodgingEmailSender : ILodgingEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;

        public LodgingEmailSender(IEmailSender emailSender, IHttpContextAccessor contextAccessor)
        {
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
        }

        public void SendLodgingEmail(string controllerAndMethod, string email, string subject, string inputMessage)
        {
            string callbackUrl = "http://" + _contextAccessor.HttpContext.Request.Host + "/" + controllerAndMethod;
            string htmlMessage = inputMessage + $" View by clicking <a href='{callbackUrl}'>here</a>.";

            _emailSender.SendEmailAsync(email, subject, htmlMessage).Wait();
        }
    }
}
