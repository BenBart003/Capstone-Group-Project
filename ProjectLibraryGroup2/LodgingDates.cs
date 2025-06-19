using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryGroup2
{
    public class LodgingDates
    {
        public int LodgingDatesID { get; set; }
        [Required]
        public Guest Guest { get; set; }
        //[Required]
        //public string GuestId { get; set; }
        [Required]
        public Lodging Lodging { get; set; }
        [Required]
        public DateOnly CheckInDate { get; set; }
        [Required]
        public DateOnly CheckOutDate { get; set; }
        public double TotalCost => (CheckOutDate.DayNumber - CheckInDate.DayNumber) * Lodging.CostPerNight;


        public LodgingDates() { }

        public LodgingDates(Guest guest, Lodging lodging, DateOnly checkInDate, DateOnly checkOutDate)
        {
            this.Guest = guest;
            this.Lodging = lodging;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }
}
