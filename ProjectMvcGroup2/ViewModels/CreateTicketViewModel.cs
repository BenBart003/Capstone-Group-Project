using ProjectLibraryGroup2;
using System.ComponentModel.DataAnnotations;

namespace ProjectMvcGroup2.ViewModels
{
    public class CreateTicketViewModel
    {
        [Required]
        public TicketType SelectedTicketType { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? PassStartDate { get; set; }

        public string? GuestId { get; set; }
    }
}
