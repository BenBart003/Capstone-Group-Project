namespace ProjectMvcGroup2.Services
{
        public interface ILiftTicketEmailSender
        {
            void SendLiftTicketEmail(string controllerAndMethod, string email, string subject, string htmlMessage);
        }
    }