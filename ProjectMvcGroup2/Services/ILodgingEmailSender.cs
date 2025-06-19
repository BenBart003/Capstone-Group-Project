namespace ProjectMvcGroup2.Services
{
    public interface ILodgingEmailSender
    {
        void SendLodgingEmail(string controllerAndMethod, string email, string subject, string htmlMessage);
    }
}
