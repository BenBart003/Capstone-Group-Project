using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryGroup2
{
    public class Rents
    {
        public int RentsID { get; set; }
        [Required]
        public Guest Guest { get; set; }
        [Required]
        public EquipmentRental EquipmentRental { get; set; }
        [Required]
        public DateTime ERentStartDate { get; set; }
        [Required]
        public DateTime ERentEndDate { get; set; }
        public double TotalCost
        {
            get
            {
                int totalDays = (ERentEndDate.Date - ERentStartDate.Date).Days + 1;
                totalDays = Math.Max(totalDays, 1); // ensure at least 1 day is charged
                return totalDays * (EquipmentRental?.Price ?? 0);
            }
        }

        public Rents() { }

        public Rents(
            Guest guest,
            EquipmentRental equipmentRental,
            DateTime eRentStartDate,
            DateTime eRentEndDate
            )
        {
            this.Guest = guest;
            this.EquipmentRental = equipmentRental;
            this.ERentStartDate = eRentStartDate;
            this.ERentEndDate = eRentEndDate;
        }

        public static List<Rents> SearchRents(List<Rents> inputRents, string inputEmail)
        {
            return inputRents
                .Where(r => r.Guest != null &&
                            r.Guest.Email.Equals(inputEmail, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
