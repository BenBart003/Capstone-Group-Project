namespace ProjectMvcGroup2.Services
{
    public interface IEquipmentRentalEmailSender
    {
        void SendEquipmentRentalEmail(string controllerAndMethod, string email, string subject, string htmlMessage);
    }
}