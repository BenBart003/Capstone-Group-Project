using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProjectMvcGroup2.Services
{
    public class EquipmentRentalEmailSender : IEquipmentRentalEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;

        public EquipmentRentalEmailSender(IEmailSender emailSender, IHttpContextAccessor contextAccessor)
        {
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
        }

        public void SendEquipmentRentalEmail(string controllerAndMethod, string email, string subject, string inputMessage)
        {
            string callbackUrl = "http://" + _contextAccessor.HttpContext.Request.Host + "/" + controllerAndMethod;
            string htmlMessage = inputMessage + $" View by clicking <a href='{callbackUrl}'>here</a>.";

            _emailSender.SendEmailAsync(email, subject, htmlMessage).Wait();
        }
    }
}