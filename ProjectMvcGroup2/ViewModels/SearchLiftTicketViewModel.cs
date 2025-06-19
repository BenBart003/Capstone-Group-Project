using ProjectLibraryGroup2;
using System.ComponentModel.DataAnnotations;

namespace ProjectMvcGroup2.ViewModels
{
    public class SearchLiftTicketsViewModel
    {
        [Display(Name = "Guest Email")]
        public string? Email { get; set; }
        public List<TicketDate> LiftTicketSearchResult { get; set; } = new();
    }
}