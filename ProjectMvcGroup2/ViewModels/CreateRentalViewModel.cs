using ProjectLibraryGroup2;
using System.ComponentModel.DataAnnotations;

namespace ProjectMvcGroup2.ViewModels
{
    public class CreateRentalViewModel
    {
        [Required]
        public int EquipmentRentalID { get; set; }

        [Required]
        public string GuestId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ERentStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ERentEndDate { get; set; }

        // Search filters
        [DataType(DataType.Date)]
        public DateTime? SearchStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SearchEndDate { get; set; }

        public string? SearchSize { get; set; }
        public string? SearchType { get; set; }

        public List<EquipmentRental>? SearchResults { get; set; } = new();
    }
}
