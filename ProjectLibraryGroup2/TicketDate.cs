using System.ComponentModel.DataAnnotations;

namespace ProjectLibraryGroup2
{
    public class TicketDate
    {
        [Key]
        public int TicketDateID { get; set; }
        //called guest class
        public Guest Guest { get; set; }
        //called LiftTicket class rather than LiftTicketID
        public LiftTicket LiftTicket { get; set; }
        [Required]
        public DateOnly PassStartDate { get; set; }
        [Required]
        public DateOnly PassEndDate { get; set; }
        public TicketDate() { }
        public TicketDate(Guest guest, LiftTicket liftTicket, DateOnly passStartDate, DateOnly passEndDate)
        {
            //added this
            this.Guest = guest;
            this.LiftTicket = liftTicket;
            this.PassStartDate = passStartDate;
            this.PassEndDate = passEndDate;
        }
    }
}
