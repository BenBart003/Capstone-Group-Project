using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryGroup2
{
    public class Lodging
    {
        public int LodgingID { get; set; }
        public int RoomNumber { get; set; }
        public string BedCount { get; set; }
        public double CostPerNight { get; set; }

        public Lodging() { }

        public Lodging(int roomNumber, string bedCount, double costPerNight)
        {
            this.RoomNumber = roomNumber;
            this.BedCount = bedCount;
            this.CostPerNight = costPerNight;
        }

        public static List<Lodging> SearchLodging
            (List<Lodging> inputLodging,
            List<LodgingDates> inputLodgingDates,
            int? inputRoomNumber,
            string? inputBedCount,
            double? inputCostPerNight,
            DateOnly? inputCheckInDate,
            DateOnly? inputCheckOutDate
            )
        {
            if (inputRoomNumber != null)
            {
                inputLodging = inputLodging.Where(l => l.RoomNumber == inputRoomNumber).ToList();
            }

            if (inputBedCount != null)
            {
                inputLodging = inputLodging.Where(l => l.BedCount == inputBedCount).ToList();
            }

            if (inputCostPerNight != null)
            {
                inputLodging = inputLodging.Where(l => l.CostPerNight == inputCostPerNight).ToList();
            }

            if (inputCheckInDate != null && inputCheckOutDate != null)
            {
                inputLodging = inputLodging.Where(l =>
                    !inputLodgingDates.Any(ld =>
                        ld.Lodging.LodgingID == l.LodgingID &&
                        (ld.CheckInDate < inputCheckOutDate && ld.CheckOutDate > inputCheckInDate)
                    )
                ).ToList();
            }

            return inputLodging;
        }
    }
}

