using Microsoft.EntityFrameworkCore;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Data;

namespace ProjectMvcGroup2.Models
{
    public class LiftTicketRepo : ILiftTicketRepo
    {
        private readonly ApplicationDbContext _database;

        public LiftTicketRepo(ApplicationDbContext database)
        {
            _database = database;
        }

        public List<LiftTicket> GetAllLiftTickets()
        {
            return _database.LiftTicket.ToList();
        }

        public LiftTicket GetLiftTicketById(int id)
        {
            return _database.LiftTicket.Find(id);
        }

        public void AddTicketPurchase(TicketDate ticket)
        {
            _database.TicketDate.Add(ticket);
            _database.SaveChanges();
        }

        public List<TicketDate> GetAllTicketDates()
        {
            return _database.TicketDate
                .Include(t => t.Guest)
                .Include(t => t.LiftTicket)
                .ToList();
        }

    }
}
