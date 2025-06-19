using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.ViewModels
{
    public class SeeAllPurchasesViewModel
    {
        public string Email { get; set; }
        public List<Rents> Rents { get; set; } = new List<Rents>();
        public List<TicketDate> Tickets { get; set; } = new List<TicketDate>();
        public List<LodgingDates> Lodgings { get; set; } = new List<LodgingDates>();
    }
}
