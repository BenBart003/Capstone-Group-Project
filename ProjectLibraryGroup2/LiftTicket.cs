using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryGroup2
{
    public class LiftTicket
    {
        public int LiftTicketID { get; set; }
        public TicketType TicketType { get; set; }
        public double Cost { get; set; }

        public LiftTicket() { }

        public LiftTicket(TicketType ticketType, double cost)
        {
            this.TicketType = ticketType;
            this.Cost = cost;
        }
    }

    public enum TicketType
    {
        SeasonPass,
        WeekendPass,
        DayPass
    }
}

