using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.Models
{
    public interface ILiftTicketRepo
    {
        List<LiftTicket> GetAllLiftTickets();
        LiftTicket GetLiftTicketById(int id);
        void AddTicketPurchase(TicketDate ticket);
        List<TicketDate> GetAllTicketDates();
    }

}
