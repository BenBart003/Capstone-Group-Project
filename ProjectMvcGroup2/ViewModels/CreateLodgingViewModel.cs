using ProjectLibraryGroup2;
using System.ComponentModel.DataAnnotations;

namespace ProjectMvcGroup2.ViewModels
{
    public class CreateLodgingViewModel
    {
        [Required]
        public int LodgingID { get; set; }

        [Required]
        public string GuestId { get; set; }

        [Required]
        public DateOnly CheckInDate { get; set; }

        [Required]
        public DateOnly CheckOutDate { get; set; }

        // Search filters
        public DateOnly? SearchCheckInDate { get; set; }
        public DateOnly? SearchCheckOutDate { get; set; }

        public string? SearchBedCount { get; set; }
        public List<Lodging>? SearchResults { get; set; } = new();
    }
}
